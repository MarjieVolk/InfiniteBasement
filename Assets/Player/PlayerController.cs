using System.Collections;
using System.Collections.Generic;
using Stopwatch = System.Diagnostics.Stopwatch;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    const float WEB_BUILD_MOUSE_SENSITIVITY_MULTIPLIER = 0.2f;
    const float IN_EDITOR_MOUSE_SENSITIVITY_MULTIPLIER = 1;

    public static PlayerController instance;

    public GameObject fpCamera;
    public GameObject lowerDoorRenderCamera;

    public float speedForward = 1.5f;
    public float speedBackward = 0.75f;
    public float speedSideways = 0.75f;

    public float mouseSensitivityX = 1.0f;
    public float mouseSensitivityY = 1.0f;

    public float mouseSmoothing = 2.0f;

    public float maxRotationY = 60;

    private Vector2 rotationAngles;

    private Vector2 smoothV;

    private CharacterController characterController;
    private AudioSource audioSource;

    public float gravityMultiplier = 1;
    public bool canJump = true;
    public float jumpSpeed = 3;

    private float velocityY = 0;

    // Seconds per step.
    public float footstepPeriodMultiplier = 0.5f;
    public AudioClip[] footstepsFloor;
    public AudioClip[] footstepsStairs;
    private float lastStepTimeMs = float.MinValue;
    private Stopwatch stopwatch;
    private int footstepsCount = 0;

    private bool isPressingForward = false;
    private bool isPressingBackward = false;
    private bool isPressingSideways = false;
    private bool isWalking = false;

    public bool hasMovedSinceTeleporting = false;
    public bool lastExitedUpperDoor = false;

    public Image fadeOutImage;

    private float timeToGoToMenu = -1;

    void Start()
    {
        PlayerController.instance = this;

        stopwatch = new Stopwatch();
        stopwatch.Start();

        characterController = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();

        // Hide the cursor.
        Cursor.lockState = CursorLockMode.Locked;
        // Hide the capsule that's rendered for dev-purposes in the editor.
        GameObject.Find("DevIndicator").SetActive(false);

        fadeOutImage.CrossFadeAlpha(0, 1, false);
    }

    void Update()
    {
        Translate();
        Rotate();
        HandleFootsteps();
        HandleCursorLock();

        Vector3 rotation;
        Vector3 translation;
        RoomArrangerWithTeleport.GetTeleportTransform(false, ExitRoomTrigger.instance.GetDisplacementPastDoor(), out rotation, out translation);
        lowerDoorRenderCamera.transform.rotation = Quaternion.Euler(rotation) * fpCamera.transform.rotation;
        lowerDoorRenderCamera.transform.position = fpCamera.transform.position + translation;

        if (isPressingForward || isPressingBackward || isPressingSideways)
        {
            if (!hasMovedSinceTeleporting)
            {
                Debug.Log("Post teleport: position=" + transform.position + ", rotation=" + transform.rotation);
            }
            hasMovedSinceTeleporting = true;
        }

        if (timeToGoToMenu > 0 && timeToGoToMenu <= Time.time)
        {
            SceneManager.LoadScene("MenuScene");
        }
    }

    public void FadeToMenu()
    {
        fadeOutImage.color = Color.white;
        fadeOutImage.CrossFadeAlpha(1, 1, false);
        timeToGoToMenu = Time.time + 1;
    }

    void Translate()
    {
        Vector3 horizontalDisplacement = GetHorizontalDisplacementFromInput();
        float verticalDisplacement = GetVerticalDisplacement();
        Vector3 displacement = new Vector3(horizontalDisplacement.x, verticalDisplacement, horizontalDisplacement.z);
        characterController.Move(displacement);
    }

    float GetVerticalDisplacement()
    {
        if (characterController.isGrounded)
        {
            if (canJump && Input.GetKey(KeyCode.Space))
            {
                velocityY = jumpSpeed;
            }
            else
            {
                velocityY = 0;
            }
        }
        else
        {
            velocityY += Physics.gravity.y * Time.deltaTime;
        }

        return velocityY * Time.deltaTime;
    }

    Vector3 GetHorizontalDisplacementFromInput()
    {
        float inputZ = Input.GetAxis("Vertical");
        float inputX = Input.GetAxis("Horizontal");
        isPressingForward = inputZ > 0;
        isPressingBackward = inputZ < 0;
        isPressingSideways = inputX != 0;
        isWalking = characterController.isGrounded && (isPressingForward || isPressingBackward || isPressingSideways);
        float speedZ = inputZ > 0 ? speedForward : speedBackward;
        float displacementZ = inputZ * speedZ * Time.deltaTime;
        float displacementX = inputX * speedSideways * Time.deltaTime;
        Vector3 displacement = new Vector3(displacementX, 0, displacementZ);
        return transform.TransformDirection(displacement);
    }

    void Rotate()
    {
        float sensitivityMultiplier = Application.isEditor ? IN_EDITOR_MOUSE_SENSITIVITY_MULTIPLIER : WEB_BUILD_MOUSE_SENSITIVITY_MULTIPLIER;
        Vector2 mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * sensitivityMultiplier;
        mouseDelta = Vector2.Scale(mouseDelta, new Vector2(mouseSensitivityX * mouseSmoothing, mouseSensitivityY * mouseSmoothing));

        smoothV.x = Mathf.Lerp(smoothV.x, mouseDelta.x, 1f / mouseSmoothing);
        smoothV.y = Mathf.Lerp(smoothV.y, mouseDelta.y, 1f / mouseSmoothing);

        rotationAngles += smoothV;
        rotationAngles.y = Mathf.Clamp(rotationAngles.y, -maxRotationY, maxRotationY);

        fpCamera.transform.localRotation = Quaternion.AngleAxis(-rotationAngles.y, Vector3.right);
        transform.localRotation = Quaternion.AngleAxis(rotationAngles.x, transform.up);
    }

    void HandleFootsteps()
    {
        if (isWalking)
        {
            float walkingSpeed = isPressingForward ? speedForward : (isPressingBackward ? speedBackward : speedSideways);
            float footstepInterval = footstepPeriodMultiplier / walkingSpeed * 1000;
            if (stopwatch.ElapsedMilliseconds - lastStepTimeMs > footstepInterval)
            {
                // Switch steps sound depending on the tag on the floor.
                AudioClip[] clips = footstepsFloor;
                RaycastHit hit;
                if (Physics.Raycast(transform.position, Vector3.down, out hit))
                {
                    switch (hit.collider.gameObject.tag)
                    {
                        case "Floor":
                            clips = footstepsFloor;
                            break;
                        case "Stairs":
                            clips = footstepsStairs;
                            break;
                        default:
                            //Debug.LogWarning("Floor tag not recognized: " + hit.collider.gameObject.tag);
                            break;
                    }
                }

                int index = footstepsCount % clips.Length;
                audioSource.clip = clips[index];
                audioSource.Play();

                lastStepTimeMs = stopwatch.ElapsedMilliseconds;
                footstepsCount++;
            }
        }

        // TODO: This happens way too often...
        //if (!characterController.isGrounded)
        //{
        //    lastStepTimeMs = float.MinValue;
        //}
    }

    void HandleCursorLock()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Hide the cursor.
            Cursor.lockState = CursorLockMode.Locked;
        }
        if (Input.GetKeyDown("escape"))
        {
            // Show the cursor.
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void Teleport(Vector3 translation, Vector3 rotation)
    {
        rotationAngles.x += rotation.y;
        rotationAngles.y += rotation.x;

        // Need to disable the CharacterController while teleporting, otherwise it immediately overrides our updates.
        characterController.enabled = false;
        characterController.transform.rotation *= Quaternion.Euler(rotation);
        characterController.transform.position += translation;
        characterController.enabled = true;

        hasMovedSinceTeleporting = false;
    }

    public void OnRoomExited(bool isUpperDoorway, Vector3 displacementPastDoor)
    {
        lastExitedUpperDoor = isUpperDoorway;
        RoomArranger.instance.OnRoomExited(isUpperDoorway, displacementPastDoor);
    }

    public float GetRadius()
    {
        return characterController.radius * transform.localScale.x;
    }
}

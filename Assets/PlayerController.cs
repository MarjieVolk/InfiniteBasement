using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject fpCamera;
    public float speedForward = 3.0f;
    public float speedBackward = 1.5f;
    public float speedSideways = 1.5f;

    public float mouseSensitivityX = 2.0f;
    public float mouseSensitivityY = 2.0f;

    public float mouseSmoothing = 2.0f;

    public float maxRotationY = 60;

    private Vector2 rotationAngles;

    private Vector2 smoothV;

    private CharacterController characterController;

    public float gravityMultiplier = 1;
    public bool canJump = true;
    public float jumpSpeed = 3;

    private float velocityY = 0;

    void Start()
    {
        // Hide the cursor.
        Cursor.lockState = CursorLockMode.Locked;
        characterController = GetComponent<CharacterController>();
        GameObject.Find("DevIndicator").SetActive(false);
    }

    void Update()
    {
        Translate();
        Rotate();

        if (Input.GetKeyDown("escape"))
        {
            // Show the cursor.
            Cursor.lockState = CursorLockMode.None;
        }
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
        float speedZ = inputZ > 0 ? speedForward : speedBackward;
        float displacementZ = inputZ * speedZ * Time.deltaTime;
        float displacementX = Input.GetAxis("Horizontal") * speedSideways * Time.deltaTime;
        Vector3 displacement = new Vector3(displacementX, 0, displacementZ);
        return transform.TransformDirection(displacement);
    }

    void Rotate()
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        mouseDelta = Vector2.Scale(mouseDelta, new Vector2(mouseSensitivityX * mouseSmoothing, mouseSensitivityY * mouseSmoothing));

        smoothV.x = Mathf.Lerp(smoothV.x, mouseDelta.x, 1f / mouseSmoothing);
        smoothV.y = Mathf.Lerp(smoothV.y, mouseDelta.y, 1f / mouseSmoothing);

        rotationAngles += smoothV;
        rotationAngles.y = Mathf.Clamp(rotationAngles.y, -maxRotationY, maxRotationY);

        fpCamera.transform.localRotation = Quaternion.AngleAxis(-rotationAngles.y, Vector3.right);
        transform.localRotation = Quaternion.AngleAxis(rotationAngles.x, transform.up);
    }
}

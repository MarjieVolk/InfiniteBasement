using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    private InteractTrigger currentTarget;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision == null)
        {
            return;
        }

        InteractTrigger nextTarget = collision.transform.GetComponent<InteractTrigger>();

        if (nextTarget == null)
        {
            return;
        }

        if (nextTarget != currentTarget)
        {
            if (currentTarget != null)
            {
                currentTarget.SetIsTargeted(false);
            }

            currentTarget = nextTarget;

            if (nextTarget != null)
            {
                nextTarget.SetIsTargeted(true);
            }
        }
    }

    void OnTriggerExit(Collider collision)
    {
        InteractTrigger pastTarget = collision.transform.GetComponent<InteractTrigger>();

        if (currentTarget == pastTarget && currentTarget != null)
        {
            currentTarget.SetIsTargeted(false);
            currentTarget = null;
        }
    }
}

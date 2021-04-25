using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    void OnTriggerStay(Collider collision) {
        if (Input.GetKeyDown("e")) {
            Component comp = collision.transform.GetComponent("InteractTrigger");
            if (!comp) {
                return;
            }
            InteractTrigger trigger = comp as InteractTrigger;
            if (!trigger) {
                return;
            }

            trigger.OnInteract();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

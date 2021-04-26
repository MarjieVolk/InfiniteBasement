using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * An event that can occur due to a player interaction.
 *
 * Generally, you'll want to use or define a subclass of this class.
 */
public abstract class Trigger : MonoBehaviour
{

    /** Actually run the trigger logic. */
    protected abstract void OnTrigger();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

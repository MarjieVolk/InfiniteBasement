using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleOnStart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MeshRenderer mesh = GetComponent<MeshRenderer>();
        if (mesh) {
            mesh.enabled = false;
        }
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        if (sprite) {
            sprite.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

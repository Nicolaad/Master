using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followHead : MonoBehaviour
{

    public float yOffset = 0f;

    // Update is called once per frame
    void Update()
    {
        Transform cam = Camera.main.transform;
        transform.position = cam.position + new Vector3(0,yOffset,0);
        transform.rotation = cam.rotation;
    }
}

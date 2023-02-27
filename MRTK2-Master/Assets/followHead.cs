using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followHead : MonoBehaviour
{

    public float yOffset = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
          Transform cam = Camera.main.transform;
        transform.position = cam.position + new Vector3(0,yOffset,0);
        transform.rotation = cam.rotation;
    }
}

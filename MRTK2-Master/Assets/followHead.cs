using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followHead : MonoBehaviour
{

  
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
          Transform cam = Camera.main.transform;
        transform.position = cam.position;
        transform.rotation = cam.rotation;
    }
}

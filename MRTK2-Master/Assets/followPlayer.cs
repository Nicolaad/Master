using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
          Transform playerTransform = Camera.main.transform;
        this.transform.position = playerTransform.position;
    }
}

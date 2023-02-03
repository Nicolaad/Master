using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startSession : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Q)) {
            Debug.Log("Creating relay");
            TestRelay.CreateRelay();
        }

        if (Input.GetKeyDown(KeyCode.P)) {
            Debug.Log("joining relay");
            //TestRelay.JoinRelay();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.Netcode;

public class playerMovement : NetworkBehaviour {
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;
        Vector3 moveDir = new Vector3(0, 0, 0);
        if (Input.GetKeyDown(KeyCode.W)) moveDir.z = +.1f;
        if (Input.GetKeyDown(KeyCode.S)) moveDir.z = -.1f;
        if (Input.GetKeyDown(KeyCode.A)) moveDir.x = -.1f;
        if (Input.GetKeyDown(KeyCode.D)) moveDir.x = +.1f;

        float moveSpeed = 20f;
        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }
}



    // Start is called before the first frame update
    
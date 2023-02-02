using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerNetwork : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) {
            return;
        }
        Vector3 moveDir = new Vector3(0, 0, 0);
        if(Input.GetKey(KeyCode.W)) moveDir.x = 0.1f;
        if(Input.GetKey(KeyCode.S)) moveDir.x = -0.1f;
        if(Input.GetKey(KeyCode.A)) moveDir.z = 0.1f;
        if(Input.GetKey(KeyCode.D)) moveDir.z = -0.1f;

        float moveSpeed = 3f;
        transform.position += moveDir * moveSpeed * Time.deltaTime;
        
    }
}

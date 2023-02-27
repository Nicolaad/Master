using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class loadPlayer : NetworkBehaviour
{

    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
         Instantiate(player);
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}

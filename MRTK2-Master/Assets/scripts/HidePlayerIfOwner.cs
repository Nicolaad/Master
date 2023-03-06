using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class HidePlayerIfOwner : NetworkBehaviour
{

    [SerializeField] private GameObject[] objectsToHide;
    [SerializeField] private MeshRenderer[] meshesToHide;
    [SerializeField] private CapsuleCollider[] collidersToHide;

    void Start()
    {
        if(IsOwner){
            foreach(GameObject g in objectsToHide){
                g.SetActive(false);
            }
            foreach(MeshRenderer m in meshesToHide){
                m.enabled = false;
            }
            foreach(CapsuleCollider c in collidersToHide){
                c.enabled = false;
            }
        }
    }

}

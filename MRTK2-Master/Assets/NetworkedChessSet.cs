using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkedChessSet : MonoBehaviour
{
     private GameObject wrapper;

    void Start()
    {
        wrapper = GameObject.Find("ClientsideScaleWrapper");
        transform.position = wrapper.transform.position;
        transform.localScale = wrapper.transform.localScale;
        transform.rotation = wrapper.transform.rotation;

    }

}

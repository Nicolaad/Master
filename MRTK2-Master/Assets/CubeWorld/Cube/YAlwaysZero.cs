using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YAlwaysZero : MonoBehaviour
{

    private Vector3 initialPos;

    private void Start() {
        initialPos = this.transform.localPosition;
    }
    
    void Update()
    {
        this.transform.localPosition = new Vector3(this.transform.localPosition.x, initialPos.y, this.transform.localPosition.z);
    }
}

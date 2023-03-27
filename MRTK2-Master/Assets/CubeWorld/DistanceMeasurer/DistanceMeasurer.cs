using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DistanceMeasurer : MonoBehaviour
{
    [SerializeField] private Transform target;

    private Vector3 _lastPosition;

    [SerializeField] private Text displayField;

    private void Update() {
        if(transform == null){
            return;
        }

        if(_lastPosition != transform.position){
            updateDisplayedDistance();
        }
    }

    private float measureXZDistance(){
        Vector3 distance = this.transform.position - target.transform.position;
        distance.y = 0;
        return distance.magnitude;
    }


    private void updateDisplayedDistance(){
        Debug.Log(measureXZDistance());
        displayField.text = measureXZDistance()*100 + "cm";
    }

    public void setTarget(Transform newTarget){
        target = newTarget;
    }


}

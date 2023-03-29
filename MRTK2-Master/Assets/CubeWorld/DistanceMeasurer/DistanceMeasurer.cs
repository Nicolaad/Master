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
        if(target == null){
            return;
        }

        if(_lastPosition != target.position){
            updateDisplayedDistance();
            _lastPosition = target.position;
        }
    }

    private float measureXZDistance(){
        Vector3 distance = this.transform.position - target.transform.position;
        distance.y = 0;
        return distance.magnitude;
    }


    private void updateDisplayedDistance(){
        float distance = measureXZDistance()*100;

        if(distance < 0.001f){
            distance = 0f;
        }
        
        displayField.text = distance+  "cm";
    }

    public void setTarget(Transform newTarget){
        target = newTarget;
    }


}

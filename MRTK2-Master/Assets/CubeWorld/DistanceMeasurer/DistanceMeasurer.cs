using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DistanceMeasurer : MonoBehaviour
{
    private Transform target;

    private Vector3 _lastPosition;

    [SerializeField] private Text displayField;
    
    [SerializeField] private GameObject goalMarkerPrefab;
    private GameObject _currentMarker;

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

    public void setMarker(Vector3 scale){
        if(_currentMarker != null){
            Destroy(_currentMarker);
        }


        _currentMarker = Instantiate(goalMarkerPrefab, transform.position, transform.rotation);
        _currentMarker.name = "TargetIndicator";
        _currentMarker.transform.SetParent(this.transform);

        _currentMarker.transform.localScale = new Vector3(scale.x, 1f, scale.z);

        //changes it so that the marker is spawned on the front right corner, like the cubes. Maket it easier to measure from on a grid
        Vector3 startLocalPos = _currentMarker.transform.localPosition;
        Vector3 adjustedStartPos = new Vector3(startLocalPos.x - scale.x/2, startLocalPos.y, startLocalPos.z + scale.z/2);
        _currentMarker.transform.localPosition = adjustedStartPos;
        
    }

}

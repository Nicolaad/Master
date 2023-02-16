using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine;
using UnityEngine.Events;

public class BoardAnchorController : MonoBehaviour
{
    [SerializeField] 
    private GameObject lockButton;
    private Camera playerCamera;
    
    //using namespace to distinguish it from OVR's ObjectManipulator class
    private Microsoft.MixedReality.Toolkit.UI.ObjectManipulator objectManipulator;

    private void Start() {
        playerCamera = Camera.main;
        addFunctionToClickEvent(lockPosition);
        lockButtonFacePlayer();
    }

    private void lockButtonFacePlayer(){
        objectManipulator = this.GetComponent<Microsoft.MixedReality.Toolkit.UI.ObjectManipulator>();
        lockButton.transform.LookAt(playerCamera.transform.position);

        //face camera + 180 degrees, as the buttons display appears on its back
        lockButton.transform.Rotate(new Vector3(0,180, 0)); 
    }

    public void handlePointMovementStart(){
        lockButton.SetActive(false);
    }

    public void handlePointMovementEnd(){
        lockButton.SetActive(true);
        lockButtonFacePlayer();
    }

    public void lockPosition(){
        lockButton.SetActive(false);
        objectManipulator.enabled = false;
    }

    public void unlockPosition(){
        lockButton.SetActive(true);
        objectManipulator.enabled = true;
    }

    public void addFunctionToClickEvent(UnityAction call){
        this.GetComponentInChildren<ButtonConfigHelper>().OnClick.AddListener(call);
    }

    public Vector3 getAnchorPosition(){
        //modify this function to change how the anchorPoint is calculated
        return this.transform.position;
    }
}

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

    private void Start() {
        playerCamera = Camera.main;
        addFunctionToClickEvent(lockPosition);
        lockButtonFacePlayer();
    }

    private void lockButtonFacePlayer(){
        //face camera + 180 degrees, so that hte buttons fron is shown, rather than the back
        lockButton.transform.LookAt(playerCamera.transform.position);
        lockButton.transform.Rotate(new Vector3(0,180, 0)); 
    }

    public void handlePointMovementStart(){
        lockButton.SetActive(false);
    }

    public void handlePointMovementEnd(){
        lockButton.SetActive(true);
        lockButtonFacePlayer();
    }

    private void lockPosition(){
        lockButton.SetActive(false);
        this.GetComponent<ObjectManipulator>().enabled = false;
    }

    public void addFunctionToClickEvent(UnityAction call){
        this.GetComponentInChildren<ButtonConfigHelper>().OnClick.AddListener(call);
    }
}

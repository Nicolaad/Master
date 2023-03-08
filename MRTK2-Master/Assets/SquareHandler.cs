using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.Input;
using Unity.Netcode;
using UnityEngine;

public class SquareHandler : MonoBehaviour,IMixedRealityPointerHandler
{
    private bool active = false;
    [SerializeField] private GameObject networkObjectParent;

    public void OnPointerDown(MixedRealityPointerEventData eventData)
    {

        Debug.Log(eventData.selectedObject);
        if(HandleActiveSquares.startSquare == null && HandleActiveSquares.SquareContainsPiece(gameObject)) {
            RequestOwnership();
            Debug.Log("active square clicked");
            HandleActiveSquares.startSquare = gameObject;
            HandleActiveSquares.startSquare.GetComponent<Renderer>().enabled = true;
            HandleActiveSquares.startSquare.GetComponent<Renderer>().material.color = new Color(200, 0, 0, 1);
        }
        else if (HandleActiveSquares.targetSquare == null && HandleActiveSquares.startSquare != null){
            Debug.Log("target square clicked");
            HandleActiveSquares.targetSquare = gameObject;
            HandleActiveSquares.targetSquare.GetComponent<Renderer>().enabled = true;
            HandleActiveSquares.targetSquare.GetComponent<Renderer>().material.color = new Color(0, 0, 200, 1);

           
        }

      
       
        active = true;
        
    }


    public void RequestOwnership(){
        OwnershipManager.Instance.RequestOwnershipOfObject(networkObjectParent);
    }
    

    public void OnPointerDragged(MixedRealityPointerEventData eventData)
    {
       
    }

    public void OnPointerUp(MixedRealityPointerEventData eventData)
    {
     
    }

    public void OnPointerClicked(MixedRealityPointerEventData eventData)
    {
       
    }
}

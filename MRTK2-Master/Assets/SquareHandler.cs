using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.Input;
using UnityEngine;

public class SquareHandler : MonoBehaviour,IMixedRealityPointerHandler
{
    bool active = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnTriggerEnter(Collider other)
    {
       
    }

    public void OnPointerDown(MixedRealityPointerEventData eventData)
    {
        if(HandleActiveSquares.startSquare == null) {
            Debug.Log("active square clicked");
            HandleActiveSquares.startSquare = gameObject;
            HandleActiveSquares.startSquare.GetComponent<Renderer>().enabled = true;
            HandleActiveSquares.startSquare.GetComponent<Renderer>().material.color = new Color(200, 0, 0, 1);
        }
        else if (HandleActiveSquares.targetSquare == null){
            Debug.Log("target square clicked");
            HandleActiveSquares.targetSquare = gameObject;
            HandleActiveSquares.targetSquare.GetComponent<Renderer>().enabled = true;
            HandleActiveSquares.targetSquare.GetComponent<Renderer>().material.color = new Color(0, 0, 200, 1);

           
        }

      
       
        active = true;
        
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

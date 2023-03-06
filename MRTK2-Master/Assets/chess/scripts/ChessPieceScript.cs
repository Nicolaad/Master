using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine;

public class ChessPieceScript : MonoBehaviour
{

    [SerializeField]
    private GameObject networkObjectParent;

    private void Start() {
        this.GetComponent<Microsoft.MixedReality.Toolkit.UI.ObjectManipulator>().OnManipulationStarted.AddListener((data) =>{RequestOwnership();});
        this.GetComponent<Microsoft.MixedReality.Toolkit.UI.ObjectManipulator>().OnManipulationEnded.AddListener((data) =>{setYPosToBoardPos();});
    }

    void Update(){
        if(this.transform.localPosition.y < 0){
            this.transform.localPosition = new Vector3(transform.localPosition.x, 0, transform.localPosition.z);
        }
    }

    public void setYPosToBoardPos(){
        //set local y to 0, which should be the starting y value. 
        this.transform.localPosition = new Vector3(transform.localPosition.x, 0, transform.localPosition.z);

        //decided to adjust the position when the piece is let go, so that hte player can twirl it. If this feels weird, then a constraint should be added to the piee so that it is always upright, even in movement.
        this.transform.localRotation = Quaternion.identity;
    }

    public void RequestOwnership(){
        OwnershipManager.Instance.RequestOwnershipOfObject(networkObjectParent);
    }

}

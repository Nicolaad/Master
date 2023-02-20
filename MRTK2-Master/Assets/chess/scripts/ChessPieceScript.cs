using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine;

public class ChessPieceScript : MonoBehaviour
{

    private void Start() {
        this.GetComponent<Microsoft.MixedReality.Toolkit.UI.ObjectManipulator>().OnManipulationEnded.AddListener((data) =>{setYPosToBoardPos();});
    }

    public void setYPosToBoardPos(){
        //set local y to 0, which should be the starting y value. 
        this.transform.localPosition = new Vector3(transform.localPosition.x, 0, transform.localPosition.z);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardFactory : MonoBehaviour
{
    [SerializeField]
    private Transform[] cornerPoints;
    
    [SerializeField]
    private GameObject board;

    [SerializeField]
    private bool continousUpdate;

    [SerializeField]
    private bool debugLines;

    public bool instantiateBoard;


    private void Update() {
        if(instantiateBoard){
            instantiateBoard = false;
            InstantiateBoard();
        }

        if(continousUpdate && board ){
            updateBoardPosition();
        }
        if(debugLines){
            DrawLines();
        }
    }

    public void InstantiateBoard(){
        if(cornerPoints.Length < 3){
            Debug.Log("could not instantiate board: not enough points ");
            return;
        }

        board = GameObject.CreatePrimitive(PrimitiveType.Cube);
        updateBoardPosition();
        return;
    }

    private void updateBoardPosition(){
        Vector3 width = cornerPoints[1].position - cornerPoints[0].position;
        Vector3 length = cornerPoints[2].position - cornerPoints[0].position;
        float height =0.1f;

        board.transform.localScale = new Vector3(Vector3.Magnitude(width), height , Vector3.Magnitude(length));
        //using setPositionAndRotation, as changing posiition for some reason does not update the thing
        board.transform.SetPositionAndRotation(cornerPoints[0].position + width/2 + length/2 , Quaternion.LookRotation((cornerPoints[0].position + width/2) - (cornerPoints[0].position + width/2 + length/2) ,  Vector3.Cross(length, width)));


    }

    private void DrawLines(){
        Debug.DrawLine(cornerPoints[0].position, cornerPoints[1].position, Color.blue);
        Debug.DrawLine(cornerPoints[0].position, cornerPoints[2].position, Color.blue);
        Vector3 width = cornerPoints[1].position - cornerPoints[0].position;
        Vector3 length = cornerPoints[2].position - cornerPoints[0].position;
        Debug.DrawRay(cornerPoints[0].position + width/2 + length/2, Vector3.Cross(length, width), Color.green);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleActiveSquares : MonoBehaviour
{

    public static GameObject startSquare;
    public static GameObject targetSquare;
    bool isMoveActive = false;
    public float speed = .2f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

     
    // Update is called once per frame
    void Update()
    {

       
       if (startSquare != null && targetSquare != null) {
        GameObject piece = FindClosestPieceOnStartSquare(startSquare);
        if( piece.transform.position != targetSquare.transform.position) {
             movePiece(piece, targetSquare);
        }
       
        }
    }

   
   public void movePiece(GameObject currentObject, GameObject targetSquare) {
    float step = speed * Time.deltaTime;
    Vector3 targetPos = targetSquare.transform.position;
    Debug.Log(currentObject.name + " is closest");
    currentObject.transform.position = Vector3.MoveTowards(currentObject.transform.position, targetPos, step);
   
    }




public static GameObject FindClosestPieceOnStartSquare(GameObject startSquare)
{
    GameObject[] piecesOnStartSquare = GameObject.FindGameObjectsWithTag("piece");
    float closestDistance = Mathf.Infinity;
    GameObject closestPiece = null;

    foreach (GameObject piece in piecesOnStartSquare)
    {
        if (piece.transform.parent.gameObject == startSquare)
        {
            float distance = Vector3.Distance(startSquare.transform.position, piece.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPiece = piece;
            }
        }
    }

    return closestPiece;
}
}

      


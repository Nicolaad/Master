using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HandleActiveSquares : MonoBehaviour
{


    public float speed = .2f;



    /*
        public void movePiece(GameObject currentObject, GameObject targetSquare)
        {
            float step = speed * Time.deltaTime;
            Vector3 targetPos = targetSquare.transform.position;
            // Debug.Log(currentObject.name + " is closest");
            currentObject.transform.position = Vector3.MoveTowards(currentObject.transform.position, targetPos, step);

        }
    */

    public static bool SquareContainsPiece(GameObject square)
    {
        return square.transform.childCount > 0;
    }

    public static GameObject getPieceInSquare(GameObject square)
    {
        GameObject[] whites = GameObject.FindGameObjectsWithTag("whitepiece");
        GameObject[] blacks = GameObject.FindGameObjectsWithTag("blackpiece");
        GameObject[] pieces = whites.Concat(blacks).ToArray();

        foreach (GameObject piece in pieces)
        {
            if (piece.transform.position == square.transform.position)
            {
                return piece;
            }
        }
        return null;
    }


    public static GameObject FindClosestPieceOnStartSquare(GameObject startSquare)
    {
        GameObject[] whites = GameObject.FindGameObjectsWithTag("whitepiece");
        GameObject[] blacks = GameObject.FindGameObjectsWithTag("blackpiece");
        GameObject[] pieces = whites.Concat(blacks).ToArray();
        //GameObject[] piecesOnStartSquare = GameObject.FindGameObjectsWithTag("whitepiece");

        float closestDistance = Mathf.Infinity;
        GameObject closestPiece = null;

        foreach (GameObject piece in pieces)
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




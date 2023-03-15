using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HandleActiveSquares : MonoBehaviour
{

    public static GameObject startSquare;
    public static GameObject targetSquare;
    public float speed = .2f;

    void Update()
    {


        if (startSquare != null && targetSquare != null)
        {
            GameObject piece = FindClosestPieceOnStartSquare(startSquare);
            if (piece.transform.position != targetSquare.transform.position)
            {
                movePiece(piece, targetSquare);
            }
            else
            {
                piece.transform.parent = targetSquare.transform;
                startSquare.GetComponent<Renderer>().enabled = false;
                targetSquare.GetComponent<Renderer>().enabled = false;
                startSquare = null;
                targetSquare = null;
            }

        }
    }


    public void movePiece(GameObject currentObject, GameObject targetSquare)
    {
        float step = speed * Time.deltaTime;
        Vector3 targetPos = targetSquare.transform.position;
        // Debug.Log(currentObject.name + " is closest");
        currentObject.transform.position = Vector3.MoveTowards(currentObject.transform.position, targetPos, step);
        checkIfPieceCaptured(currentObject);

    }


    public void checkIfPieceCaptured(GameObject currentObject)
    {
        GameObject[] whites = GameObject.FindGameObjectsWithTag("whitepiece");
        GameObject[] blacks = GameObject.FindGameObjectsWithTag("blackpiece");
        GameObject[] pieces = whites.Concat(blacks).ToArray();

        foreach (GameObject piece in pieces)
        {
            if (piece.transform.position == currentObject.transform.position && piece != currentObject)
            {
                piece.SetActive(false);
            }
        }
    }

    public static bool SquareContainsPiece(GameObject square)
    {
        return square.transform.childCount > 0;
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




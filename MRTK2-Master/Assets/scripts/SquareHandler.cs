using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.MixedReality.Toolkit.Input;
using Unity.Netcode;
using UnityEngine;

public class SquareHandler : MonoBehaviour, IMixedRealityPointerHandler
{

    [SerializeField] private float moveSpeed = .2f;
    private static GameObject startSquare;
    private static GameObject targetSquare;
    [SerializeField] private GameObject networkObjectParent;

    public void OnPointerDown(MixedRealityPointerEventData eventData)
    {

        Debug.Log(eventData.selectedObject);
        if (startSquare == null && HandleActiveSquares.getPieceInSquare(gameObject) != null)
        {
            RequestOwnership();
            Debug.Log("active square clicked");
            startSquare = gameObject;
            startSquare.GetComponent<Renderer>().enabled = true;
            startSquare.GetComponent<Renderer>().material.color = new Color(200, 0, 0, 1);
        }
        else if (targetSquare == null && startSquare != null)
        {
            Debug.Log("target square clicked");
            GameObject pieceToMove = HandleActiveSquares.getPieceInSquare(startSquare);

            targetSquare = gameObject;
            targetSquare.GetComponent<Renderer>().enabled = true;
            targetSquare.GetComponent<Renderer>().material.color = new Color(0, 0, 200, 1);

            var movePieceCoroutine = movePiece(pieceToMove, startSquare, targetSquare);
            StartCoroutine(movePieceCoroutine);
            startSquare = null;
            targetSquare = null;
        }
    }

    private void Update()
    {

    }

    public IEnumerator movePiece(GameObject currentObject, GameObject startSquare, GameObject targetSquare)
    {
        Vector3 targetPos = targetSquare.transform.position;
        Vector3 startPos = currentObject.transform.position;

        //float distance = Vector3.Distance(startPos, targetPos);
        //Debug.Log(distance);

        while (currentObject.transform.position != targetPos)
        {
            currentObject.transform.position = Vector3.MoveTowards(currentObject.transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        startSquare.GetComponent<Renderer>().enabled = false;
        targetSquare.GetComponent<Renderer>().enabled = false;
        checkIfPieceCaptured(currentObject);
    }


    public AudioClip clip = null;
    private void checkIfPieceCaptured(GameObject currentObject)
    {
        GameObject[] whites = GameObject.FindGameObjectsWithTag("whitepiece");
        GameObject[] blacks = GameObject.FindGameObjectsWithTag("blackpiece");
        GameObject[] pieces = whites.Concat(blacks).ToArray();

        foreach (GameObject piece in pieces)
        {
            if (piece.transform.position == currentObject.transform.position && piece != currentObject)
            {
                //piece.SetActive(false);
                Destroy(piece);
                Debug.Log("piece captured");
                GameObject handleactivesquares = GameObject.Find("HandleActiveSquares");
                AudioSource captureAudio = handleactivesquares.GetComponent<AudioSource>();
                captureAudio.Play();
            }
        }


    }




    public void RequestOwnership()
    {
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

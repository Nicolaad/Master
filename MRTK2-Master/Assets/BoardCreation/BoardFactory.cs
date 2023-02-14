using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardFactory : MonoBehaviour
{
    [SerializeField]
    private Transform[] cornerPoints;
    
    [SerializeField]
    private GameObject boardPrefab;

    private GameObject board;

    [SerializeField]
    private bool continousUpdate;

    public bool instantiateBoard;

    private GameObject pointerIndicator = null;


    private void Update() {
        if(instantiateBoard){
            instantiateBoard = false;
            InstantiateBoard();
        }

        if(continousUpdate && board ){
            //updateBoardPosition();
            twoPointAlgorithm(cornerPoints[0].transform.position, cornerPoints[1].transform.position);
        }
    }

    public void InstantiateBoard(){
        if(cornerPoints.Length < 3){
            Debug.Log("could not instantiate board: not enough points ");
            return;
        }
        board = Instantiate(boardPrefab);

        twoPointAlgorithm(cornerPoints[0].transform.position, cornerPoints[1].transform.position);
        return;
    }

    private void twoPointAlgorithm(Vector3 pA, Vector3 pC){
        //ignores changes in y plane
        //Assumes a square board, as it is used in chess and lessens setup time

        Vector3 crossSection = new Vector3(pC.x-pA.x, 0, pC.z-pA.z);
        Vector3 perpendicularCrossSection = new Vector3(crossSection.z, 0, -crossSection.x);
        float  crossSectionMagnitude = Vector3.Magnitude(crossSection);
        Vector3 center = pA + crossSection/2;
        Vector3 pB = center + perpendicularCrossSection/2;

        float width = Vector3.Distance(pA, pB);
        
        board.transform.localScale = new Vector3(width, width, width);
        
        Quaternion rotation = Quaternion.LookRotation((pA + (pB-pA)/2) - center, Vector3.up);
        board.transform.SetPositionAndRotation(center , rotation);



    }

}

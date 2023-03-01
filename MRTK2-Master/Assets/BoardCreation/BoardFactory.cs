using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class BoardFactory : NetworkBehaviour
{
    
    [SerializeField]
    private GameObject boardPrefab;

    [SerializeField]
    private GameObject wrapper;

    public  void InstantiateBoardBasedOnCorners(Vector3 pA, Vector3 pC){
        wrapper.SetActive(true);

        setTransformBasedOn2Corners(wrapper, pA, pC);

        if(IsServer){
            GameObject newBoard = Instantiate(boardPrefab);
            newBoard.GetComponent<NetworkObject>().Spawn();
            
        }else{
            //handles the case for when the board is spawned by the server before the client has specified an anchor
            GameObject serverSpawnedBoard = GameObject.FindGameObjectWithTag("Board");
            if(serverSpawnedBoard != null){
                serverSpawnedBoard.GetComponent<MoveBoardToWrapper>().SetBoardPositionToWrapper();

            }
        }


    }


    private void setTransformBasedOn2Corners(GameObject board ,Vector3 pA, Vector3 pC){
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

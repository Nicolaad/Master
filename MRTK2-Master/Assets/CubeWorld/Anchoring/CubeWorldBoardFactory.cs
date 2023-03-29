using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CubeWorldBoardFactory : MonoBehaviour
{

    [SerializeField]private GameObject boardPrefab, wrapper, player;

    public void InstantiateBoardBasedOnCorners(Vector3 pA, Vector3 pC)
    {
        setTransformBasedOn2Corners(wrapper, pA, pC);

        GameObject newBoard = Instantiate(boardPrefab);
        newBoard.transform.parent = wrapper.transform;
        
        //Should be little need to rescale or translate, if everything is kept consistent, optionally, it should handle that some objects should be not scaled
        RescaleWrapperContent();
        
        //transformScaleWrapperAndPlayerToWorldCenter();
    }


    private void setTransformBasedOn2Corners(GameObject board, Vector3 pA, Vector3 pC)
    {
        //ignores changes in y plane
        //Assumes a square board, as it is used in chess and lessens setup time

        Vector3 crossSection = new Vector3(pC.x - pA.x, 0, pC.z - pA.z);
        Vector3 perpendicularCrossSection = new Vector3(crossSection.z, 0, -crossSection.x);
        float crossSectionMagnitude = Vector3.Magnitude(crossSection);
        Vector3 center = pA + crossSection / 2;
        Vector3 pB = center + perpendicularCrossSection / 2;

        float width = Vector3.Distance(pA, pB);

        board.transform.localScale = new Vector3(width, width, width);

        Quaternion rotation = Quaternion.LookRotation(-((pA + (pB - pA) / 2) - center), Vector3.up);
        board.transform.SetPositionAndRotation(center, rotation);
    }


    private void RescaleWrapperContent()
    {
        //resets all the children within the wrapper, used to make sure that it properly comforms 
        for (int i = 0; i < wrapper.transform.childCount; i++)
        {
            var child = wrapper.transform.GetChild(i);
            if (child != null)
            {
                child.localPosition = Vector3.zero;
                //child.localScale = Vector3.one;
                child.localRotation = Quaternion.identity;
            }
        }
    }

    private void transformScaleWrapperAndPlayerToWorldCenter()
    {
        //moves the board to position 0,0,0 , with rotation of 0 so that it can be synchronized with the other players.
        //also moves the player the boared using parenting
        var previousParent = player.transform.parent;
        player.transform.SetParent(wrapper.transform);
        wrapper.transform.position = Vector3.zero;
        wrapper.transform.rotation = Quaternion.identity;
        player.transform.SetParent(previousParent);
    }

}


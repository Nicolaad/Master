using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.UI;
using Unity.Netcode;
using UnityEngine;

public class BoardCreationManager : NetworkBehaviour
{
    public enum State { NotStarted, DesignateFirstCorner, DesignateSecondCorner, ConfirmMarkings, Finished }
    public enum Input { Next, Back, Cancel, Reset }

    private State currentState = State.NotStarted;

    [SerializeField]
    private GameObject boardAnchorPointPrefab;

    private GameObject[] boardAnchorPoints = new GameObject[2];

    [SerializeField]
    private GameObject startButton, confirmationBox;

    [SerializeField]
    private BoardFactory boardFactory;

    public void changeAndUpdateState(Input input)
    {
        currentState = calculateState(currentState, input);
        HandeCurrentState();
    }

    private State calculateState(State current, Input input) =>
        (current, input) switch
        {
            (State.NotStarted, Input.Next) => State.DesignateFirstCorner,
            (State.DesignateFirstCorner, Input.Next) => State.DesignateSecondCorner,
            (State.DesignateFirstCorner, Input.Back) => State.DesignateFirstCorner,
            (State.DesignateSecondCorner, Input.Next) => State.ConfirmMarkings,
            (State.ConfirmMarkings, Input.Cancel) => State.NotStarted,
            (State.ConfirmMarkings, Input.Next) => State.Finished,
            (State.Finished, Input.Reset) => State.NotStarted,
            _ => throw new System.NotSupportedException(
            $"{current} has no transition on {input}")
        };


    private void HandeCurrentState()
    {
        switch (currentState)
        {
            case State.NotStarted:
                setupNotStartedState();
                break;
            case State.DesignateFirstCorner:
                setupDesignateFirstCorner();
                break;
            case State.DesignateSecondCorner:
                setupDesignteSecondCorner();
                break;
            case State.ConfirmMarkings:
                SetupConfirmMarkings();
                break;
            case State.Finished:
                SetupFinished();
                break;
            default:
                throw new System.NotSupportedException(
            $"{currentState} has no function to handle the state");
        };
    }

    private void setupNotStartedState()
    {
        clearupGeneratedElements();
        tryRemoveBoard();
        startButton.SetActive(true);
        return;
    }
    private void setupDesignateFirstCorner()
    {
        startButton.SetActive(false);

        if (boardAnchorPoints[0] is null)
        {
            boardAnchorPoints[0] = Instantiate(boardAnchorPointPrefab);
            boardAnchorPoints[0].transform.position = startButton.transform.position;
            boardAnchorPoints[0].GetComponent<BoardAnchorController>().addFunctionToClickEvent(handleNextInput);
        }
        else
        {
            boardAnchorPoints[0].GetComponent<BoardAnchorController>().unlockPosition();
        }

        if (boardAnchorPoints[1])
        {
            Destroy(boardAnchorPoints[1]);
            boardAnchorPoints[1] = null;
        }

    }
    private void setupDesignteSecondCorner()
    {
        if (boardAnchorPoints[1] is null)
        {
            boardAnchorPoints[1] = Instantiate(boardAnchorPointPrefab);
            boardAnchorPoints[1].transform.position = boardAnchorPoints[0].transform.position + new Vector3(0.0f, 0.1f, 0.0f);
            boardAnchorPoints[1].GetComponent<BoardAnchorController>().addFunctionToClickEvent(handleNextInput);
        }
        else
        {
            boardAnchorPoints[1].GetComponent<BoardAnchorController>().unlockPosition();
        }


    }
    private void SetupConfirmMarkings()
    {
        confirmationBox.SetActive(true);
    }

    private void SetupFinished()
    {
        boardFactory.InstantiateBoardBasedOnCorners(boardAnchorPoints[0].GetComponent<BoardAnchorController>().getAnchorPosition(), boardAnchorPoints[1].GetComponent<BoardAnchorController>().getAnchorPosition());
        clearupGeneratedElements();
        return;
    }

    private void clearupGeneratedElements()
    {
        if (boardAnchorPoints.Length > 0)
        {
            for (int i = 0; i < boardAnchorPoints.Length; i++)
            {
                Destroy(boardAnchorPoints[i]);
                boardAnchorPoints[i] = null;

            }
        }
        startButton.SetActive(false);
        confirmationBox.SetActive(false);

    }

    public void tryRemoveBoard()
    {
        GameObject board = GameObject.FindGameObjectWithTag("Board");
        if (board && IsServer)
        {
            Destroy(board);
        }
        else
        {
            boardFactory.hideWrapper();

        }
    }

    //need functions without input as a wrapper, so that they can be used in unity events
    public void handleNextInput()
    {
        Debug.Log("Going to next state. Current state: " + currentState + ". Action: " + Input.Next);
        changeAndUpdateState(Input.Next);
    }
    public void handleBackInput()
    {
        changeAndUpdateState(Input.Back);
    }
    public void handleCancelInput()
    {
        changeAndUpdateState(Input.Cancel);
    }

    public void handleResetInput()
    {
        changeAndUpdateState(Input.Reset);
    }

}






using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine;

public class BoardCreationManager : MonoBehaviour
{
    public enum State {NotStarted, DesignateFirstCorner, DesignateSecondCorner, ConfirmMarkings, Finished}
    public enum Input{Next, Back, Cancel}

    private State currentState = State.NotStarted;

    [SerializeField]
    private GameObject boardAnchorPointPrefab;

    private GameObject[] boardAnchorPoints = new GameObject[2];

    [SerializeField]
    private GameObject startButton, confirmationBox;

    [SerializeField]
    private BoardFactory boardFactory;


    public void changeAndUpdateState(Input input){
        currentState = calculateState(currentState, input);
        HandeCurrentState();
    }
    
    private State calculateState(State current, Input input) =>
        (current, input) switch{
            (State.NotStarted, Input.Next) => State.DesignateFirstCorner,
            (State.DesignateFirstCorner, Input.Next) => State.DesignateSecondCorner,
            (State.DesignateFirstCorner, Input.Back) => State.DesignateFirstCorner,
            (State.DesignateSecondCorner, Input.Next) => State.ConfirmMarkings,
            (State.DesignateSecondCorner, Input.Cancel) => State.NotStarted,
            (State.ConfirmMarkings, Input.Next) => State.Finished,
            _ => throw new System.NotSupportedException(
            $"{current} has no transition on {input}")
        };
    

    private void HandeCurrentState(){
        switch (currentState){
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
            default: throw new System.NotSupportedException(
            $"{currentState} has no function to handle the state");
        };
    }

    private void setupNotStartedState(){
        //TODO legg på riktig tekst på buttonen
        startButton.SetActive(true);
        return;
    }
    private void setupDesignateFirstCorner(){
        startButton.SetActive(false);
        //spawn et objekt (som konfigurerers i unity. Seet tooltip her)
        //TODO configurer objektet med tooltips + lokalisasjon
        if(boardAnchorPoints[0] is null){
            boardAnchorPoints[0] = Instantiate(boardAnchorPointPrefab);
            boardAnchorPoints[0].transform.position = startButton.transform.position + new Vector3(0, -0.2f, 0);
            boardAnchorPoints[0].GetComponentInChildren<PressableButtonHoloLens2>().ButtonPressed.AddListener(handleNextInput);
        }

        if(boardAnchorPoints[1]){
            Destroy(boardAnchorPoints[1]);
            boardAnchorPoints[1] = null;
        }

    }
    private void setupDesignteSecondCorner(){
        //spawn et objekt (som konfigurerers i unity. Seet tooltip her)
        //TODO configurer objektet med tooltips + lokalisasjon
        if(boardAnchorPoints[1] is null){
            boardAnchorPoints[1] = Instantiate(boardAnchorPointPrefab);
            boardAnchorPoints[1].transform.position = boardAnchorPoints[0].transform.position + new Vector3(0.2f, 0, 0.2f);
            boardAnchorPoints[1].GetComponentInChildren<PressableButtonHoloLens2>().ButtonPressed.AddListener(handleNextInput);
        }


    }
    private void SetupConfirmMarkings(){
        confirmationBox.SetActive(true); 
        //TODO configurere objektet med riktig tekst + lokalisasjon
        //spawn confirm boksen her (som konfgurerers i unity)
    }
    private void SetupFinished(){
        confirmationBox.SetActive(false); 

        boardFactory.InstantiateBoardBasedOnCorners(boardAnchorPoints[0].transform.position, boardAnchorPoints[1].transform.position);
       

        foreach (var point in boardAnchorPoints){
            Destroy(point);
        }
        boardAnchorPoints = null;
        Destroy(confirmationBox);
        //Kanskje sette staten tilbake på et senere tidspunkt?
        return;
    }

    //need functions without input as a wrapper, so that they can be used in unity events
    public void handleNextInput(){
        Debug.Log("Going to next state. Current state: " + currentState+". Action: " +Input.Next);
        changeAndUpdateState(Input.Next);
    }
    public void handleBackInput(){
        changeAndUpdateState(Input.Back);
    }
    public void handleCancelInput(){
        changeAndUpdateState(Input.Cancel);
    }
    
    }

    




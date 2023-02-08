using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardCreationManager : MonoBehaviour
{
    public enum State {NotStarted, DesignateFirstCorner, DesignateSecondCorner, ConfirmMarkings, Finished}
    public enum Input{Next, Back, Cancel}

    private State currentState = State.NotStarted;

    [SerializeField]
    private GameObject boardAnchorPointPrefab, startButtonPrefab, confirmationBoxPrefab;

    private GameObject[] boardAnchorPoints;
    private GameObject startButton, confirmationBox;


    public void changeAndUpdateState(State current, Input input){
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
        return;
    }
    private void setupDesignateFirstCorner(){
        //spawn et objekt (som konfigurerers i unity. Seet tooltip her)
        //TODO configurer objektet med tooltips + lokalisasjon
        if(!boardAnchorPoints[0]){
            boardAnchorPoints[0] = Instantiate(boardAnchorPointPrefab);
        }

        if(boardAnchorPoints[1]){
            Destroy(boardAnchorPoints[1]);
            boardAnchorPoints[1] = null;
        }

    }
    private void setupDesignteSecondCorner(){
        //spawn et objekt (som konfigurerers i unity. Seet tooltip her)
        //TODO configurer objektet med tooltips + lokalisasjon
        if(!boardAnchorPoints[1]){
            boardAnchorPoints[1] = Instantiate(boardAnchorPointPrefab);
        }


    }
    private void SetupConfirmMarkings(){
        Instantiate(boardAnchorPointPrefab); 
        //TODO configurere objektet med riktig tekst + lokalisasjon
        //spawn confirm boksen her (som konfgurerers i unity)
        return;
    }
    private void SetupFinished(){
        //TODO instantiate boards with points positions

        foreach (var point in boardAnchorPoints){
            Destroy(point);
        }
        boardAnchorPoints = null;
        Destroy(confirmationBox);
        //instansier det fysiske brettet. 
        //fjern alle markerts 
        
        //Kanskje sette staten tilbake på et senere tidspunkt?
        return;
    }

    }




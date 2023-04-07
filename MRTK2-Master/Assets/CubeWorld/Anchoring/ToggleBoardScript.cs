using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleBoardScript : MonoBehaviour
{
    [SerializeField] private Renderer boardRenderer;

    public void toggleBoard(){
        boardRenderer.enabled = !boardRenderer.enabled;
    }
}

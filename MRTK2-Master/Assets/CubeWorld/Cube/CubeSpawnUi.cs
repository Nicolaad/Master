using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine;

public class CubeSpawnUi : MonoBehaviour
{
    [SerializeField] private CubeSpawner cubeSpawner;

    [SerializeField] private ButtonConfigHelper instantiateButtonConfig; //used to update current size in spawn

    private void Start() {
        updateSizeLabel();
    }

    public void handleIncreasedClick(){
        cubeSpawner.incrementSelectedCubeIndex();
        updateSizeLabel();
    }

    public void handleDecreasedClick(){
        cubeSpawner.decrementSelectedCubeIndex();
        updateSizeLabel();        
    }

    public void handleInstantiateClick(){
        cubeSpawner.clearCubes();
        cubeSpawner.spawnCube();

    }

    private void updateSizeLabel(){
        instantiateButtonConfig.MainLabelText = "Spawn " + cubeSpawner.getCurrentCubeSize() + "cm";
    }
}

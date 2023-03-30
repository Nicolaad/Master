using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] 
    private GameObject cubePrefab;
    [SerializeField, Range(1, 10)] //change in editor only when debuging

    private int currentSelectedCubeIndex = 0;
    private List<GameObject> instantiatedCubes = new List<GameObject>();
    private static float[] CUBE_SIZES = {0.01f, 0.015f, 0.02f, 0.03f, 0.04f, 0.05f, 0.06f, 0.07f, 0.08f, 0.09f, 0.10f};
    

    [SerializeField] private DistanceMeasurer measurePoint;

    [Button]
    public void spawnCube(){

        float cubeSize = getCurrentCubeSize();

        GameObject cube = Instantiate(cubePrefab, transform.position, transform.rotation);
        cube.transform.SetParent(this.transform);
        cube.transform.localPosition= new Vector3(-cubeSize/2, cubeSize/2, cubeSize/2);
        cube.transform.SetParent(null);
        
        //NB, important that the cube is not withinb a scaled parent!
        cube.transform.localScale = Vector3.one * (getCurrentCubeSize());
        cube.name = "Cube";
        Transform target = cube.transform.GetChild(0); //unsafe, but should work as the cube is limited
        measurePoint.setTarget(target);
        measurePoint.setMarker(cube.transform.localScale);
        instantiatedCubes.Add(cube);
        
    }

    [Button]
    public void clearCubes(){
        foreach(GameObject go in instantiatedCubes){
            Destroy(go);
        }
        instantiatedCubes.Clear();
    }

    public float getCurrentCubeSize(){
        return CUBE_SIZES[currentSelectedCubeIndex];
    }

    public void incrementSelectedCubeIndex(){
        currentSelectedCubeIndex++;
        if(currentSelectedCubeIndex >= CUBE_SIZES.Length){
            currentSelectedCubeIndex = CUBE_SIZES.Length-1;
        }
        
    }
    public void decrementSelectedCubeIndex(){
        currentSelectedCubeIndex--;
        if(currentSelectedCubeIndex <= 0){
            currentSelectedCubeIndex = 0;
        }
    }


}

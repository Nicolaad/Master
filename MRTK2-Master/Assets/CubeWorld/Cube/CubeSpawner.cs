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
    private static float[] CUBE_SIZES = {1f, 1.5f, 2f, 3f, 4f, 5f, 6f, 7f, 8f, 9f, 10f};
    

    [SerializeField] private DistanceMeasurer measurePoint;

    [Button]
    public void spawnCube(){
        GameObject cube = Instantiate(cubePrefab, transform.position, transform.rotation);
        
        //NB, important that the cube is not withinb a scaled parent!
        cube.transform.localScale = Vector3.one * (getCurrentCubeSize()/100f);
        measurePoint.setTarget(cube.transform);
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

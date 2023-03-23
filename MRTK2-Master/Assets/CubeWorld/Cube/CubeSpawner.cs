using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] private GameObject cubePrefab;
    
    private List<GameObject> instantiatedCubes = new List<GameObject>();
    
    [SerializeField, Range(1, 10)]
    private float selectedCubeSize =1f;



    [Button]
    public void spawnCube(){
        GameObject cube = Instantiate(cubePrefab, transform.position, transform.rotation);
        
        //NB, important that the cube is not withinb a scaled parent!
        cube.transform.localScale = Vector3.one * (selectedCubeSize/100f);
        
        instantiatedCubes.Add(cube);
        
    }

    [Button]
    public void clearCubes(){
        foreach(GameObject go in instantiatedCubes){
            Destroy(go);
        }
        instantiatedCubes.Clear();
    }



}

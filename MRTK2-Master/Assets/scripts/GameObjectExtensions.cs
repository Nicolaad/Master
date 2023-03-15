using System.Collections.Generic;
using UnityEngine;

public static class GameObjectExtensions
{
    public static List<GameObject> getChildrenWithMeshRenderers(GameObject parent)
    {
        if (parent == null || parent.transform == null)
        {
            return new List<GameObject>();
        }

        List<GameObject> objectsWithMeshRenderers = new List<GameObject>();
        foreach (GameObject child in parent.transform)
        {
            MeshRenderer meshRenderer = child.GetComponent<MeshRenderer>();
            if (meshRenderer)
            {
                objectsWithMeshRenderers.Add(child);
                getChildrenWithMeshRenderers(child);
            }
        }
        return objectsWithMeshRenderers;
    }
}
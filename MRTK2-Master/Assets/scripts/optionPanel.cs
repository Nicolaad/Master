using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEditor;
using UnityEngine;

public class optionPanel : NetworkBehaviour
{


    // default values
    bool renderAvatar = true;
    bool physicalBoard = true;
    bool renderBoard = true;
    bool isWhite = true;

    [SerializeField] private GameObject OVRprefab;

    [SerializeField] private GameObject boardPhysical;

    [SerializeField] private GameObject touchBoard;

    public static GameObject activeBoard;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void toggleBoardRenderer()
    {
        Debug.Log("button clicked");

        try
        {
            GameObject board = GameObject.Find("Board");
            Debug.Log("found board");
            MeshRenderer boardRenderer = board.GetComponent<MeshRenderer>();
            if (boardRenderer.enabled)
            {
                boardRenderer.enabled = false;
            }
            else
            {
                boardRenderer.enabled = true;
            }

        }

        catch
        {
            Debug.Log("could not find board");
        }

    }


    public void togglePieceRenderer(string tag)
    {
        GameObject[] pieces = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject piece in pieces)
        {
            MeshRenderer pieceRenderer = piece.GetComponent<MeshRenderer>();
            if (pieceRenderer)
            {
                if (pieceRenderer.enabled)
                {
                    pieceRenderer.enabled = false;
                }
                else
                {
                    pieceRenderer.enabled = true;
                }
            }
        }
    }



    public static List<GameObject> getChildrenWithMeshRenderers(GameObject parent)
    {
        if (parent == null || parent.transform == null)
        {
            return new List<GameObject>();
        }

        List<GameObject> objectsWithMeshRenderers = new List<GameObject>();
        foreach (Transform child in parent.transform)
        {
            MeshRenderer meshRenderer = child.GetComponent<MeshRenderer>();
            if (meshRenderer)
            {
                objectsWithMeshRenderers.Add(child.gameObject);
                getChildrenWithMeshRenderers(child.gameObject);
            }
        }
        return objectsWithMeshRenderers;
    }


    public void ToggleMeshRenderers()
    {

        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in playerObjects)
        {
            if (IsOwner)
            {
                Debug.Log(player.name);
                foreach (MeshRenderer renderer in player.GetComponentsInChildren<MeshRenderer>())
                {
                    renderer.enabled = !renderer.enabled;


                }
                foreach (SkinnedMeshRenderer head in player.GetComponentsInChildren<SkinnedMeshRenderer>())
                {
                    if (head)
                    {
                        head.enabled = !head.enabled;
                    }
                }


            }

        }

    }

    public void togglePassthrough()
    {
        if (OVRManager.instance.isInsightPassthroughEnabled)
        {
            OVRManager.instance.isInsightPassthroughEnabled = false;
        }
        else
        {
            OVRManager.instance.isInsightPassthroughEnabled = true;
        }
    }




    public void toggleBoardOnOff()
    {
        Debug.Log("board toggler clicked");
        //BoardFactory.changeBoard();
        List<GameObject> objectsInScene = GetNonSceneObjects();
        GameObject board = null;
        foreach (GameObject o in objectsInScene)
        {
            if (o.tag == "Board")
            {
                Debug.Log("found board");
                board = o;
                Debug.Log(board.name);
                if (board.activeSelf)
                {
                    board.SetActive(false);
                    Debug.Log("board disabled");
                }
                else
                {
                    Debug.Log("board enabled");
                    board.SetActive(true);
                }

            }

        }


    }

    List<GameObject> GetNonSceneObjects()
    {
        List<GameObject> objectsInScene = new List<GameObject>();

        foreach (GameObject go in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
        {
            objectsInScene.Add(go);
            /*if (EditorUtility.IsPersistent(go.transform.root.gameObject) && !(go.hideFlags == HideFlags.NotEditable || go.hideFlags == HideFlags.HideAndDontSave))
                objectsInScene.Add(go);*/
        }

        return objectsInScene;
    }

    public void changePlayerColor()
    {
        GameObject board = GameObject.FindGameObjectWithTag("Board");
        board.transform.Rotate(0, 180, 0);
    }

    public void changeBoardType()
    {




    }
}
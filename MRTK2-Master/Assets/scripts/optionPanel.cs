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


    private SerializeField[] boardPhysical;
    private SerializeField[] touchBoard;

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
            if (!IsOwner)
            {
                Debug.Log(player.name);
                foreach (MeshRenderer renderer in player.GetComponentsInChildren<MeshRenderer>())
                {
                    renderer.enabled = !renderer.enabled;
                }
            }
        }

    }
    public void toggleAvatarRenderer()
    {
        {
            Debug.Log("clicked avatar toggle button");
            // Get all connected clients
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("Player"))
            {
                // Check if the network object belongs to another player
                if (go.GetComponent<NetworkObject>().IsLocalPlayer)
                {
                    List<GameObject> objectsWithRenderers = getChildrenWithMeshRenderers(go);
                    foreach (GameObject renderobject in objectsWithRenderers)
                    {
                        Debug.Log("objects with renderers:" + renderobject.name);
                        if (renderobject.GetComponent<Renderer>().enabled)
                        {
                            renderobject.GetComponent<Renderer>().enabled = false;
                        }
                        else
                        {
                            renderobject.GetComponent<Renderer>().enabled = true;
                        }
                    }
                }
            }
        }

    }


    // disables player prefab rendering

    public void toggleAvatarOnOff()
    {

        // Get all connected clients
        foreach (var networkClient in NetworkManager.Singleton.ConnectedClientsList)
        {
            // Check if the network object belongs to another player
            if (!networkClient.PlayerObject.GetComponent<NetworkObject>().IsLocalPlayer)
            {
                NetworkObject playerObject = networkClient.PlayerObject;

                foreach (Transform child in playerObject.GetComponentInChildren<Transform>())
                {
                    foreach (Transform grandchild in child.GetComponentInChildren<Transform>())
                    {
                        Debug.Log(child);
                        Renderer renderer = grandchild.GetComponent<Renderer>();
                        if (renderer != null)
                        {
                            if (renderer.enabled)
                            {
                                renderer.enabled = false;
                            }
                            else
                            {
                                renderer.enabled = true;
                            }

                        }
                    }



                }

            }
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
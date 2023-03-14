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
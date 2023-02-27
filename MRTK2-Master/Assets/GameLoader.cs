using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoader : NetworkBehaviour
{
    // Start is called before the first frame update


    public void LoadGame() {
        SceneManager.LoadScene(1);
    }
}

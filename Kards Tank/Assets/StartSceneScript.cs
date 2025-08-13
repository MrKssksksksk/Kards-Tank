using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneScript : MonoBehaviour
{
    public void loadMainScene()
    {
        // Scene currentScene = SceneManager.GetActiveScene();
        Debug.Log("button pressed.");
        SceneManager.LoadScene("MainScene");
    }
}

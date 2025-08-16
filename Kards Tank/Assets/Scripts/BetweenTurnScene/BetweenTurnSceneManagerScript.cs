using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BetweenTurnSceneManagerScript : MonoBehaviour
{
    public void OnNextTurnButtonClick()
    {


        SceneManager.LoadScene("MainScene");
    }
}

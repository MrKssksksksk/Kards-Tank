using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameoverManageScript : MonoBehaviour
{
    public GameObject p1, p2;

    private void Update()
    {
        if (p1.GetComponent<TankDataScript>().cHP < 0)
        {
            p2Win();
        }
        else if (p2.GetComponent<TankDataScript>().cHP < 0)
        {
            p1Win();
        }
    }

    private void saveData()
    {
        if (Static.playerDatas.Count != 2)
        {
            if (Static.playerDatas.Count == 1) Static.playerDatas.RemoveAt(0);
            Static.playerDatas.Add(p1.GetComponent<TankDataScript>());
            Static.playerDatas.Add(p2.GetComponent<TankDataScript>());
        }
        else
        {
            Static.playerDatas[0] = p1.GetComponent<TankDataScript>();
            Static.playerDatas[1] = p2.GetComponent<TankDataScript>();
        }
    }

    [ContextMenu("p1 win")]
    public void p1Win()
    {
        saveData();
        Static.p1Score++;
        SceneManager.LoadScene("MainScene"); // change to "BetweenTurnScene" after its finished
    }
    [ContextMenu("p2 win")]
    public void p2Win()
    {
        saveData();
        Static.p2Score++;
        SceneManager.LoadScene("MainScene");
    }
}

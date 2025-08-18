using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameoverManageScript : MonoBehaviour
{
    public GameObject p1, p2;
    public AudioManagerScript audioManagerScript;

    private void Start()
    {
        p1 = GameObject.FindGameObjectWithTag("Player1");
        p2 = GameObject.FindGameObjectWithTag("Player2");
    }

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
        Debug.Log("Save Data");
        //for (int i = 0; i < 2 - StaticData.Instance.playerDatas.Count; i++) StaticData.Instance.playerDatas.Add(new TankEssentialData());
        //StaticData.Instance.playerDatas[0].input(p1.GetComponent<TankDataScript>());
        //StaticData.Instance.playerDatas[1].input(p2.GetComponent<TankDataScript>());
        if (StaticData.Instance.playerDatas.Count < 1) StaticData.Instance.playerDatas.Add(new TankEssentialData(p1.GetComponent<TankDataScript>()));
        else StaticData.Instance.playerDatas[0].input(p1.GetComponent<TankDataScript>());
        if (StaticData.Instance.playerDatas.Count < 2) StaticData.Instance.playerDatas.Add(new TankEssentialData(p2.GetComponent<TankDataScript>()));
        else StaticData.Instance.playerDatas[1].input(p2.GetComponent<TankDataScript>());
        Debug.Log("Save Data Succeed");
    }

    [ContextMenu("p1 win")]
    public void p1Win()
    {
        audioManagerScript.PlaySfx(17); // 总部爆炸
        p2.GetComponent<TankDataScript>().cHP = 9999; // 反正TankDataScript.Start()里面会重置，这里是防止再次触发Gameover
        saveData();
        StaticData.Instance.p1Score++;
        StaticData.Instance.turn++;
        SceneManager.LoadScene("BetweenTurnScene");
    }

    [ContextMenu("p2 win")]
    public void p2Win()
    {
        audioManagerScript.PlaySfx(17); // 总部爆炸
        p1.GetComponent<TankDataScript>().cHP = 9999;
        saveData();
        StaticData.Instance.p2Score++;
        StaticData.Instance.turn++;
        SceneManager.LoadScene("BetweenTurnScene");
    }
}

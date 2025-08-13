using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticData : MonoBehaviour
{
    public static StaticData Instance;

    public List<TankEssentialData> playerDatas = new List<TankEssentialData>();
    public int p1Score = 0, p2Score = 0;
    public int turn = 1;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 保持数据在场景切换时不被销毁
        }
        else
        {
            Destroy(gameObject); // 防止重复
        }
    }
}

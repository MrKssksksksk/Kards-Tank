using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ItemManagerScript : MonoBehaviour
{
    //道具生成
    public GameObject resourcePoint;
    public int spawnTime;
    private float spawnTimer;
    public List<int> spawnWeight = new List<int>(); // length not determined yet
    private int weightSum;

    void Start()
    {
        spawnTimer = 0;
    }

    void Update()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer > spawnTime)
        {
            spawnTimer = 0;
            generateResourcePoint();
        }
    }

    private int randomId()
    {
        weightSum = spawnWeight.Sum();
        int x = Random.Range(1, weightSum + 1);
        int s = 0;
        for (int i = 0; i < spawnWeight.Count; i++)
        {
            s += spawnWeight[i];
            if (x <= s)
            {
                return i;
            }
        }
        return -1;
    }

    [ContextMenu("Generate Resource Point")]
    private void generateResourcePoint()
    {
        GameObject r = Instantiate(resourcePoint);
        r.GetComponent<ItemMapScript>().getPointId(randomId());
    }

    [ContextMenu("Generate 5 Resource Point")]
    private void generate5ResourcePoint()
    {
        for (int i = 0; i < 5; i++) generateResourcePoint();
    }

}

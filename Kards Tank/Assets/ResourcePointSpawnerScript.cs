using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourcePointSpawnerScript : MonoBehaviour
{
    public GameObject resourcePoint;
    public int spawnTime;
    private float spawnTimer;
    public List<int> spawnWeight = new List<int>(); // length not determined yet
    private int weightSum;

    // Start is called before the first frame update
    void Start()
    {
        spawnTimer = 0;
    }

    // Update is called once per frame

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
        for (int i = 0;i < spawnWeight.Count;i++)
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
        r.GetComponent<ResourcePointScript>().getPointId(randomId());
    }

    [ContextMenu("Generate 5 Resource Point")]
    private void generate5ResourcePoint()
    {
        for (int i = 0; i < 5; i++) generateResourcePoint();
    }
}

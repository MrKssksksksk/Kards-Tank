using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankEffectScript : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public List<Sprite> sprites = new List<Sprite>();
    // public List<float> showingTimes = new List<float>();
    public float showingTime;
    private float createTime;
    private Transform tank;

    public void getType(int type, float s, Transform t)
    {
        spriteRenderer.sprite = sprites[type];
        // showingTime = showingTimes[type];
        showingTime = s;
        tank = t;
    }

    public void Start()
    {
        createTime = Time.time;
    }

    public void Update()
    {
        transform.position = tank.position;
        if (Time.time - createTime > showingTime)
        {
            Destroy(gameObject);
        }
    }
}

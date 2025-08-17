using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ItemMapScript : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public List<Sprite> sprites = new List<Sprite>();
    public int id;
    public int existenceTime;
    private float existenceTimer = 0;

    private void Update()
    {
        existenceTimer += Time.deltaTime;
        if (existenceTimer > existenceTime)
        {
            Destroy(gameObject);
        }
    }

    public void getPointId(int _id)
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
        id = _id;
    }

    private void Start() // actually run after getPointId()
    {
        randomizePosition();
        spriteRenderer.sprite = sprites[id];
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (existenceTimer < 0.1f)
        {
            existenceTimer = 0;
            randomizePosition();
            return;
        }
        if (collision.gameObject.layer == 6) // Player
        {
            // TankDataScript tankDataScript = collision.gameObject.GetComponent<TankDataScript>();
            TankLogicScript tankLogicScript = collision.gameObject.GetComponent<TankLogicScript>();
            
            tankLogicScript.giveItem(id);
            // 后续音效可以添加


            Destroy(gameObject);
        }
    }

    private void randomizePosition()
    {
        transform.position = new Vector3(-7.5f + Random.Range(0, 16), -3.5f + Random.Range(0, 8), 0);
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEditor.Experimental.RestService;
using UnityEngine;
using DG.Tweening;
using JetBrains.Annotations;
using UnityEditor.UI;
using System.Threading.Tasks;
using static UnityEditor.Progress;

public class ItemAniScript : MonoBehaviour
{
    public SpriteRenderer SelfRenderer;
    public float P1Targetx;
    public float P2Targetx;
    public float Width;
    public float Y;
    public float TargetRotation;
    public ItemLogicScript itemLogicScript;
    // public List<Sprite> sprites;
    private int slot;


    async void Start()
    {
        DOTween.Init();
        SelfRenderer = GetComponent<SpriteRenderer>();
        slot = getSlot();
        setSprite(itemLogicScript.data.Id);
        // await DrawCard();

        // test
        // ItemData e = GameObject.FindGameObjectWithTag("ItemManager").GetComponent<ItemDataScript>().Example1;
        // await UseCard();
    }

    public async Task DrawCard()
    {
        await DrawCardAmine(itemLogicScript.ownerIndex);
    }

    private async Task DrawCardAmine(int playerIndex) //抽卡动画,异步函数，具体概念问ai
    {
        transform.position = new Vector3(0, -5.6f, 0);
        transform.rotation = Quaternion.Euler(0, 90f, 0);
        Sequence seq = DOTween.Sequence();
        float Target_x = playerIndex == 0 ?
        P1Targetx + (Width * (slot)) :
        P2Targetx - (Width * (slot)); 
        float Target_y = Y;
        //此行往下均为动画
        seq.Append(transform.DOLocalRotateQuaternion(
                Quaternion.Euler(Vector3.zero),
                0.1f)); //旋转到0度
        if (transform.position.x > Target_x)
        {
            seq.Append(transform.DOLocalRotateQuaternion(
                Quaternion.Euler(new Vector3(0, 0, TargetRotation)),
                0.1f)); //第一个角度，第二个是在n秒内,移动是一样的
        }
        else
        {
            seq.Append(transform.DOLocalRotateQuaternion(
                Quaternion.Euler(new Vector3(0, 0, -TargetRotation)),
                0.1f));
        }
        seq.Append(transform.DOMoveX(Target_x, 1f)).SetEase(Ease.InOutQuad); //坐标移动
        seq.Append(transform.DOLocalRotateQuaternion(
                Quaternion.Euler(Vector3.zero),
                0.1f)); //旋转到0度
        seq.OnComplete(() => Debug.Log("Card draw animation completed"));
        seq.Play();

        await seq.AsyncWaitForCompletion();
    }

    public async Task UseCard()
    {
        await UseCardAnime();
    }

    private async Task UseCardAnime()
    {
        Sequence seq = DOTween.Sequence();
        seq.Join(SelfRenderer.DOFade(0f, 0.8f)); //第一个是透明度
        seq.Join(transform.DOLocalMoveY(transform.localPosition.y + 1f, 0.8f));
        seq.OnComplete(() => Debug.Log("Card use animation completed"));
        seq.Play();

        await seq.AsyncWaitForCompletion();
    }

    private async Task updatePosition()
    {
        
    }

    private int getSlot()
    {
        return itemLogicScript.slot;
    }

    public void setSprite(int itemId)
    {
        // SelfRenderer.sprite = sprites[itemId];
        SelfRenderer.sprite = itemLogicScript.data.sprite;
    }


    private void Update()
    {
        if (getSlot() != slot) // 当前一个道具被使用时，需要平移  此时logicScript.slot会改变
        {
            slot = getSlot();
            updatePosition();
        }
    }

}


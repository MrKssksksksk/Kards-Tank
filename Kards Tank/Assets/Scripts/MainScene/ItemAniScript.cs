using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Dependencies.NCalc;
// using UnityEditor.Experimental.RestService;
using UnityEngine;
using DG.Tweening;
using JetBrains.Annotations;
// using UnityEditor.UI;
using System.Threading.Tasks;
using static UnityEngine.GraphicsBuffer;
// using static UnityEditor.Progress;

public class ItemAniScript : MonoBehaviour
{
    public SpriteRenderer SelfRenderer;
    public float P1Targetx;
    public float P2Targetx;
    public float Width;
    public float Y;
    public float P1SurplusX;
    public float P2SurplusX;
    public float ChooseHeight; // 被选中时升高的高度
    public float TargetRotation;
    public ItemLogicScript itemLogicScript;
    // public List<Sprite> sprites;
    private int slot;
    private int playerIndex;


    void Start()
    {
        DOTween.Init();
        //SelfRenderer = GetComponent<SpriteRenderer>();
        //slot = getSlot();
        //playerIndex = itemLogicScript.owner.GetComponent<TankDataScript>().playerIndex;
        //setSprite(itemLogicScript.MyData);
    }

    public void ChooseCard()
    {
        transform.DOMoveY(Y + ChooseHeight, 0.1f)
        .SetEase(Ease.InOutQuad)
        .OnComplete(() => Debug.Log("ChooseCard Anime Complete"));
    }

    public void UnChooseCard()
    {
        transform.DOMoveY(Y, 0.1f)
        .SetEase(Ease.InOutQuad)
        .OnComplete(() => Debug.Log("card unchoose anime complete"));
    }

    public void DrawCard() //这个封装是多余的
    {
        DrawCardAmine();
    }

    public void DrawCardAmine() //抽卡动画,异步函数，具体概念问ai
    {
        transform.position = new Vector3(0, -5.6f, 0);
        transform.rotation = Quaternion.Euler(0, 90f, 0);
        gotoSlot(slot);
    }

    public void DrawSurplusCardAnime(bool doRotate = true) //爆牌动画
    {
        transform.position = new Vector3(0, -5.6f, 0);
        transform.rotation = Quaternion.Euler(0, 90f, 0);
        Sequence seq = DOTween.Sequence();
        float Target_x = playerIndex == 0 ? P1SurplusX : P2SurplusX;
        float Target_y = Y;
        if (doRotate)
        {
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
        }
        seq.Append(transform.DOMoveX(Target_x, 1f)).SetEase(Ease.InOutQuad);
        seq.Append(transform.DOLocalRotateQuaternion(
                Quaternion.Euler(Vector3.zero),
                0.1f));
        seq.AppendInterval(0.3f);
        seq.Append(transform.DOMoveY(transform.position.y + 0.2f, 0.3f)).SetEase(Ease.InOutQuad);
        seq.Append(transform.DOMoveY(transform.position.y - 4f, 0.5f)).SetEase(Ease.InOutQuad);
        seq.OnComplete(() => Debug.Log("Card use animation completed"));
        seq.Play();
    }

    public void UseCard()
    {
        UseCardAnime();
    }

    private void UseCardAnime()
    {
        Sequence seq = DOTween.Sequence();
        seq.Join(SelfRenderer.DOFade(0f, 0.8f)); //第一个是透明度
        seq.Join(transform.DOLocalMoveY(transform.localPosition.y + 1f, 0.8f));
        seq.OnComplete(() => Debug.Log("Card use animation completed"));
        seq.Play();
    }

    private void gotoSlot(int targetSlot, bool doRotate = true)
    {
        Sequence seq = DOTween.Sequence();
        float Target_x = playerIndex == 0 ?
        P1Targetx + (Width * targetSlot) :
        P2Targetx - (Width * targetSlot);
        float Target_y = Y;
        //此行往下均为动画
        seq.Append(transform.DOLocalRotateQuaternion(
                Quaternion.Euler(Vector3.zero),
                0.1f)); //旋转到0度
        if (doRotate)
        {
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
        }
        seq.Append(transform.DOMoveX(Target_x, 1f)).SetEase(Ease.InOutQuad); //坐标移动
        seq.Append(transform.DOLocalRotateQuaternion(
                Quaternion.Euler(Vector3.zero),
                0.1f)); //旋转到0度
        seq.OnComplete(() => Debug.Log("Card draw animation completed"));
        seq.Play();
    }

    private int getSlot()
    {
        return itemLogicScript.slot;
    }

    public void setSprite(ItemData itemData)
    {
        // SelfRenderer.sprite = sprites[itemId];
        SelfRenderer.sprite = itemData.sprite;
    }


    private void Update()
    {
        SelfRenderer.sortingOrder = slot + 7;
        /*
         * sortingOrder 即为Renderer里面的Order in Layer 即图层序号，数字越大越在上面
         * "layer order.txt"中标明了各个图层的序号
         * 7为item1，8为item2，9为item3
         * 
         */
        if (getSlot() != slot) // 当前一个道具被使用时，需要平移  此时logicScript.slot会改变
        {
            slot = getSlot();
            gotoSlot(slot, false);
        }
    }

}


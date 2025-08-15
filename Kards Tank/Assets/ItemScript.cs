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

public class ItemScript : MonoBehaviour
{
    public SpriteRenderer SelfRenderer;
    public float P1Targetx = -11.45f;
    public float P2Targetx = 11.45f;
    public float Width = 1.1f;
    public float Y = -5.6f;
    public float TargetRotation = 10f;
    public TankDataScript PlayerData;
    public GameObject P1Tank; //test


    async void Start() //test
    {
        DOTween.Init();
        SelfRenderer = GetComponent<SpriteRenderer>();
        ItemData e = GameObject.FindGameObjectWithTag("ItemManager")
        .GetComponent<ItemDataScript>()
        .Example1;
        await DrawCard(P1Tank, e);
        await UseCard(P1Tank, e);
    }

    public async Task DrawCard(GameObject Player, ItemData ItemData)
    {
        await DrawCardAmine(Player);
    }

    public async Task DrawCardAmine(GameObject Player) //抽卡动画,异步函数，具体概念问ai
    {
        transform.position = new Vector3(0, -5.6f, 0);
        transform.rotation = Quaternion.Euler(0, 90f, 0);
        Sequence seq = DOTween.Sequence();
        PlayerData = Player.GetComponent<TankDataScript>();
        float Target_x = PlayerData.playerIndex == 0 ?
        P1Targetx + (Width * (PlayerData.items.Count())) :
        P2Targetx - (Width * (PlayerData.items.Count())); //测试！测试！实际使用时.Count()需加上-1
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

    public async Task UseCard(GameObject Player, ItemData ItemData = null)
    {
        await UseCardAnime(Player);
    }

    public async Task UseCardAnime(GameObject Player)
    {
        Sequence seq = DOTween.Sequence();
        seq.Join(SelfRenderer.DOFade(0f, 0.8f)); //第一个是透明度
        seq.Join(transform.DOLocalMoveY(transform.localPosition.y + 1f, 0.8f));
        seq.OnComplete(() => Debug.Log("Card use animation completed"));
        seq.Play();

        await seq.AsyncWaitForCompletion();
    }

}


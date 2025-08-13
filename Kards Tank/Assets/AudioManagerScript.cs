using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerScript : MonoBehaviour
{
    [SerializeField] AudioSource BgmAudio;
    public AudioClip japBGM;
    public AudioClip gerBGM;
    public AudioClip sovBGM;
    public AudioClip engBGM;
    public AudioClip usaBGM;

    [SerializeField] AudioSource SfxAudio; //Sfx:音效

    public AudioClip MediumTankMove; //MT中型坦克 HT重型坦克
    public AudioClip MediumTankAttack;
    public AudioClip HeavyTankMove;
    public AudioClip HeavyTankAttack;

    public AudioClip DecreaseActionCost; //减行动花费
    public AudioClip Trigger;  //齿轮触发

    public AudioClip sovLashen;
    public AudioClip sovSu; //转折点

    public AudioClip japTaiko2;  //Taiko：太鼓 2是打两下
    public AudioClip japSurpriseAttack; //偷袭

    public AudioClip gerMillitaryMusic; //军乐
    public AudioClip gerBlitzDoctrine; //闪击战法
    public AudioClip gerContestDoctrine; //竞争战法

    public AudioClip engNavalPower; //海军力量

    public void PlaySfx(AudioClip clip)  //需要时调用即可
    {
        SfxAudio.PlayOneShot(clip);
    }

    void Start() //只是测试
    {
        PlaySfx(gerMillitaryMusic);

        BgmAudio.clip = japBGM;
        BgmAudio.Play();
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerScript : MonoBehaviour
{
    [SerializeField] AudioSource BgmAudio;
    //public AudioClip japBGM;
    //public AudioClip gerBGM;
    //public AudioClip sovBGM;
    //public AudioClip engBGM;
    //public AudioClip usaBGM;
    public List<AudioClip> bgmList = new List<AudioClip>();

    [SerializeField] AudioSource SfxAudio; //Sfx:音效

    //public AudioClip MediumTankMove; //MT中型坦克 HT重型坦克
    //public AudioClip MediumTankAttack;
    //public AudioClip HeavyTankMove;
    //public AudioClip HeavyTankAttack;

    //public AudioClip DecreaseActionCost; //减行动花费
    //public AudioClip Trigger;  //齿轮触发

    //public AudioClip sovLashen;
    //public AudioClip sovSu; //转折点

    //public AudioClip japTaiko2;  //Taiko：太鼓 2是打两下
    //public AudioClip japSurpriseAttack; //偷袭

    //public AudioClip gerMillitaryMusic; //军乐
    //public AudioClip gerBlitzDoctrine; //闪击战法
    //public AudioClip gerContestDoctrine; //竞争战法

    //public AudioClip engNavalPower; //海军力量

    public List<AudioClip> sfxList = new List<AudioClip>();

    public void PlaySfx(int index)  //需要时调用即可
    {
        SfxAudio.PlayOneShot(sfxList[index]);
    }

    public void PlayBgm(int index)
    {
        BgmAudio.clip = bgmList[index];
        BgmAudio.Play();
    }

    void Start()
    {
        PlaySfx(16); // 回合切换
        PlayBgm(0); // jap bgm
    }

}

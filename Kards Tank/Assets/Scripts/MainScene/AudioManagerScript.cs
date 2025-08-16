using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Analytics;

public class AudioManagerScript : MonoBehaviour
{
    [SerializeField] AudioSource BgmAudio; //在Unity编辑器查看音效列表
    public List<AudioClip> bgmList = new List<AudioClip>();

    [SerializeField] AudioSource SfxAudio; //Sfx:音效

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
        int bgmIndex = Random.Range(0, bgmList.Count());
        PlaySfx(16); // 回合切换
        PlayBgm(bgmIndex); // jap bgm
    }

}

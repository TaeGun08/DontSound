using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("AudioSource")]
    [SerializeField] private AudioSource bgmAudioSource;
    [SerializeField] private AudioSource fxsAudioSource;

    [Header("AudioClips")]
    [SerializeField] private List<AudioClip> bgmClips;
    [SerializeField] private List<AudioClip> fxsClips;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (SettingManager.Instance.SaveCheck == true)
        {
            bgmAudioSource.volume = SettingManager.Instance.GetSlidersValue(0);
            fxsAudioSource.volume = SettingManager.Instance.GetSlidersValue(1);
            SettingManager.Instance.SaveCheck = false;
        }
    }

    /// <summary>
    /// 다른 스크립트를 통해 bgm을 변경시키기 위한 함수
    /// </summary>
    /// <param name="_bgmNumber"></param>
    public void SetBgmClips(int _bgmNumber)
    {
        if (bgmClips.Count >= _bgmNumber)
        {
            bgmAudioSource.clip = bgmClips[_bgmNumber];
        }

        bgmAudioSource.Play();
    }

    /// <summary>
    /// 설정 저장버튼을 눌렀을 때 저장된 값을 넣어주기 위한 함수
    /// </summary>
    /// <returns></returns>
    public float GetFxsVolume()
    {
        return SettingManager.Instance.GetSlidersValue(1);
    }

    /// <summary>
    /// 다른 스크립트에서 bgm을 멈추게 하기 위한 함수
    /// </summary>
    public void BgmPause()
    {
        bgmAudioSource.Pause();
    }
}

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
    /// �ٸ� ��ũ��Ʈ�� ���� bgm�� �����Ű�� ���� �Լ�
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
    /// ���� �����ư�� ������ �� ����� ���� �־��ֱ� ���� �Լ�
    /// </summary>
    /// <returns></returns>
    public float GetFxsVolume()
    {
        return SettingManager.Instance.GetSlidersValue(1);
    }

    /// <summary>
    /// �ٸ� ��ũ��Ʈ���� bgm�� ���߰� �ϱ� ���� �Լ�
    /// </summary>
    public void BgmPause()
    {
        bgmAudioSource.Pause();
    }
}

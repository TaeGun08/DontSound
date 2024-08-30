using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainTitleManager : MonoBehaviour
{
    private SettingManager settingManager;

    [Header("���� ��ư")]
    [SerializeField] private Button startButton;

    [Header("���� ��ư")]
    [SerializeField] private Button settingButton;

    [Header("���� ��ư")]
    [SerializeField] private Button exitButton;

    private void Awake()
    {
        buttons();
    }

    private void Start()
    {
        settingManager = SettingManager.Instance;

        SoundManager.Instance.SetBgmClips(0);
    }

    /// <summary>
    /// ��ư�� ��� ����
    /// </summary>
    private void buttons()
    {
        startButton.onClick.AddListener(() =>
        {
            FadeInOut.Instance.SetActive(false, () =>
            {
                SceneManager.LoadSceneAsync("School");

                FadeInOut.Instance.SetActive(true);
            });
        });

        settingButton.onClick.AddListener(() =>
        {
            settingManager.SettingObject().SetActive(true);
        });

        exitButton.onClick.AddListener(() =>
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        });
    }
}

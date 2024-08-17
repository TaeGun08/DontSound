using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainTitleManager : MonoBehaviour
{
    [Header("시작 버튼")]
    [SerializeField] private Button startButton;

    [Header("설정 버튼")]
    [SerializeField] private Button settingButton;

    [Header("종료 버튼")]
    [SerializeField] private Button exitButton;

    private void Awake()
    {
        buttons();
    }

    /// <summary>
    /// 버튼의 기능 모음
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainTitleManager : MonoBehaviour
{
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

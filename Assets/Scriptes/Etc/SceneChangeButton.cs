using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChangeButton : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private Button sceneButton;

    private void Awake()
    {
        sceneButton.onClick.AddListener(() => 
        {
            FadeInOut.Instance.SetActive(false, () =>
            {
                SceneManager.LoadSceneAsync(sceneName);

                FadeInOut.Instance.SetActive(true);
            });
        });
    }

    private void Start()
    {
        SoundManager.Instance.BgmPause();
    }
}

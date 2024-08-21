using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingManager : MonoBehaviour
{
    public static SettingManager Instance;

    [Header("설정 캔버스")]
    [SerializeField] private GameObject settingCanvas;
    private GameObject settingObject; // 설정 캔버스의 오브젝트

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        settingObject = Instantiate(settingCanvas, transform);
    }

    /// <summary>
    /// 설정 캔버스 오브젝트를 다른 스크립트에 보내주기 위한 함수
    /// </summary>
    /// <returns></returns>
    public GameObject SettingObject()
    {
        return settingObject;
    }
}

using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
    public static SettingManager Instance;

    [Header("설정 캔버스")]
    [SerializeField] private GameObject settingCanvas;
    private GameObject settingObject; // 설정 캔버스의 오브젝트
    private Button closeButton; // 설정창을 닫는 버튼
    private List<Slider> sliders = new List<Slider>(); // 배경음, 효과음, 민감도 설정을 위한 슬라이더
    private List<TMP_Text> valueText = new List<TMP_Text>(); // 설정 값을 표기해줄 텍스트
    private Toggle windowToggle; // 창모드를 위한 토글
    private Button settingSaveButton; // 설정 저장 버튼

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
        settingObject.SetActive(false);

        closeButton = settingObject.transform.GetChild(0).Find("X").GetComponent<Button>();

        for (int iNum = 0; iNum < 3; iNum++)
        {
            sliders.Add(settingObject.transform.GetChild(0).Find("SliderLayout").GetChild(iNum).GetComponent<Slider>());
            valueText.Add(settingObject.transform.GetChild(0).Find("ValueTextLayout").GetChild(iNum).GetComponent<TMP_Text>());
        }

        windowToggle = settingObject.transform.GetChild(0).Find("WindowMode").GetComponent<Toggle>();
        settingSaveButton = settingObject.transform.GetChild(0).Find("Save").GetComponent<Button>();
    }

    private void Start()
    {
        closedButton();
        saveSettingCheck();
        saveButton();
    }

    /// <summary>
    /// 설정창을 닫는 버튼
    /// </summary>
    private void closedButton()
    {
        closeButton.onClick.AddListener(() => 
        {
            settingObject.SetActive(false);
        });
    }

    /// <summary>
    /// 설정 저장이 되어있는지 확인을 하는 함수
    /// </summary>
    private void saveSettingCheck()
    {
        int count = sliders.Count;

        if (PlayerPrefs.GetString("saveSetting") == string.Empty)
        {
            for (int iNum = 0; iNum < count; iNum++)
            {
                sliders[iNum].value = 0.5f;
            }
        }
        else
        {
            string getSaveSetting = PlayerPrefs.GetString("saveSetting");

            for (int iNum = 0; iNum < count; iNum++)
            {
                sliders[iNum].value = JsonConvert.DeserializeObject<float>(getSaveSetting);
            }
        }
    }

    /// <summary>
    /// 설정 저장 버튼
    /// </summary>
    private void saveButton()
    {
        settingSaveButton.onClick.AddListener(() => 
        {
            int count = sliders.Count;

            string setSaveSetting = JsonConvert.SerializeObject(sliders);
            PlayerPrefs.SetString("saveSetting", setSaveSetting);
        });
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

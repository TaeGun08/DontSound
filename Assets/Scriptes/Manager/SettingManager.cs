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

    public class SaveSetting
    {
        public List<float> sliders = new List<float>(); // 저장된 배경음, 효과음, 민감도 설정을 위한 슬라이더
        public bool windowToggle; // 저장된 창모드를 위한 토글
    }

    private SaveSetting saveSetting = new SaveSetting();

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
            DontDestroyOnLoad(gameObject);
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

    private void Update()
    {
        textChange();
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
                saveSetting.sliders.Add(0.5f);
                saveSetting.windowToggle = false;
                sliders[iNum].value = 0.5f;
                windowToggle.isOn = false;
            }
        }
        else
        {
            string getSaveSetting = PlayerPrefs.GetString("saveSetting");
            saveSetting = JsonConvert.DeserializeObject<SaveSetting>(getSaveSetting);

            for (int iNum = 0; iNum < count; iNum++)
            {
                sliders[iNum].value = saveSetting.sliders[iNum];
            }

            windowToggle.isOn = saveSetting.windowToggle;
        }

        Screen.SetResolution(1920, 1080, windowToggle.isOn);
    }

    /// <summary>
    /// 설정 저장 버튼
    /// </summary>
    private void saveButton()
    {
        settingSaveButton.onClick.AddListener(() => 
        {
            int count = sliders.Count;

            for (int iNum = 0; iNum < count; iNum++)
            {
                saveSetting.sliders[iNum] = sliders[iNum].value;
            }

            saveSetting.windowToggle = windowToggle.isOn;

            string setSaveSetting = JsonConvert.SerializeObject(saveSetting);
            PlayerPrefs.SetString("saveSetting", setSaveSetting);

            Screen.SetResolution(1920, 1080, windowToggle.isOn);
        });
    }

    /// <summary>
    /// 수치를 눈으로 보여주기 위한 텍스트를 실시간으로 수치를 변경하기 위한 함수
    /// </summary>
    private void textChange()
    {
        valueText[0].text = $"{(sliders[0].value * 100).ToString("F0")}";
        valueText[1].text = $"{(sliders[1].value * 100).ToString("F0")}";
        valueText[2].text = $"{sliders[2].value.ToString("F1")}";
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

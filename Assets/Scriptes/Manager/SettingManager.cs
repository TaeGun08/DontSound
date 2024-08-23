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

    [Header("���� ĵ����")]
    [SerializeField] private GameObject settingCanvas;
    private GameObject settingObject; // ���� ĵ������ ������Ʈ
    private Button closeButton; // ����â�� �ݴ� ��ư
    private List<Slider> sliders = new List<Slider>(); // �����, ȿ����, �ΰ��� ������ ���� �����̴�
    private List<TMP_Text> valueText = new List<TMP_Text>(); // ���� ���� ǥ������ �ؽ�Ʈ
    private Toggle windowToggle; // â��带 ���� ���
    private Button settingSaveButton; // ���� ���� ��ư

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
    /// ����â�� �ݴ� ��ư
    /// </summary>
    private void closedButton()
    {
        closeButton.onClick.AddListener(() => 
        {
            settingObject.SetActive(false);
        });
    }

    /// <summary>
    /// ���� ������ �Ǿ��ִ��� Ȯ���� �ϴ� �Լ�
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
    /// ���� ���� ��ư
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
    /// ���� ĵ���� ������Ʈ�� �ٸ� ��ũ��Ʈ�� �����ֱ� ���� �Լ�
    /// </summary>
    /// <returns></returns>
    public GameObject SettingObject()
    {
        return settingObject;
    }
}

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

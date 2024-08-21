using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingManager : MonoBehaviour
{
    public static SettingManager Instance;

    [Header("���� ĵ����")]
    [SerializeField] private GameObject settingCanvas;
    private GameObject settingObject; // ���� ĵ������ ������Ʈ

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
    /// ���� ĵ���� ������Ʈ�� �ٸ� ��ũ��Ʈ�� �����ֱ� ���� �Լ�
    /// </summary>
    /// <returns></returns>
    public GameObject SettingObject()
    {
        return settingObject;
    }
}

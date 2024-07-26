using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    private GameObject spotLight; //������ �Һ� ������Ʈ

    private void Awake()
    {
        spotLight = transform.GetChild(0).gameObject;

        spotLight.SetActive(false);
    }

    /// <summary>
    /// �������� �Ѱ� ���� �Լ�
    /// </summary>
    public void LightOnOff(bool _onOff)
    {
        spotLight.SetActive(_onOff);
    }
}

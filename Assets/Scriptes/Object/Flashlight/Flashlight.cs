using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    private GameObject spotLight; //손전등 불빛 오브젝트

    private void Awake()
    {
        spotLight = transform.GetChild(0).gameObject;

        spotLight.SetActive(false);
    }

    /// <summary>
    /// 손전등을 켜고 끄는 함수
    /// </summary>
    public void LightOnOff(bool _onOff)
    {
        spotLight.SetActive(_onOff);
    }
}

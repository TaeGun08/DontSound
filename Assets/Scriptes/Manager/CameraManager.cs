using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    [Header("ī�޶�")]
    [SerializeField] private List<Camera> cameras;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    /// <summary>
    /// ī�޶� �Ŵ����� �ִ� ī�޶� �������� ���� �Լ�
    /// </summary>
    /// <param name="_cameraNumber">ī�޶� ��ȣ</param>
    /// <returns>0�� PlayerCamera</returns>
    public Camera GetCamera(int _cameraNumber)
    {
        return cameras[_cameraNumber];
    }
}

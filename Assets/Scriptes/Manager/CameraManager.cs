using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    [Header("ī�޶�")]
    [SerializeField] private List<Camera> cameras;

    [Header("���߾� ī�޶�")]
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

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

    /// <summary>
    /// ���߾� ī�޶� �ٸ� ��ũ��Ʈ�� ���� ���� �� �� �ְ� ������ִ� �Լ�
    /// </summary> 0�� �ѱ� 1�� ����
    public void SetVirtualCameraOnOff(int _onOff)
    {
        virtualCamera.gameObject.SetActive(_onOff == 0 ? true : false);
    }
}

using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    [Header("카메라")]
    [SerializeField] private List<Camera> cameras;

    [Header("버추얼 카메라")]
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    private CinemachinePOV cinemachinePov;

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

        cinemachinePov = virtualCamera.GetCinemachineComponent<CinemachinePOV>();
    }

    private void Start()
    {
        cinemachinePov.m_HorizontalAxis.m_MaxSpeed = SettingManager.Instance.GetSlidersValue(2) * 300;
        cinemachinePov.m_VerticalAxis.m_MaxSpeed = SettingManager.Instance.GetSlidersValue(2) * 300;
    }

    /// <summary>
    /// 카메라 매니저에 있는 카메라를 가져오기 위한 함수
    /// </summary>
    /// <param name="_cameraNumber">카메라 번호</param>
    /// <returns>0번 PlayerCamera</returns>
    public Camera GetCamera(int _cameraNumber)
    {
        return cameras[_cameraNumber];
    }

    /// <summary>
    /// 버추얼 카메라를 다른 스크립트를 통해 끄고 켤 수 있게 만들어주는 함수
    /// </summary> 0은 켜기 1은 끄기
    public void SetVirtualCameraOnOff(int _onOff)
    {
        virtualCamera.gameObject.SetActive(_onOff == 0 ? true : false);
    }

    /// <summary>
    /// SettingManger에서 마우스 민감도를 넣어주기 위한 함수
    /// </summary>
    /// <param name="_mouseSensitivity"></param>
    /// <param name="_mouseSensitivity"></param>
    public void SetMouseSensitivity(float _mouseSensitivity)
    {
        cinemachinePov.m_HorizontalAxis.m_MaxSpeed = _mouseSensitivity * 300;
        cinemachinePov.m_VerticalAxis.m_MaxSpeed = _mouseSensitivity * 300;
    }
}

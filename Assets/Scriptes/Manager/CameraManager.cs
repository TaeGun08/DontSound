using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    [Header("카메라")]
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
    /// 카메라 매니저에 있는 카메라를 가져오기 위한 함수
    /// </summary>
    /// <param name="_cameraNumber">카메라 번호</param>
    /// <returns>0번 PlayerCamera</returns>
    public Camera GetCamera(int _cameraNumber)
    {
        return cameras[_cameraNumber];
    }
}

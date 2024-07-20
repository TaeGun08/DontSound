using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("컨트롤러")]
    [SerializeField] private List<GameObject> controllerObjects;

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
    /// 게임 매니저에서 관리하는 컨트롤러들
    /// </summary>
    /// <param name="_controllerNumber">사용할 컨트롤러 번호</param>
    /// <returns>0번 MoveController, </returns>
    public GameObject GetControllerObject(int _controllerNumber)
    {
        return controllerObjects[_controllerNumber];
    }
}

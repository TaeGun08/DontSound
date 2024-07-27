using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("컨트롤러")]
    [SerializeField] private List<GameObject> controllerObjects;

    [Header("플레이어")]
    [SerializeField] private GameObject player;

    [Header("몬스터가 돌아다닐 장소")]
    [SerializeField] private MonsterPlaceToGo monsterPlaceToGo;

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

    /// <summary>
    /// 게임 매니저에서 관리하는 플레이어
    /// </summary>
    /// <returns></returns>
    public GameObject GetPlayer()
    {
        return player;
    }

    /// <summary>
    /// 게임 매니저에서 관리하는 몬스터가 돌아다닐 장소
    /// </summary>
    /// <returns></returns>
    public MonsterPlaceToGo GetMonsterPlaceToGo()
    {
        return monsterPlaceToGo;
    }
}

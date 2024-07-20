using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("��Ʈ�ѷ�")]
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
    /// ���� �Ŵ������� �����ϴ� ��Ʈ�ѷ���
    /// </summary>
    /// <param name="_controllerNumber">����� ��Ʈ�ѷ� ��ȣ</param>
    /// <returns>0�� MoveController, </returns>
    public GameObject GetControllerObject(int _controllerNumber)
    {
        return controllerObjects[_controllerNumber];
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPlaceToGo : MonoBehaviour
{
    [Header("���Ͱ� ���ƴٳ���� ���")]
    [SerializeField] private List<Transform> toGoTrs;

    /// <summary>
    /// ���Ͱ� ������ ���
    /// </summary>
    /// <param name="_toGo"></param>
    /// <returns></returns>
    public Transform GetToGoTrs(int _toGo)
    {
        return toGoTrs[_toGo];
    }
}

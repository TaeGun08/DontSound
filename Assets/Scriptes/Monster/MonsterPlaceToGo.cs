using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPlaceToGo : MonoBehaviour
{
    [Header("몬스터가 돌아다녀야할 장소")]
    [SerializeField] private List<Transform> toGoTrs;

    /// <summary>
    /// 몬스터가 가야할 장소
    /// </summary>
    /// <param name="_toGo"></param>
    /// <returns></returns>
    public Transform GetToGoTrs(int _toGo)
    {
        return toGoTrs[_toGo];
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindPlayerColl : MonoBehaviour
{
    private GameObject player; //플레이어 오브젝트

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            player = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            player = null;
        }
    }

    /// <summary>
    /// 콜라이더에 플레이어가 들어왔는지 확인하기 위한 함수
    /// </summary>
    /// <returns></returns>
    public GameObject GetPlayer()
    {
        return player;
    }
}

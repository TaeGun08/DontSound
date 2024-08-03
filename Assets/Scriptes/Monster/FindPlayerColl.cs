using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindPlayerColl : MonoBehaviour
{
    private BoxCollider boxCollider; //플레이어를 체크할 박스콜라이더
    private Vector3 curBoxCollider; //처음 박스 콜라이더 크기를 담을 변수

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

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
        curBoxCollider = boxCollider.size;
    }

    /// <summary>
    /// 콜라이더에 플레이어가 들어왔는지 확인하기 위한 함수
    /// </summary>
    /// <returns></returns>
    public GameObject GetPlayer()
    {
        return player;
    }

    /// <summary>
    /// 상황에 따라 콜라이더의 크기를 바꿔주기 위한 함수
    /// </summary>
    public void SetBoxCollSize(Vector3 _sizeXYZ)
    {
        boxCollider.size = _sizeXYZ;
    }

    /// <summary>
    /// 다시 원래 크기의 콜라이더로 돌려주기 위한 함수
    /// </summary>
    public void ResetBoxCollSize()
    {
        boxCollider.size = curBoxCollider;
    }
}

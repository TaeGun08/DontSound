using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindPlayerColl : MonoBehaviour
{
    private GameObject player; //�÷��̾� ������Ʈ

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
    /// �ݶ��̴��� �÷��̾ ���Դ��� Ȯ���ϱ� ���� �Լ�
    /// </summary>
    /// <returns></returns>
    public GameObject GetPlayer()
    {
        return player;
    }
}

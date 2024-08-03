using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindPlayerColl : MonoBehaviour
{
    private BoxCollider boxCollider; //�÷��̾ üũ�� �ڽ��ݶ��̴�
    private Vector3 curBoxCollider; //ó�� �ڽ� �ݶ��̴� ũ�⸦ ���� ����

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

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
        curBoxCollider = boxCollider.size;
    }

    /// <summary>
    /// �ݶ��̴��� �÷��̾ ���Դ��� Ȯ���ϱ� ���� �Լ�
    /// </summary>
    /// <returns></returns>
    public GameObject GetPlayer()
    {
        return player;
    }

    /// <summary>
    /// ��Ȳ�� ���� �ݶ��̴��� ũ�⸦ �ٲ��ֱ� ���� �Լ�
    /// </summary>
    public void SetBoxCollSize(Vector3 _sizeXYZ)
    {
        boxCollider.size = _sizeXYZ;
    }

    /// <summary>
    /// �ٽ� ���� ũ���� �ݶ��̴��� �����ֱ� ���� �Լ�
    /// </summary>
    public void ResetBoxCollSize()
    {
        boxCollider.size = curBoxCollider;
    }
}

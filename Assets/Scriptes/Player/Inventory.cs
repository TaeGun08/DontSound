using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [Header("�κ��丮")]
    [SerializeField, Tooltip("�κ��丮 ����")] private GameObject[] items;
    private int keyCount; //���踦 � ������ �ִ��� üũ�ϱ� ���� ����

    private void Awake()
    {
        items = new GameObject[4];
    }

    /// <summary>
    /// ȹ���� �������� �κ��丮�� �־��ֱ� ���� �Լ�
    /// </summary>
    /// <param name="_item"></param>
    public void SetItem(GameObject _item)
    {
        int length = items.Length;

        for (int iNum = 0; iNum < length; iNum++)
        {
            if (items[iNum] == null)
            {
                items[iNum] = _item;
                break;
            }
        }
    }

    /// <summary>
    /// Ű�� �Ծ��� �� ī��Ʈ�� �÷��ֱ� ���� �Լ�
    /// </summary>
    public void KeyCountUp()
    {
        keyCount++;
    }

    /// <summary>
    /// Ű ī��Ʈ�� �������� ���� �Լ�
    /// </summary>
    /// <returns></returns>
    public int GetKeyCount()
    {
        return keyCount;
    }
}

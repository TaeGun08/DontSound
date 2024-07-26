using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [Header("인벤토리")]
    [SerializeField, Tooltip("인벤토리 공간")] private GameObject[] items;

    private void Awake()
    {
        items = new GameObject[4];
    }

    /// <summary>
    /// 획득한 아이템을 인벤토리에 넣어주기 위한 함수
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
}

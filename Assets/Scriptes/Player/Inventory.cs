using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [Header("인벤토리")]
    [SerializeField, Tooltip("인벤토리 공간")] private GameObject[] items;
    private int keyCount; //열쇠를 몇개 가지고 있는지 체크하기 위한 변수

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

    /// <summary>
    /// 키를 먹었을 때 카운트를 올려주기 위한 함수
    /// </summary>
    public void KeyCountUp()
    {
        keyCount++;
    }

    /// <summary>
    /// 키 카운트를 가져오기 위한 함수
    /// </summary>
    /// <returns></returns>
    public int GetKeyCount()
    {
        return keyCount;
    }
}

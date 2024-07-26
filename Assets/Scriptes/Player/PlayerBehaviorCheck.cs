using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviorCheck : MonoBehaviour
{
    [Header("플레이어의 행동")]
    [SerializeField, Tooltip("행동을 하고 있는지")] private bool isBehavior;
    public bool IsBehavior
    {
        get
        {
            return isBehavior;
        }
        set
        {
            isBehavior = value;
        }
    }

    private float walkRunCheck; //걷는 중인지 뛰는 중인지 체크
    public float WalkRunCheck
    {
        get
        {
            return walkRunCheck;
        }
        set
        {
            walkRunCheck = value;
        }
    }

    private float isHorizontal; //수평으로 움직이고 있는지 체크
    public float IsHorizontal
    {
        get
        {
            return isHorizontal;
        }
        set
        {
            isHorizontal = value;
        }
    }

    private float isVertical; //수직으로 움직이고 있는지 체크
    public float IsVertical
    {
        get
        {
            return isVertical;
        }
        set
        {
            isVertical = value;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlyaerAnimation : MonoBehaviour
{
    private PlayerBehaviorCheck playerBehaviorCheck; //플레이어의 행동을 체크하는 스크립트

    private Animator anim; //플레이어의 애니메이션

    private void Awake()
    {
        playerBehaviorCheck = GetComponent<PlayerBehaviorCheck>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        playerAnim();
    }

    /// <summary>
    /// 플레이어 애니메이션을 담당하는 함수
    /// </summary>
    private void playerAnim()
    {
        anim.SetBool("isBehavior", playerBehaviorCheck.IsBehavior);
        anim.SetFloat("WalkRunCheck", playerBehaviorCheck.WalkRunCheck);
        anim.SetFloat("isHorizontal", playerBehaviorCheck.IsHorizontal);
        anim.SetFloat("isVertical", playerBehaviorCheck.IsVertical);
    }
}

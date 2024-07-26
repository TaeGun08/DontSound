using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlyaerAnimation : MonoBehaviour
{
    private PlayerBehaviorCheck playerBehaviorCheck; //�÷��̾��� �ൿ�� üũ�ϴ� ��ũ��Ʈ

    private Animator anim; //�÷��̾��� �ִϸ��̼�

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
    /// �÷��̾� �ִϸ��̼��� ����ϴ� �Լ�
    /// </summary>
    private void playerAnim()
    {
        anim.SetBool("isBehavior", playerBehaviorCheck.IsBehavior);
        anim.SetFloat("WalkRunCheck", playerBehaviorCheck.WalkRunCheck);
        anim.SetFloat("isHorizontal", playerBehaviorCheck.IsHorizontal);
        anim.SetFloat("isVertical", playerBehaviorCheck.IsVertical);
    }
}

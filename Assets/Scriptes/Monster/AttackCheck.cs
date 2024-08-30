using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AttackCheck : MonoBehaviour
{
    private bool attack = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            FadeInOut.Instance.SetActive(false, () =>
            {
                SceneManager.LoadSceneAsync("Dead");

                Cursor.lockState = CursorLockMode.None;

                FadeInOut.Instance.SetActive(true);
            });
        }
    }

    /// <summary>
    /// 공격을 할 수 있는 상태인지 체크하는 변수를 보내는 함수
    /// </summary>
    /// <returns></returns>
    public bool GetAttack()
    {
        return attack;
    }
}

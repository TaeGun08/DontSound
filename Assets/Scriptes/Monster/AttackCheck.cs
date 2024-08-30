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
    /// ������ �� �� �ִ� �������� üũ�ϴ� ������ ������ �Լ�
    /// </summary>
    /// <returns></returns>
    public bool GetAttack()
    {
        return attack;
    }
}

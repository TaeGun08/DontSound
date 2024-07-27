using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    private PlayerBehaviorCheck playerBehaviorCheck; //�÷��̾��� �ൿ�� üũ�ϴ� ��ũ��Ʈ

    private CharacterController characterController; //�÷��̾��� ĳ���� ��Ʈ�ѷ�

    [Header("�÷��̾� ������ ����")]
    [SerializeField, Tooltip("�÷��̾��� �ȴ� �ӵ�")] private float walkSpeed;
    [SerializeField, Tooltip("�÷��̾��� �ٴ� �ӵ�")] private float runSpeed;
    private bool runCheck = false; //�÷��̾ �޸��� �ִ��� Ȯ���ϱ� ���� �Լ�, �޸��� �ִٸ� ���׹̳ʰ� �������� �ʵ��� ����
    [SerializeField, Tooltip("�÷��̾��� �߷�")] private float gravity;
    [Space]
    [SerializeField, Tooltip("�÷��̾��� ���׹̳�")] private float stamina;

    private void Awake()
    {
        playerBehaviorCheck = GetComponent<PlayerBehaviorCheck>();
        characterController = GetComponent<CharacterController>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        playerMove();
        playerMouseRotate();
        playerGravity();
        playerStamina();
    }

    /// <summary>
    /// �÷��̾ �Ȱ� ���ִ� �Լ�
    /// </summary>
    private void playerMove()
    {
        if (!Input.GetKey(KeyCode.W) &&
            !Input.GetKey(KeyCode.S) &&
            !Input.GetKey(KeyCode.D) &&
            !Input.GetKey(KeyCode.A) &&
            playerBehaviorCheck.IsBehavior == true)
        {
            playerBehaviorCheck.IsBehavior = false;
        }

        if (Input.GetKey(KeyCode.W))
        {
            playerWalkOrRunCheck(1);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            playerWalkOrRunCheck(2);
        }
        else if ((!Input.GetKey(KeyCode.W) ||
              !Input.GetKey(KeyCode.S)) && 
              (playerBehaviorCheck.IsVertical == 1 ||
              playerBehaviorCheck.IsVertical == -1))
        {
            playerBehaviorCheck.IsVertical = 0;
        }

        if (Input.GetKey(KeyCode.D))
        {
            playerWalkOrRunCheck(3);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            playerWalkOrRunCheck(4);
        }
        else if ((!Input.GetKey(KeyCode.D) ||
            !Input.GetKey(KeyCode.A)) &&
            (playerBehaviorCheck.IsHorizontal == 1 ||
              playerBehaviorCheck.IsHorizontal == -1))
        {
            playerBehaviorCheck.IsHorizontal = 0;
        }
    }

    /// <summary>
    /// �÷��̾ �Ȱ� �ִ��� �ٰ� �ִ����� ���� �ӵ��� �����ϱ� ���� �Լ�
    /// </summary>
    private void playerWalkOrRunCheck(int _directionNumber)
    {
        if (!Input.GetKey(KeyCode.LeftShift) || (Input.GetKey(KeyCode.LeftShift) && stamina <= 0 ))
        {
            playerBehaviorCheck.IsBehavior = true;
            playerBehaviorCheck.WalkRunCheck = 0;

            if (runCheck == true)
            {
                runCheck = false;
            }

            if (_directionNumber == 1)
            {
                characterController.Move(transform.forward.normalized * walkSpeed * Time.deltaTime);
                playerBehaviorCheck.IsVertical = 1;
            }
            else if (_directionNumber == 2)
            {
                characterController.Move(-transform.forward.normalized * walkSpeed * Time.deltaTime);
                playerBehaviorCheck.IsVertical = -1;
            }
            else if (_directionNumber == 3)
            {
                characterController.Move(transform.right.normalized * walkSpeed * Time.deltaTime);
                playerBehaviorCheck.IsHorizontal = 1;
            }
            else if (_directionNumber == 4)
            {
                characterController.Move(-transform.right.normalized * walkSpeed * Time.deltaTime);
                playerBehaviorCheck.IsHorizontal = -1;
            }
        }
        else if (Input.GetKey(KeyCode.LeftShift) && stamina > 0)
        {
            playerBehaviorCheck.IsBehavior = true;
            playerBehaviorCheck.WalkRunCheck = 1;

            if (runCheck == false)
            {
                runCheck = true;
            }

            stamina -= Time.deltaTime * 15;

            if (_directionNumber == 1)
            {
                characterController.Move(transform.forward.normalized * runSpeed * Time.deltaTime);
                playerBehaviorCheck.IsVertical = 1;
            }
            else if (_directionNumber == 2)
            {
                characterController.Move(-transform.forward.normalized * runSpeed * Time.deltaTime);
                playerBehaviorCheck.IsVertical = -1;
            }
            else if (_directionNumber == 3)
            {
                characterController.Move(transform.right.normalized * runSpeed * Time.deltaTime);
                playerBehaviorCheck.IsHorizontal = 1;
            }
            else if (_directionNumber == 4)
            {
                characterController.Move(-transform.right.normalized * runSpeed * Time.deltaTime);
                playerBehaviorCheck.IsHorizontal = -1;
            }
        }
    }

    /// <summary>
    /// �÷��̾��� �߷��� ����ϴ� �Լ�
    /// </summary>
    private void playerGravity()
    {
        if (characterController.isGrounded == false)
        {
            characterController.Move(new Vector3(0f, -gravity, 0f) * Time.deltaTime);
        }
        else
        {
            characterController.Move(new Vector3(0f, 0f, 0f));
        }
    }

    /// <summary>
    /// �÷��̾ �޸��ų� ������ ���� �� ���׹̳ʸ� ���� ��Ű�� ���� �Լ� �װ� �ƴ϶�� ����
    /// </summary>
    private void playerStamina()
    {
        if (!Input.GetKey(KeyCode.LeftShift) && stamina < 100)
        {
            stamina += Time.deltaTime * 10;
        }
    }

    /// <summary>
    /// ���콺 ȸ���� ���� �÷��̾ ȸ����Ű�� ���� �Լ�
    /// </summary>
    private void playerMouseRotate()
    {
        transform.rotation = Quaternion.Euler(0f, CameraManager.Instance.GetCamera(0).transform.eulerAngles.y, 0f);
    }
}

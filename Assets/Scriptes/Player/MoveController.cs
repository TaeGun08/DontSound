using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    private CharacterController characterController; //플레이어의 캐릭터 컨트롤러

    [Header("플레이어 움직임 설정")]
    [SerializeField, Tooltip("플레이어의 걷는 속도")] private float walkSpeed;
    [SerializeField, Tooltip("플레이어의 뛰는 속도")] private float runSpeed;
    private bool runCheck = false; //플레이어가 달리고 있는지 확인하기 위한 함수, 달리고 있다면 스테미너가 충전되지 않도록 제어
    [SerializeField, Tooltip("플레이어의 점프 힘")] private float jumpPower;
    [SerializeField, Tooltip("플레이어의 중력")] private float gravity;
    [Space]
    [SerializeField, Tooltip("플레이어의 스테미너")] private float stamina;

    private void Awake()
    {
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
        playerJump();
        playerGravity();
        playerStamina();
    }

    /// <summary>
    /// 플레이어가 걷게 해주는 함수
    /// </summary>
    private void playerMove()
    {
        if (Input.GetKey(KeyCode.W))
        {
            playerWalkOrRunCheck(1);
        }

        if (Input.GetKey(KeyCode.S))
        {
            playerWalkOrRunCheck(2);
        }

        if (Input.GetKey(KeyCode.D))
        {
            playerWalkOrRunCheck(3);
        }

        if (Input.GetKey(KeyCode.A))
        {
            playerWalkOrRunCheck(4);
        }
    }

    /// <summary>
    /// 플레이어가 걷고 있는지 뛰고 있는지에 따라 속도를 변경하기 위한 함수
    /// </summary>
    private void playerWalkOrRunCheck(int _directionNumber)
    {
        if (!Input.GetKey(KeyCode.LeftShift) || (Input.GetKey(KeyCode.LeftShift) && stamina <= 0 ))
        {
            if (runCheck == true)
            {
                runCheck = false;
            }

            if (_directionNumber == 1)
            {
                characterController.Move(transform.forward.normalized * walkSpeed * Time.deltaTime);
            }
            else if (_directionNumber == 2)
            {
                characterController.Move(-transform.forward.normalized * walkSpeed * Time.deltaTime);
            }
            else if (_directionNumber == 3)
            {
                characterController.Move(transform.right.normalized * walkSpeed * Time.deltaTime);
            }
            else if (_directionNumber == 4)
            {
                characterController.Move(-transform.right.normalized * walkSpeed * Time.deltaTime);
            }
        }
        else if (Input.GetKey(KeyCode.LeftShift) && stamina > 0)
        {
            if (runCheck == false)
            {
                runCheck = true;
            }

            stamina -= Time.deltaTime * 30;

            if (_directionNumber == 1)
            {
                characterController.Move(transform.forward.normalized * runSpeed * Time.deltaTime);
            }
            else if (_directionNumber == 2)
            {
                characterController.Move(-transform.forward.normalized * runSpeed * Time.deltaTime);
            }
            else if (_directionNumber == 3)
            {
                characterController.Move(transform.right.normalized * runSpeed * Time.deltaTime);
            }
            else if (_directionNumber == 4)
            {
                characterController.Move(-transform.right.normalized * runSpeed * Time.deltaTime);
            }
        }
    }

    /// <summary>
    /// 플레이어의 중력을 담당하는 함수
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
    /// 플레이어가 점프를 할 수 있게 해주는 함수
    /// </summary>
    private void playerJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && characterController.isGrounded == true)
        {
            characterController.Move(new Vector3(0f, jumpPower, 0f) * Time.deltaTime);
        }
    }

    /// <summary>
    /// 플레이어가 달리거나 점프를 했을 때 스테미너를 감소 시키기 위한 함수 그게 아니라면 충전
    /// </summary>
    private void playerStamina()
    {
        if (!Input.GetKey(KeyCode.LeftShift) && stamina < 100)
        {
            stamina += Time.deltaTime * 10;
        }
    }

    /// <summary>
    /// 마우스 회전에 따라 플레이어를 회전시키기 위한 함수
    /// </summary>
    private void playerMouseRotate()
    {
        transform.rotation = Quaternion.Euler(0f, CameraManager.Instance.GetCamera(0).transform.eulerAngles.y, 0f);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveController : MonoBehaviour
{
    private CanvasManager canvasManager; //캔버스매니저

    private PlayerBehaviorCheck playerBehaviorCheck; //플레이어의 행동을 체크하는 스크립트

    private CharacterController characterController; //플레이어의 캐릭터 컨트롤러

    [Header("플레이어 움직임 설정")]
    [SerializeField, Tooltip("플레이어의 걷는 속도")] private float walkSpeed;
    [SerializeField, Tooltip("플레이어의 뛰는 속도")] private float runSpeed;
    [SerializeField, Tooltip("플레이어의 앉아서 걷는 속도")] private float sitSpeed;
    private bool runCheck = false; //플레이어가 달리고 있는지 확인하기 위한 함수, 달리고 있다면 스테미너가 충전되지 않도록 제어
    [SerializeField, Tooltip("플레이어의 중력")] private float gravity;
    [Space]
    [SerializeField, Tooltip("플레이어의 스테미너")] private float stamina;
    [Space]
    [SerializeField, Tooltip("플레이어의 머리 위치")] private Transform headTrs;

    private AudioSource playerStepAudio; //플레이어 발걸음 사운드를 담을 오디오
    private AudioSource playerBreatAudio; //플레이어 숨소리를 담을 오디오
    private float audioTimer; //오디오를 시간이 지나면 실행시킬 수 있게 만들 변수
    private bool audioPlay; //오디오를 실행시킬 변수
    private float audioDelay; //몇 초 간격으로 실행시킬건지 정할 수 있게 만들 변수
    private float curAudioSound; //현재 사운드
    private bool staminaCheck = false; //스테미너가 일정 수치 이하로 떨어졌는지 체크하기 위한 변수

    private void Awake()
    {
        playerBehaviorCheck = GetComponent<PlayerBehaviorCheck>();
        characterController = GetComponent<CharacterController>();

        playerStepAudio = transform.Find("PlayerStepSound").GetComponent<AudioSource>();
        playerBreatAudio = transform.Find("PlayerBreatSound").GetComponent<AudioSource>();
    }

    private void Start()
    {
        canvasManager = CanvasManager.Instance;

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        playerTimers();
        playerMove();
        playerMouseRotate();
        playerGravity();
        playerStamina();
        playerSit();

        if (SoundManager.Instance != null && SoundManager.Instance.GetFxsVolume() != curAudioSound)
        {
            playerStepAudio.volume = SoundManager.Instance.GetFxsVolume();
            playerBreatAudio.volume = SoundManager.Instance.GetFxsVolume();
        }
    }

    /// <summary>
    /// 플레이어와 관련된 타이머를 모아두는 곳
    /// </summary>
    private void playerTimers()
    {
        if (audioPlay == true)
        {
            audioTimer += Time.deltaTime;

            if (audioTimer >= audioDelay)
            {
                playerStepAudio.Play();
                audioTimer = 0;
                audioPlay = false;
            }
        }
    }

    /// <summary>
    /// 플레이어가 걷게 해주는 함수
    /// </summary>
    private void playerMove()
    {
        Vector3 moveDir = Vector3.zero;
        if (Input.GetKey(KeyCode.W) == true)
        {
            moveDir.z = 1;
        }
        else if (Input.GetKey(KeyCode.S) == true)
        {
            moveDir.z = -1;
        }

        if (Input.GetKey(KeyCode.A) == true)
        {
            moveDir.x = -1;
        }
        else if (Input.GetKey(KeyCode.D) == true)
        {
            moveDir.x = 1;
        }

        playerBehaviorCheck.IsBehavior = moveDir.x == 0 && moveDir.z == 0 ? false : true;

        playerBehaviorCheck.IsHorizontal = moveDir.x;
        playerBehaviorCheck.IsVertical = moveDir.z;

        runCheck = Input.GetKey(KeyCode.LeftShift) && playerBehaviorCheck.IsBehavior == true && stamina > 0;
        if (runCheck == true)
        {
            stamina -= Time.deltaTime * 15;
        }

        playerBehaviorCheck.WalkRunCheck = runCheck ? 1 : 0;

        if (playerBehaviorCheck.SitCheck == 0)
        {
            characterController.Move(transform.rotation * moveDir.normalized * (runCheck == false ? walkSpeed : runSpeed) * Time.deltaTime);
        }
        else
        {
            characterController.Move(transform.rotation * moveDir.normalized * sitSpeed * Time.deltaTime);
        }

        if (playerBehaviorCheck.IsBehavior == true && playerBehaviorCheck.WalkRunCheck == 0 && playerBehaviorCheck.SitCheck == 0)
        {
            audioPlay = true;
            audioDelay = 1f;
        }
        else if (playerBehaviorCheck.IsBehavior == true && playerBehaviorCheck.WalkRunCheck == 1 && playerBehaviorCheck.SitCheck == 0)
        {
            audioPlay = true;
            audioDelay = 0.5f;
        }
        else if (playerBehaviorCheck.IsBehavior == false || playerBehaviorCheck.SitCheck == 1)
        {
            playerStepAudio.Pause();
        }

        //if (!Input.GetKey(KeyCode.W) &&
        //    !Input.GetKey(KeyCode.S) &&
        //    !Input.GetKey(KeyCode.D) &&
        //    !Input.GetKey(KeyCode.A) &&
        //    playerBehaviorCheck.IsBehavior == true)
        //{
        //    playerBehaviorCheck.IsBehavior = false;
        //}

        //if (Input.GetKey(KeyCode.W))
        //{
        //    playerWalkOrRunCheck(1);
        //}
        //else if (Input.GetKey(KeyCode.S))
        //{
        //    playerWalkOrRunCheck(2);
        //}
        //else if ((!Input.GetKey(KeyCode.W) ||
        //      !Input.GetKey(KeyCode.S)) && 
        //      (playerBehaviorCheck.IsVertical == 1 ||
        //      playerBehaviorCheck.IsVertical == -1))
        //{
        //    playerBehaviorCheck.IsVertical = 0;
        //}

        //if (Input.GetKey(KeyCode.D))
        //{
        //    playerWalkOrRunCheck(3);
        //}
        //else if (Input.GetKey(KeyCode.A))
        //{
        //    playerWalkOrRunCheck(4);
        //}
        //else if ((!Input.GetKey(KeyCode.D) ||
        //    !Input.GetKey(KeyCode.A)) &&
        //    (playerBehaviorCheck.IsHorizontal == 1 ||
        //      playerBehaviorCheck.IsHorizontal == -1))
        //{
        //    playerBehaviorCheck.IsHorizontal = 0;
        //}
    }

    /// <summary>
    /// 플레이어가 걷고 있는지 뛰고 있는지에 따라 속도를 변경하기 위한 함수
    /// </summary>
    private void playerWalkOrRunCheck(int _directionNumber)
    {
        if (!Input.GetKey(KeyCode.LeftShift) || (Input.GetKey(KeyCode.LeftShift) && stamina <= 0))
        {
            playerBehaviorCheck.WalkRunCheck = 0;

            if (runCheck == true)
            {
                runCheck = false;
            }


            //if (_directionNumber == 1)
            //{

            //    characterController.Move(transform.forward.normalized * walkSpeed * Time.deltaTime);
            //    playerBehaviorCheck.IsVertical = 1;
            //}
            //else if (_directionNumber == 2)
            //{
            //    characterController.Move(-transform.forward.normalized * walkSpeed * Time.deltaTime);
            //    playerBehaviorCheck.IsVertical = -1;
            //}
            //else if (_directionNumber == 3)
            //{
            //    characterController.Move(transform.right.normalized * walkSpeed * Time.deltaTime);
            //    playerBehaviorCheck.IsHorizontal = 1;
            //}
            //else if (_directionNumber == 4)
            //{
            //    characterController.Move(-transform.right.normalized * walkSpeed * Time.deltaTime);
            //    playerBehaviorCheck.IsHorizontal = -1;
            //}


        }
        else if (Input.GetKey(KeyCode.LeftShift) && stamina > 0)
        {
            playerBehaviorCheck.WalkRunCheck = 1;

            if (runCheck == false)
            {
                runCheck = true;
            }

            stamina -= Time.deltaTime * 15;

            //if (_directionNumber == 1)
            //{
            //    characterController.Move(transform.forward.normalized * runSpeed * Time.deltaTime);
            //    playerBehaviorCheck.IsVertical = 1;
            //}
            //else if (_directionNumber == 2)
            //{
            //    characterController.Move(-transform.forward.normalized * runSpeed * Time.deltaTime);
            //    playerBehaviorCheck.IsVertical = -1;
            //}
            //else if (_directionNumber == 3)
            //{
            //    characterController.Move(transform.right.normalized * runSpeed * Time.deltaTime);
            //    playerBehaviorCheck.IsHorizontal = 1;
            //}
            //else if (_directionNumber == 4)
            //{
            //    characterController.Move(-transform.right.normalized * runSpeed * Time.deltaTime);
            //    playerBehaviorCheck.IsHorizontal = -1;
            //}
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
    /// 플레이어가 달리거나 점프를 했을 때 스테미너를 감소 시키기 위한 함수 그게 아니라면 충전
    /// </summary>
    private void playerStamina()
    {
        if (stamina > 100)
        {
            stamina = 100;
        }

        if (stamina <= 0 && staminaCheck == false)
        {
            playerBreatAudio.Play();
            staminaCheck = true;
        }
        else if (stamina >= 30 && staminaCheck == true)
        {
            playerBreatAudio.Pause();
            staminaCheck = false;
        }

        if (!Input.GetKey(KeyCode.LeftShift) && stamina < 100)
        {
            stamina += Time.deltaTime * 10;
        }

        if (stamina < 100)
        {
            if (canvasManager.GetCanvas().transform.Find("StaminaBar").gameObject.activeSelf == false)
            {
                canvasManager.GetCanvas().transform.Find("StaminaBar").gameObject.SetActive(true);
            }
            canvasManager.GetCanvas().transform.Find("StaminaBar").GetComponent<Image>().fillAmount = stamina / 100;
        }
        else
        {
            canvasManager.GetCanvas().transform.Find("StaminaBar").gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 플레이어 앉기 기능
    /// </summary>
    private void playerSit()
    {
        if (Input.GetKey(KeyCode.LeftControl) && !(Input.GetKey(KeyCode.LeftShift) && stamina != 0))
        {
            playerBehaviorCheck.SitCheck = 1;
            headTrs.localPosition = new Vector3(1, 0, 0);
        }
        else
        {
            playerBehaviorCheck.SitCheck = 0;
            headTrs.localPosition = new Vector3(0, 0, 0);
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

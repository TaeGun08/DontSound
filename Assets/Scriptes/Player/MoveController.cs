using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveController : MonoBehaviour
{
    private CanvasManager canvasManager; //ĵ�����Ŵ���

    private PlayerBehaviorCheck playerBehaviorCheck; //�÷��̾��� �ൿ�� üũ�ϴ� ��ũ��Ʈ

    private CharacterController characterController; //�÷��̾��� ĳ���� ��Ʈ�ѷ�

    [Header("�÷��̾� ������ ����")]
    [SerializeField, Tooltip("�÷��̾��� �ȴ� �ӵ�")] private float walkSpeed;
    [SerializeField, Tooltip("�÷��̾��� �ٴ� �ӵ�")] private float runSpeed;
    [SerializeField, Tooltip("�÷��̾��� �ɾƼ� �ȴ� �ӵ�")] private float sitSpeed;
    private bool runCheck = false; //�÷��̾ �޸��� �ִ��� Ȯ���ϱ� ���� �Լ�, �޸��� �ִٸ� ���׹̳ʰ� �������� �ʵ��� ����
    [SerializeField, Tooltip("�÷��̾��� �߷�")] private float gravity;
    [Space]
    [SerializeField, Tooltip("�÷��̾��� ���׹̳�")] private float stamina;
    [Space]
    [SerializeField, Tooltip("�÷��̾��� �Ӹ� ��ġ")] private Transform headTrs;

    private AudioSource playerStepAudio; //�÷��̾� �߰��� ���带 ���� �����
    private AudioSource playerBreatAudio; //�÷��̾� ���Ҹ��� ���� �����
    private float audioTimer; //������� �ð��� ������ �����ų �� �ְ� ���� ����
    private bool audioPlay; //������� �����ų ����
    private float audioDelay; //�� �� �������� �����ų���� ���� �� �ְ� ���� ����
    private float curAudioSound; //���� ����
    private bool staminaCheck = false; //���׹̳ʰ� ���� ��ġ ���Ϸ� ���������� üũ�ϱ� ���� ����

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
    /// �÷��̾�� ���õ� Ÿ�̸Ӹ� ��Ƶδ� ��
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
    /// �÷��̾ �Ȱ� ���ִ� �Լ�
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
    /// �÷��̾ �Ȱ� �ִ��� �ٰ� �ִ����� ���� �ӵ��� �����ϱ� ���� �Լ�
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
    /// �÷��̾� �ɱ� ���
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
    /// ���콺 ȸ���� ���� �÷��̾ ȸ����Ű�� ���� �Լ�
    /// </summary>
    private void playerMouseRotate()
    {
        transform.rotation = Quaternion.Euler(0f, CameraManager.Instance.GetCamera(0).transform.eulerAngles.y, 0f);
    }
}

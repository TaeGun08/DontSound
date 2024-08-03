using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    private GameManager gameManager; //���ӸŴ���

    private NavMeshAgent agent; //������ AI

    private Animator anim; //������ �ִϸ�����

    private GameObject player; //���Ͱ� ������ �÷��̾�
    private PlayerBehaviorCheck playerBehavior; //�÷��̾ ������ �ֺ����� �����̴��� üũ�ϱ� ���Ͽ� �޾ƿ� ����

    private bool roarCheck = false; //�����Ҹ��� ���� angryMode�� �����ϱ� ���� ����
    private float roarTimer; //�����Ҹ� ���� ���߰� �ϱ� ���� ����

    private bool angryMode = false; //�����̿��� �Ҹ��� ������ �� �������� �ϱ� ���� ����
    private float angryTimer = 0f; //angryMode�� ���ӽð�

    private bool aggroCheck = false; //������ ��׷θ� üũ�ϱ� ���� ����
    private float aggroTimer = 0f; //������ ��׷ΰ� Ǯ���� �ð�

    private bool sniffCheck = false; //������ ���� �´� ����� �۵��� �ߴ��� üũ�ϱ� ���� ����
    private float sniffTimer = 0f; //���Ͱ� ���� �´� ���� ���͸� ���߰� ���� �ð�
    private bool sniffCoolCheck;
    private float sniffCoolTimer = 0f;

    private bool randomPos = false; //���Ͱ� ���� �߰��ϰ� ���� ���� �� ������ ��ġ�� �����̰� ������ִ� ����
    private float randomPosTimer = 0f; //���Ͱ� ���� ������ ��ġ�� �̵��� �� �ְ� ������ִ� ����
    [SerializeField] private int randomNumber = 0; //������ ��ȣ�� �޾ƿ� ����

    [Header("�÷��̾ Ȯ���� �ݶ��̴�")]
    [SerializeField] private List<FindPlayerColl> findPlayers;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
        player = gameManager.GetPlayer();
        playerBehavior = player.GetComponent<PlayerBehaviorCheck>();

        int randomNum = Random.Range(0, 9);
        randomNumber = randomNum;
    }

    private void Update()
    {
        timers();
        playerBehaviorCheck();
    }

    /// <summary>
    /// ������ Ÿ�̸�
    /// </summary>
    private void timers()
    {
        if (roarCheck == true && angryMode == false)
        {
            roarTimer += Time.deltaTime;

            if (roarTimer <= 0.1f)
            {
                findPlayers[0].SetBoxCollSize(new Vector3(100f, 40f, 100f));
                anim.SetBool("isRoar", true);
                agent.speed = 0f;
            }

            if (roarTimer >= 5f)
            {
                anim.SetBool("isRoar", false);
                anim.SetBool("isAngry", true);
                agent.speed = 3.5f;
                roarTimer = 0f;
                roarCheck = false;
                angryMode = true;
            }
        }

        if (angryMode == true && findPlayers[1].GetPlayer() == null && roarCheck == false)
        {
            angryTimer += Time.deltaTime;

            if (angryTimer >= 20f)
            {
                findPlayers[0].ResetBoxCollSize();
                anim.SetBool("isAngry", false);
                anim.SetBool("isWalk", true);
                angryTimer = 0f;
                agent.speed = 2f;
                angryMode = false;
            }
        }

        if (aggroCheck == true)
        {
            aggroTimer += Time.deltaTime;

            if (aggroTimer >= 10f)
            {
                int randomNum = Random.Range(0, 9);
                randomNumber = randomNum;
                agent.SetDestination(gameManager.GetMonsterPlaceToGo().GetToGoTrs(randomNumber).position);

                aggroTimer = 0;
                aggroCheck = false;
                sniffCheck = true;
            }
        }

        if (sniffCoolCheck == true)
        {
            sniffCoolTimer += Time.deltaTime;

            if (sniffCoolTimer >= 5f)
            {
                sniffCoolTimer = 0;
                sniffCoolCheck = false;
            }
        }

        if (sniffCheck == true && sniffCoolCheck == false)
        {
            sniffTimer += Time.deltaTime;

            if (sniffTimer <= 0.1f)
            {
                agent.speed = 0f;
                anim.SetBool("isSniff", true);
            }

             if (sniffTimer >= 5f)
            {
                anim.SetBool("isSniff", false);
                agent.speed = 2f;
                sniffTimer = 0;
                sniffCheck = false;
                sniffCoolCheck = true;
            }
        }

        if (randomPos == true)
        {
            randomPosTimer += Time.deltaTime;

            if (randomPosTimer >= 10)
            {
                int randomNum = Random.Range(0, 9);
                randomNumber = randomNum;

                randomPosTimer = 0;
                randomPos = false;
            }
        }
    }

    /// <summary>
    /// �÷��̾ �ݶ��̴��� ������ �� �÷��̾ �߰��� �� �ְ� ������ִ� �Լ�
    /// </summary>
    private void playerBehaviorCheck()
    {
        if (findPlayers[1].GetPlayer() != null && playerBehavior.IsBehavior == true)
        {
            if (sniffCheck == false)
            {
                roarCheck = true;
            }

            agent.SetDestination(gameManager.GetPlayer().transform.position);
        }
        else if (findPlayers[0].GetPlayer() != null && playerBehavior.IsBehavior == true)
        {
            if (playerBehavior.WalkRunCheck == 1 && sniffCheck == false)
            {
                roarCheck = true;
            }
            agent.SetDestination(gameManager.GetPlayer().transform.position);

            if (roarCheck == false && angryMode == false)
            {
                anim.SetBool("isWalk", true);
            }
        }
        else if (findPlayers[0].GetPlayer() != null && playerBehavior.IsBehavior == false && angryMode == false)
        {
            aggroCheck = true;
        }
        else if (findPlayers[0].GetPlayer() == null && playerBehavior.IsBehavior == false)
        {
            randomPos = true;

            anim.SetBool("isWalk", true);

            agent.SetDestination(gameManager.GetMonsterPlaceToGo().GetToGoTrs(randomNumber).position);
        }
    }
}

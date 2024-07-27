using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
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

    [Header("�÷��̾ Ȯ���� �ݶ��̴�")]
    [SerializeField] private List<FindPlayerColl> findPlayers;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        player = GameManager.Instance.GetPlayer();
        playerBehavior = player.GetComponent<PlayerBehaviorCheck>();
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
                anim.SetBool("isRoar", true);
                agent.speed = 0f;
            }
            else
            {
                anim.SetBool("isRoar", false);
            }

            if (roarTimer >= 5f)
            {
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

            if (angryTimer >= 10f)
            {
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

            if (aggroTimer >= 5f)
            {
                aggroTimer = 0;
                aggroCheck = false;
                sniffCheck = true;
            }
        }

        if (sniffCheck == true)
        {
            sniffTimer += Time.deltaTime;

            if (sniffTimer <= 0.1f)
            {
                agent.speed = 0f;
                anim.SetBool("isSniff", true);
            }
            else
            {
                anim.SetBool("isSniff", false);
            }

             if (sniffTimer >= 4f)
            {
                agent.speed = 2f;
                sniffTimer = 0;
                sniffCheck = false;
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
            roarCheck = true;
            agent.SetDestination(GameManager.Instance.GetPlayer().transform.position);
        }
        else if (findPlayers[0].GetPlayer() != null && playerBehavior.IsBehavior == true)
        {
            agent.SetDestination(GameManager.Instance.GetPlayer().transform.position);

            if (roarCheck == false && angryMode == false)
            {
                anim.SetBool("isWalk", true);
            }
        }
        else if (findPlayers[0].GetPlayer() != null && playerBehavior.IsBehavior == false)
        {
            if (roarCheck == false && angryMode == false && aggroCheck == false)
            {
                aggroCheck = true;
            }
        }
    }
}

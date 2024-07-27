using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    private NavMeshAgent agent; //몬스터의 AI

    private Animator anim; //몬스터의 애니메이터

    private GameObject player; //몬스터가 추적할 플레이어
    private PlayerBehaviorCheck playerBehavior; //플레이어가 몬스터의 주변에서 움직이는지 체크하기 위하여 받아올 변수

    private bool roarCheck = false; //울음소리를 내고 angryMode에 돌입하기 위한 변수
    private float roarTimer; //울음소리 동안 멈추게 하기 위한 변수

    private bool angryMode = false; //가까이에서 소리를 내었을 때 빨라지게 하기 위한 변수
    private float angryTimer = 0f; //angryMode의 지속시간

    private bool aggroCheck = false; //몬스터의 어그로를 체크하기 위한 변수
    private float aggroTimer = 0f; //몬스터의 어그로가 풀리는 시간

    private bool sniffCheck = false; //몬스터의 냄새 맞는 모션이 작동을 했는지 체크하기 위한 변수
    private float sniffTimer = 0f; //몬스터가 냄새 맞는 동안 몬스터를 멈추게 해줄 시간

    [Header("플레이어를 확인할 콜라이더")]
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
    /// 몬스터의 타이머
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
    /// 플레이어가 콜라이더에 들어왔을 때 플레이어를 추격할 수 있게 만들어주는 함수
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    private GameManager gameManager; //게임매니저

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
    private bool sniffCoolCheck;
    private float sniffCoolTimer = 0f;

    private bool randomPos = false; //몬스터가 적을 추격하고 있지 않을 때 랜덤한 위치로 움직이게 만들어주는 변수
    private float randomPosTimer = 0f; //몬스터가 다음 랜덤한 위치로 이동할 수 있게 만들어주는 변수
    private bool randomArrive = true;//랜덤한위치로 이동을 완료했는지
    private Vector3 randomDetination; //랜덤한 위치

    [Header("플레이어를 확인할 콜라이더")]
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

        //int randomNum = Random.Range(0, 9);
        //randomNumber = randomNum;

        //Vector3 destination = gameManager.GetMonsterPlaceToGo().GetToGoTrs(randomNumber).position;
        //agent.SetDestination(destination);
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
                findPlayers[0].SetBoxCollSize(new Vector3(100f, 40f, 100f));
                anim.SetBool("isRoar", true);
                agent.speed = 0f;
            }

            if (roarTimer >= 5f)
            {
                anim.SetBool("isRoar", false);
                anim.SetBool("isAngry", true);
                agent.speed = 3f;
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
                agent.SetDestination(gameManager.GetMonsterPlaceToGo().GetToGoTrs(Random.Range(0, 9)).position);

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

             if (sniffTimer >= 4f)
            {
                anim.SetBool("isSniff", false);
                agent.speed = 2f;
                sniffTimer = 0;
                sniffCheck = false;
                sniffCoolCheck = true;
            }
        }

        if (randomPos == true && randomArrive == true)
        {
            randomPosTimer += Time.deltaTime;

            if (randomPosTimer >= 5f)
            {
                randomDetination = gameManager.GetMonsterPlaceToGo().GetToGoTrs(Random.Range(0, 9)).position;
                agent.SetDestination(randomDetination);
                randomDetination.y = 0;

                randomPosTimer = 0;
                randomPos = false;
                randomArrive = false;
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
            //aggroCheck = true;
        }
        else if (findPlayers[0].GetPlayer() == null && playerBehavior.IsBehavior == false)
        {
            if (angryMode == false)
            {
                anim.SetBool("isWalk", agent.velocity.x != 0 || agent.velocity.y != 0);
            }

            randomPos = true;

            Vector3 checkPos = transform.position;
            checkPos.y = 0;
            if (Vector3.Distance(randomDetination, checkPos) <= 0.1f && randomArrive == false)
            {
                sniffCheck = true;
                randomArrive = true;
            }
        }
    }
}

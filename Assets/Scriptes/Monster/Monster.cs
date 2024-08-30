using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

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
        if (roarCheck == true && angryMode == false) // 몬스터가 울부짖은 후 화남 상태로 변형
        {
            roarTimer += Time.deltaTime;

            if (roarTimer <= 0.1f) // 몬스터가 플레이어를 추적하는 boxcollider 크기를 더 크게 만들고, 울부짖는 모션을 실행하도록 만듦
            {
                findPlayers[0].SetBoxCollSize(new Vector3(100f, 40f, 100f));
                anim.SetBool("isRoar", true);
                agent.speed = 0f;
            }

            if (roarTimer >= 5f) // 5초 후 울부짖는 모션을 멈추고 플레이어를 빨리 추적할 수 있도록 이동속도를 올려줌, 화남 상태의 모드도 활성화 시켜줌
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
        { // 화남 상태의 모드가 활성화 중이고, 플레이어가 추적 콜라이더에 없다면 일정시간 후 화남 상태를 풀어 줌
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

        if (aggroCheck == true) // 몬스터의 어그로에 풀렸는지 체크
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

        if (sniffCoolCheck == true) // sniff를 다시 하기 위한 쿨타임
        {
            sniffCoolTimer += Time.deltaTime;

            if (sniffCoolTimer >= 5f)
            {
                sniffCoolTimer = 0;
                sniffCoolCheck = false;
            }
        }

        if (sniffCheck == true && sniffCoolCheck == false) // sniff를 할 수 있는 상황인지 체크
        {
            sniffTimer += Time.deltaTime;

            if (sniffTimer <= 0.1f) // sniff 행동을 하기 위해 AI의 이동속도를 0으로 만들어 움직이지 못하게 하고 애니메이션을 작동시킴
            {
                agent.speed = 0f;
                anim.SetBool("isSniff", true);
            }

             if (sniffTimer >= 4f) // 4초 이상의 시간이 지나면 애니메이션을 꺼주고  AI를 다시 움직이게 만들어 줌
            {
                anim.SetBool("isSniff", false);
                agent.speed = 2f;
                sniffTimer = 0;
                sniffCheck = false;
                sniffCoolCheck = true;
            }
        }

        if (randomPos == true && randomArrive == true) // 몬스터가 여러개로 지정한 위치를 랜덤으로 선택해서 갈 수 있게 만들어 줌
        {
            randomPosTimer += Time.deltaTime;

            if (randomPosTimer >= 5f) // 몬스터가 지정한 위치에 도착 후 5초가 지나면 다시 랜덤한 위치를 받아서 움직일 수 있게 만들어 줌
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
    private void playerBehaviorCheck()  //findPlayers[1].GetPlayer()은 몬스터의 근처에 왔을 때 움직이면 체크하는 콜라이더
    {                                                //findPlayers[0].GetPlayer()은 몬스터의 멀리에서 움직였을 때 체크하는 콜라이더
        if (findPlayers[1].GetPlayer() != null && playerBehavior.IsBehavior == true)
        { //플레이어를 추적하는 콜라이더에 있고, 플레이어가 움직이고 있다면 화남 상태로 돌입할 수 있도록 만들어 줌
            if (sniffCheck == false)
            {
                roarCheck = true;
            }

            agent.SetDestination(gameManager.GetPlayer().transform.position);
        }
        else if (findPlayers[0].GetPlayer() != null && playerBehavior.IsBehavior == true)
        { //플레이어를 추적하는 콜라이더에 있고, 플레이어가 움직이고 있다면 추적할 수있게 뛰었다면 화남 상태로 돌입할 수 있게
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
        //else if (findPlayers[0].GetPlayer() != null && playerBehavior.IsBehavior == false && angryMode == false)
        //{
        //    //aggroCheck = true;
        //}
        else if (findPlayers[0].GetPlayer() == null && playerBehavior.IsBehavior == false)
        { //플레이어가 콜라이더 밖에 있으면 랜덤으로 지정한 위치로 이동할 수 있게 만들어 줌
            if (angryMode == false)
            {
                anim.SetBool("isWalk", agent.velocity.x != 0 || agent.velocity.y != 0);
            }

            randomPos = true;

            Vector3 checkPos = transform.position;
            checkPos.y = 0;
            if (Vector3.Distance(randomDetination, checkPos) <= 0.1f && randomArrive == false)
            { //랜덤으로 지정된 위치에 도착하면 sniff를 활성화 시켜주고 다시 랜덤한 위치로 이동할 수 있게 randomArrive도 활성화 해줌
                sniffCheck = true;
                randomArrive = true;
            }
        }
    }
}

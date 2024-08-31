using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Monster : MonoBehaviour
{
    private GameManager gameManager; //���ӸŴ���

    private NavMeshAgent agent; //������ AI

    private Animator anim; //������ �ִϸ�����

    private GameObject player; //���Ͱ� ������ �÷��̾�
    private PlayerBehaviorCheck playerBehavior; //�÷��̾ ������ �ֺ����� �����̴��� üũ�ϱ� ���Ͽ� �޾ƿ� ����

    private bool roarCheck = false; //�����Ҹ��� ���� angryMode�� �����ϱ� ���� ����
    private float roarTimer; //�����Ҹ� ���� ���߰� �ϱ� ���� ����
    private bool roarCollTimeCheck = false; // �����Ҹ��� ��Ÿ���� üũ�ϱ� ���� ����
    private float roarCollTimer; // �����Ҹ��� ��Ÿ��

    private bool angryMode = false; //�����̿��� �Ҹ��� ������ �� �������� �ϱ� ���� ����
    private float angryTimer = 0f; //angryMode�� ���ӽð�

    private bool sniffCheck = false; //������ ���� �´� ����� �۵��� �ߴ��� üũ�ϱ� ���� ����
    private float sniffTimer = 0f; //���Ͱ� ���� �´� ���� ���͸� ���߰� ���� �ð�
    private bool sniffCoolCheck;
    private float sniffCoolTimer = 0f;

    private bool randomPos = false; //���Ͱ� ���� �߰��ϰ� ���� ���� �� ������ ��ġ�� �����̰� ������ִ� ����
    private float randomPosTimer = 0f; //���Ͱ� ���� ������ ��ġ�� �̵��� �� �ְ� ������ִ� ����
    private bool randomArrive = true;//��������ġ�� �̵��� �Ϸ��ߴ���
    private Vector3 randomDetination; //������ ��ġ

    [Header("�÷��̾ Ȯ���� �ݶ��̴�")]
    [SerializeField] private List<FindPlayerColl> findPlayers;

    [Header("���� ȿ���� Ŭ��")]
    [SerializeField] private List<AudioClip> fxsClips;
    private AudioSource monsterAudio;
    private float curAudioSound; //���� ����

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        monsterAudio = transform.Find("MonsterSound").GetComponent<AudioSource>();

        monsterAudio.clip = fxsClips[1];
        monsterAudio.Play();
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

        if (SoundManager.Instance != null && SoundManager.Instance.GetFxsVolume() != curAudioSound)
        {
            monsterAudio.volume = SoundManager.Instance.GetFxsVolume();
        }
    }

    /// <summary>
    /// ������ Ÿ�̸�
    /// </summary>
    private void timers()
    {
        if (roarCheck == true && angryMode == false) // ���Ͱ� ���¢�� �� ȭ�� ���·� ����
        {
            roarTimer += Time.deltaTime;

            if (roarTimer <= 0.1f) // ���Ͱ� �÷��̾ �����ϴ� boxcollider ũ�⸦ �� ũ�� �����, ���¢�� ����� �����ϵ��� ����
            {
                monsterAudio.clip = fxsClips[0];
                monsterAudio.Play();
                findPlayers[0].SetBoxCollSize(new Vector3(100f, 40f, 100f));
                anim.SetBool("isRoar", true);
                agent.speed = 0f;
                roarCollTimeCheck = true;
            }

            if (roarTimer >= 5f) // 5�� �� ���¢�� ����� ���߰� �÷��̾ ���� ������ �� �ֵ��� �̵��ӵ��� �÷���, ȭ�� ������ ��嵵 Ȱ��ȭ ������
            {
                if (SoundManager.Instance != null)
                {
                    SoundManager.Instance.SetBgmClips(2);
                }
                monsterAudio.loop = true;
                monsterAudio.clip = fxsClips[2];
                monsterAudio.Play();
                anim.SetBool("isRoar", false);
                anim.SetBool("isAngry", true);
                agent.speed = 3f;
                roarTimer = 0f;
                roarCheck = false;
                angryMode = true;
            }
        }

        if (roarCollTimeCheck == true)
        {
            roarCollTimer += Time.deltaTime;

            if (roarCollTimer >= 30f)
            {
                roarCollTimeCheck = false;
                roarCollTimer = 0;
            }
        }

        if (angryMode == true && findPlayers[1].GetPlayer() == null && roarCheck == false)
        { // ȭ�� ������ ��尡 Ȱ��ȭ ���̰�, �÷��̾ ���� �ݶ��̴��� ���ٸ� �����ð� �� ȭ�� ���¸� Ǯ�� ��
            angryTimer += Time.deltaTime;

            if (angryTimer >= 20f)
            {
                if (SoundManager.Instance != null)
                {
                    SoundManager.Instance.SetBgmClips(1);
                }
                monsterAudio.clip = fxsClips[1];
                monsterAudio.Play();
                findPlayers[0].ResetBoxCollSize();
                anim.SetBool("isAngry", false);
                anim.SetBool("isWalk", true);
                angryTimer = 0f;
                agent.speed = 2f;
                angryMode = false;
            }
        }

        if (sniffCoolCheck == true) // sniff�� �ٽ� �ϱ� ���� ��Ÿ��
        {
            sniffCoolTimer += Time.deltaTime;

            if (sniffCoolTimer >= 5f)
            {
                sniffCoolTimer = 0;
                sniffCoolCheck = false;
            }
        }

        if (sniffCheck == true && sniffCoolCheck == false) // sniff�� �� �� �ִ� ��Ȳ���� üũ
        {
            sniffTimer += Time.deltaTime;

            if (sniffTimer <= 0.1f) // sniff �ൿ�� �ϱ� ���� AI�� �̵��ӵ��� 0���� ����� �������� ���ϰ� �ϰ� �ִϸ��̼��� �۵���Ŵ
            {
                agent.speed = 0f;
                anim.SetBool("isSniff", true);
            }

             if (sniffTimer >= 4.5f) // 4�� �̻��� �ð��� ������ �ִϸ��̼��� ���ְ�  AI�� �ٽ� �����̰� ����� ��
            {
                anim.SetBool("isSniff", false);
                agent.speed = 2f;
                agent.SetDestination(randomDetination);
                sniffTimer = 0;
                sniffCheck = false;
                sniffCoolCheck = true;
            }
        }

        if (randomPos == true && randomArrive == true) // ���Ͱ� �������� ������ ��ġ�� �������� �����ؼ� �� �� �ְ� ����� ��
        {
            randomPosTimer += Time.deltaTime;

            if (randomPosTimer >= 5f) // ���Ͱ� ������ ��ġ�� ���� �� 5�ʰ� ������ �ٽ� ������ ��ġ�� �޾Ƽ� ������ �� �ְ� ����� ��
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
    /// �÷��̾ �ݶ��̴��� ������ �� �÷��̾ �߰��� �� �ְ� ������ִ� �Լ�
    /// </summary>
    private void playerBehaviorCheck()  //findPlayers[1].GetPlayer()�� ������ ��ó�� ���� �� �����̸� üũ�ϴ� �ݶ��̴�
    {                                                //findPlayers[0].GetPlayer()�� ������ �ָ����� �������� �� üũ�ϴ� �ݶ��̴�
        if (findPlayers[1].GetPlayer() != null)
        { //�÷��̾ �����ϴ� �ݶ��̴��� �ְ�, �÷��̾ �����̰� �ִٸ� ȭ�� ���·� ������ �� �ֵ��� ����� ��
            if (playerBehavior.IsBehavior == true)
            {
                if (sniffCheck == false && roarCollTimeCheck == false)
                {
                    randomArrive = false;
                    monsterAudio.loop = false;
                    monsterAudio.clip = fxsClips[0];
                    monsterAudio.Play();
                    roarCheck = true;
                }

                if (angryMode == false && playerBehavior.SitCheck == 1)
                {
                    sniffCheck = true;
                }
                else
                {
                    agent.SetDestination(gameManager.GetPlayer().transform.position);
                }
            }
            else if (playerBehavior.IsBehavior == false && angryMode == true)
            {
                agent.SetDestination(gameManager.GetPlayer().transform.position);
            }
            else if (playerBehavior.IsBehavior == false && angryMode == false)
            {
                sniffCheck = true;
            }
        }
        else if (findPlayers[0].GetPlayer() != null && playerBehavior.IsBehavior == true && playerBehavior.SitCheck == 0)
        { //�÷��̾ �����ϴ� �ݶ��̴��� �ְ�, �÷��̾ �����̰� �ִٸ� ������ ���ְ� �پ��ٸ� ȭ�� ���·� ������ �� �ְ�

            if (playerBehavior.WalkRunCheck == 1 && sniffCheck == false && roarCollTimeCheck == false)
            {
                randomArrive = false;
                monsterAudio.loop = false;
                monsterAudio.clip = fxsClips[0];
                monsterAudio.Play();
                roarCheck = true;
            }

            agent.SetDestination(gameManager.GetPlayer().transform.position);

            if (roarCheck == false && angryMode == false)
            {
                Vector3 checkPos = transform.position;
                checkPos.y = 0;

                anim.SetBool("isWalk", Vector3.Distance(randomDetination, checkPos) <= 0.2f ? false : true);
            }
        }
        else if ((findPlayers[0].GetPlayer() == null && playerBehavior.IsBehavior == false) || 
            (findPlayers[0].GetPlayer() != null && (playerBehavior.IsBehavior == true || playerBehavior.IsBehavior == false) && playerBehavior.SitCheck == 1))
        { //�÷��̾ �ݶ��̴� �ۿ� ������ �������� ������ ��ġ�� �̵��� �� �ְ� ����� ��
            Vector3 checkPos = transform.position;
            checkPos.y = 0;

            if (angryMode == false && sniffCheck == false)
            {
                anim.SetBool("isWalk", Vector3.Distance(randomDetination, checkPos) <= 0.2f ? false : true);
            }

            randomPos = true;

            if (Vector3.Distance(randomDetination, checkPos) <= 0.2f && randomArrive == false)
            { //�������� ������ ��ġ�� �����ϸ� sniff�� Ȱ��ȭ �����ְ� �ٽ� ������ ��ġ�� �̵��� �� �ְ� randomArrive�� Ȱ��ȭ ����
                sniffCheck = true;
                randomArrive = true;
            }
        }
    }
}

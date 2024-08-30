using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private CanvasManager canvasManager;
    private SettingManager settingManager;

    [Header("��Ʈ�ѷ�")]
    [SerializeField] private List<GameObject> controllerObjects;

    [Header("�÷��̾�")]
    [SerializeField] private GameObject player;

    [Header("���Ͱ� ���ƴٴ� ���")]
    [SerializeField] private MonsterPlaceToGo monsterPlaceToGo;

    private GameObject option; // �ɼ� UI ������Ʈ
    private List<Button> optionButton = new List<Button>(); // �ɼ� ��ư�� ��� ���� ����Ʈ

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        canvasManager = CanvasManager.Instance;

        settingManager = SettingManager.Instance;

        option = canvasManager.GetCanvas().transform.Find("Option").gameObject;

        for (int iNum = 0; iNum < 3; iNum++)
        {
            optionButton.Add(option.transform.GetChild(0).GetChild(iNum).GetComponent<Button>());
        }

        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.SetBgmClips(1);
        }
    }

    private void Update()
    {
        optionCheck();
    }

    /// <summary>
    /// �ɼ� ��ư�� ����ϴ� �Լ�
    /// </summary>
    private void optionCheck()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (option.activeSelf == false)
            {
                CameraManager.Instance.SetVirtualCameraOnOff(1);
                Cursor.lockState = CursorLockMode.None;
                option.SetActive(true);
            }
            else if (option.activeSelf == true && settingManager.SettingObject().activeSelf == true)
            {
                settingManager.SettingObject().SetActive(false);
            }
            else if (option.activeSelf == true && settingManager.SettingObject().activeSelf == false)
            {
                CameraManager.Instance.SetVirtualCameraOnOff(0);
                Cursor.lockState = CursorLockMode.Locked;
                option.SetActive(false);
            }
        }
       
        optionButton[0].onClick.AddListener(() =>
        {
            CameraManager.Instance.SetVirtualCameraOnOff(0);
            Cursor.lockState = CursorLockMode.Locked;
            option.SetActive(false);
        });

        optionButton[1].onClick.AddListener(() =>
        {
            settingManager.SettingObject().SetActive(true);
        });

        optionButton[2].onClick.AddListener(() =>
        {
            FadeInOut.Instance.SetActive(false, () =>
            {
                SceneManager.LoadSceneAsync("MainTitle");

                FadeInOut.Instance.SetActive(true);
            });
        });
    }

    /// <summary>
    /// ���� �Ŵ������� �����ϴ� ��Ʈ�ѷ���
    /// </summary>
    /// <param name="_controllerNumber">����� ��Ʈ�ѷ� ��ȣ</param>
    /// <returns>0�� MoveController, </returns>
    public GameObject GetControllerObject(int _controllerNumber)
    {
        return controllerObjects[_controllerNumber];
    }

    /// <summary>
    /// ���� �Ŵ������� �����ϴ� �÷��̾�
    /// </summary>
    /// <returns></returns>
    public GameObject GetPlayer()
    {
        return player;
    }

    /// <summary>
    /// ���� �Ŵ������� �����ϴ� ���Ͱ� ���ƴٴ� ���
    /// </summary>
    /// <returns></returns>
    public MonsterPlaceToGo GetMonsterPlaceToGo()
    {
        return monsterPlaceToGo;
    }
}

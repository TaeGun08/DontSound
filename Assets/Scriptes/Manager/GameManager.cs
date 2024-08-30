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

    [Header("컨트롤러")]
    [SerializeField] private List<GameObject> controllerObjects;

    [Header("플레이어")]
    [SerializeField] private GameObject player;

    [Header("몬스터가 돌아다닐 장소")]
    [SerializeField] private MonsterPlaceToGo monsterPlaceToGo;

    private GameObject option; // 옵션 UI 오브젝트
    private List<Button> optionButton = new List<Button>(); // 옵션 버튼을 담기 위한 리스트

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
    /// 옵션 버튼을 담당하는 함수
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
    /// 게임 매니저에서 관리하는 컨트롤러들
    /// </summary>
    /// <param name="_controllerNumber">사용할 컨트롤러 번호</param>
    /// <returns>0번 MoveController, </returns>
    public GameObject GetControllerObject(int _controllerNumber)
    {
        return controllerObjects[_controllerNumber];
    }

    /// <summary>
    /// 게임 매니저에서 관리하는 플레이어
    /// </summary>
    /// <returns></returns>
    public GameObject GetPlayer()
    {
        return player;
    }

    /// <summary>
    /// 게임 매니저에서 관리하는 몬스터가 돌아다닐 장소
    /// </summary>
    /// <returns></returns>
    public MonsterPlaceToGo GetMonsterPlaceToGo()
    {
        return monsterPlaceToGo;
    }
}

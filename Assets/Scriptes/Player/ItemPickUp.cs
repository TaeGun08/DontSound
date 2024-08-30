using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ItemPickUp : MonoBehaviour
{
    private Inventory inventory; //플레이어의 인벤토리

    [Header("아이템 줍기 기능")]
    [SerializeField, Tooltip("아이템을 주울 수 있는 거리")] private float pickUpDistance;

    private float screenHeight; //화면 세로 크기
    private float screenWidth; //화면 가로 크기

    private bool notEscapeCheck; //탈출할 수 없을 때 뜨게 하기 위한 변수
    private float notEscapeTextTimer; //열쇠가 부족하면 일정 시간동안 텍스트를 띄울 시간

    private Color textColor;

    private void Awake()
    {
        inventory = GetComponent<Inventory>();
    }

    private void Start()
    {
        screenHeight = Screen.height;
        screenWidth = Screen.width;
    }

    private void Update()
    {
        timers();
        pickUpItemCheck();
    }

    /// <summary>
    /// 타이머 모음
    /// </summary>
    private void timers()
    {
        if (notEscapeCheck == true)
        {
            notEscapeTextTimer -= Time.deltaTime;

            textColor.a = notEscapeTextTimer;
            CanvasManager.Instance.GetCanvas().transform.Find("DontEscape").GetComponent<TMP_Text>().color = textColor;

            if (notEscapeTextTimer <= 0)
            {
                notEscapeCheck = false;
                notEscapeTextTimer = 0f;
            }
        }
    }

    /// <summary>
    /// 획득할 수 있는 아이템인지 확인하기 위한 함수
    /// </summary>
    private void pickUpItemCheck()
    {
        Ray pickUpRay = Camera.main.ScreenPointToRay(new Vector3(screenWidth * 0.5f, screenHeight * 0.5f));

        if (Physics.Raycast(pickUpRay, out RaycastHit hit, pickUpDistance, LayerMask.GetMask("Item")))
        {
            if (CanvasManager.Instance.GetCanvas().transform.Find("PickUpText").gameObject.activeSelf == false)
            {
                CanvasManager.Instance.GetCanvas().transform.Find("PickUpText").gameObject.SetActive(true);
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (hit.collider.tag == "Flashlight")
                {
                    inventory.SetItem(hit.collider.gameObject);
                    hit.collider.enabled = false;
                    hit.collider.transform.GetChild(0).gameObject.SetActive(true);
                    Transform trsCamera = CameraManager.Instance.GetCamera(0).transform;
                    hit.collider.transform.SetParent(trsCamera);
                    hit.collider.transform.position = trsCamera.position + new Vector3(0f, 0.2f, -0.15f);
                    Vector3 curPos = hit.collider.transform.localPosition;
                    curPos.x = 0f;
                    curPos.y = 0.2f;
                    curPos.z = -0.15f;
                    hit.collider.transform.localPosition = curPos;

                    //hit.collider.transform.rotation = Quaternion.Euler(new Vector3(0f, 90f, 90f));
                    hit.collider.transform.rotation = trsCamera.rotation * Quaternion.Euler(new Vector3(90f, 0f, 0));
                }
                else if (hit.collider.tag == "Key")
                {
                    inventory.SetItem(hit.collider.gameObject);
                    inventory.KeyCountUp();
                    hit.collider.gameObject.SetActive(false);
                }
                else
                {
                    if (inventory.GetKeyCount() == 3)
                    {
                        FadeInOut.Instance.SetActive(false, () =>
                        {
                            SceneManager.LoadSceneAsync("GameEnd");

                            FadeInOut.Instance.SetActive(true);
                        });
                    }
                    else
                    {
                        notEscapeCheck = true;
                        TMP_Text text = CanvasManager.Instance.GetCanvas().transform.Find("DontEscape").GetComponent<TMP_Text>();
                        textColor = text.color;
                        textColor.a = 1;
                        text.color = textColor;
                        notEscapeTextTimer = 2f;
                    }
                }
            }
        }
        else
        {
            if (CanvasManager.Instance.GetCanvas().transform.Find("PickUpText").gameObject.activeSelf == true)
            {
                CanvasManager.Instance.GetCanvas().transform.Find("PickUpText").gameObject.SetActive(false);
            }
        }
    }
}

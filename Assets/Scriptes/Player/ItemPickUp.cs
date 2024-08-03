using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ItemPickUp : MonoBehaviour
{
    private Inventory inventory; //�÷��̾��� �κ��丮

    [Header("������ �ݱ� ���")]
    [SerializeField, Tooltip("�������� �ֿ� �� �ִ� �Ÿ�")] private float pickUpDistance;

    private float screenHeight; //ȭ�� ���� ũ��
    private float screenWidth; //ȭ�� ���� ũ��

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
        pickUpItemCheck();
    }

    /// <summary>
    /// ȹ���� �� �ִ� ���������� Ȯ���ϱ� ���� �Լ�
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
                        SceneManager.LoadSceneAsync("GameEnd");
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

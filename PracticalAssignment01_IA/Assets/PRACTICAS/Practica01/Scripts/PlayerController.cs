using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 1.5f;


    public KeyCode upKey = KeyCode.UpArrow;
    public KeyCode downKey = KeyCode.DownArrow;
    public KeyCode rightKey = KeyCode.RightArrow;
    public KeyCode leftKey = KeyCode.LeftArrow;

    public KeyCode ThrowFishingRodKey = KeyCode.Mouse0;
    public KeyCode CollectFishingRodKey = KeyCode.Mouse1;
    public LineRenderer fishingLine; 

    public GameObject fishHookPrefab;
    public Transform startLinePosition;
    //public float maxLengthFishingRod = 200;
    //private float currentLength;

    private bool hasFish = false;
    private bool fishRodThrown; 
    private Camera camera;
    private bool playerMovementEnabled;
    private Vector3 mousePosition;

    void Start()
    {
        camera = Camera.main;
        fishRodThrown = false;
        playerMovementEnabled = true;
    }

    void Update()
    {
        PlayerMovement();

        if (hasFish)
        {
            return;
        }

        if (Input.GetKeyDown(ThrowFishingRodKey))
        {
            if (!fishRodThrown)
            {
                ThrowFishingRoad();
            }
        }

        if (Input.GetKeyDown(CollectFishingRodKey))
        {
            CollectFishRod();
        }

    }

    private void ThrowFishingRoad()
    {
        playerMovementEnabled = false;
        Debug.Log("Cebo Lanzado");
        //gameObject.SetActive(true);
        fishRodThrown = true;
        mousePosition = camera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        GameObject fishHook = Instantiate(fishHookPrefab);
        fishHook.transform.position = mousePosition;

        ShowFishingLine();
    }

    private void CollectFishRod()
    {
        Debug.Log("Cebo Recogido");
        playerMovementEnabled = true; 
        fishRodThrown = false;
        fishingLine.enabled = false;
        //Destroy or hide Cebo

        //if (fish.parent == fishHook)
        // points++;
        // Destroy(fish.gameObject);

        //gameObject.SetActive(false);
    }

    private void ShowFishingLine()
    {
        fishingLine.startWidth = 2;
        fishingLine.endWidth = 2;
        fishingLine.startColor = Color.black;
        fishingLine.endColor = Color.black;
        fishingLine.sortingOrder = 5; 
        fishingLine.SetPosition(0, startLinePosition.position);
        fishingLine.SetPosition(1, mousePosition);
        fishingLine.enabled = true;
    }

    private void PlayerMovement()
    {
        if (playerMovementEnabled)
        {
            Vector3 moveDirection = Vector3.zero;

            if (Input.GetKey(leftKey))
            {
                moveDirection += Vector3.left;
            }
            if (Input.GetKey(rightKey))
            {
                moveDirection += Vector3.right;
            }
            if (Input.GetKey(upKey))
            {
                moveDirection += Vector3.up;
            }
            if (Input.GetKey(downKey))
            {
                moveDirection += Vector3.down;
            }

            if (moveDirection != Vector3.zero)
            {
                moveDirection.Normalize();

                transform.position += moveDirection * speed * Time.deltaTime;

                Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, moveDirection);
                transform.rotation = targetRotation;
            }
        }
    }
}

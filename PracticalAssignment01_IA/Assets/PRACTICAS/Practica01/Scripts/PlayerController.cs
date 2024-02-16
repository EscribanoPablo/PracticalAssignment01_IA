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
    public KeyCode CollectFishingRodKey = KeyCode.Mouse0;


    public GameObject fishHookPrefab;

    private bool hasFish = false;
    private bool fishRodThrown; 
    private Camera camera;

    void Start()
    {
        camera = Camera.main;
        fishRodThrown = false;
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
        fishRodThrown = true;
        Vector3 mousePosition = camera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        GameObject fishHook = Instantiate(fishHookPrefab);
        fishHook.transform.position = mousePosition;
    }

    private void CollectFishRod()
    {
        fishRodThrown = false;
        //...
    }

    private void PlayerMovement()
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

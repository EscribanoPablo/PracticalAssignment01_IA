using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodAutoDestruction : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); 
    }

    public void OnFishAteMe()
    {
        this.gameObject.tag = "FOODVANISHED";
        spriteRenderer.enabled = false;

        Debug.Log("Food Vanished");
        StartCoroutine(OnfishAte());
    }

    private IEnumerator OnfishAte()
    {
        yield return new WaitForSeconds(3f);
        Destroy(this.gameObject);
        Debug.Log("Food Destroyed");

    }
}

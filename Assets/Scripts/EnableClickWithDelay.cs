using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableClickWithDelay : MonoBehaviour
{
    public GameObject[] SpriteButtons;
    public float EnableButtonDelay = 3f;
    private void OnEnable()
    {
        foreach (GameObject col in SpriteButtons) {
            col.GetComponent<BoxCollider2D>().enabled = false;
            Color temp = col.GetComponent<SpriteRenderer>().color;
            temp.a = 0.5f;
            col.GetComponent<SpriteRenderer>().color = temp;
        }

        Invoke("EnableButton", EnableButtonDelay);
    }

    void EnableButton() {
        foreach (GameObject col in SpriteButtons)
        {
            col.GetComponent<BoxCollider2D>().enabled = true;
            Color temp = col.GetComponent<SpriteRenderer>().color;
            temp.a = 1f;
            col.GetComponent<SpriteRenderer>().color = temp;
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RechargeScript : MonoBehaviour
{
    private bool canClick = true;
    private void Start()
    {
        
    }
    private void OnMouseUp()
    {
        if (canClick)
        {
            SceneManager.LoadScene("02_GameList");
            canClick = false;
        }
    }

}

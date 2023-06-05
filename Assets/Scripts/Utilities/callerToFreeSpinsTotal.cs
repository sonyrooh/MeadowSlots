using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class callerToFreeSpinsTotal : MonoBehaviour
{
    private TextMeshPro winningtext;
    // Start is called before the first frame update
    void Start()
    {
        winningtext = GetComponent<TextMeshPro>();
        if(!GUIManager.instance.TurboBool)
        Invoke("startScrolling", 1.5f);
        else
            Invoke("startScrolling", 1f);

        // winningtext.text = ""+SlotManager.instance.currentSpinWinningAmount.ToString("#,##0");
        winningtext.text = "";
    }
    void startScrolling()
    {
       
        FreeSpinTotal.inst_Freescroll.ScrollTo(winningtext, 0, SlotManager.instance.FreeSpinsWinSum, 3, 0);
     


    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class BigAmounts : MonoBehaviour
{
    private  TextMeshPro winningtext;
    // Start is called before the first frame update
    void Start()
    {
        if (SlotManager.instance.IsFreeSpinsEnabled)
        {
            SlotManager.instance.FreeSpinsWinSum += SlotManager.instance.currentSpinWinningAmount;
            SlotManager.instance.UpdateFreespinsTotal();
        }
        winningtext = GetComponent<TextMeshPro>();
        Bigwinscroll.instant_Bscroll.ScrollTo(winningtext, 0, SlotManager.instance.currentSpinWinningAmount, 3, 0);
        //   winningtext.text = "$"+SlotManager.instance.currentSpinWinningAmount.ToString("F2");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

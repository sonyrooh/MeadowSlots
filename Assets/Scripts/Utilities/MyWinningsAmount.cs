using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class MyWinningsAmount : MonoBehaviour
{
    private TextMeshPro winningtext;
    // Start is called before the first frame update
    void Start()
    {
        winningtext = GetComponent<TextMeshPro>();
          
          winningtext.text = "";
        startScrolling();
    }
    void startScrolling() {
        if (SlotManager.instance.IsFreeSpinsEnabled)
        {
            SlotManager.instance.FreeSpinsWinSum += SlotManager.instance.currentSpinWinningAmount;
            SlotManager.instance.UpdateFreespinsTotal();
        }
        if (GUIManager.instance.SpinNumbers > 0 || GameOperations.instance.noOfFreeSpin > 0) {

            winningtext.text = SlotManager.instance.currentSpinWinningAmount.ToString();
        }
        else
        {
            if (!GUIManager.instance.TurboBool)
                MyWinningScroll.inst_myscroll.ScrollTo(winningtext, 0, SlotManager.instance.currentSpinWinningAmount, 1, 0);
            else
                MyWinningScroll.inst_myscroll.ScrollTo(winningtext, 0, SlotManager.instance.currentSpinWinningAmount, 0.5f, 0);
        }

    }

}

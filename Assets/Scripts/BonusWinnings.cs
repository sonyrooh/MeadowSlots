using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class BonusWinnings : MonoBehaviour
{
    private TextMeshPro winningtext;
    // Start is called before the first frame update
    void OnEnable()
    {
        winningtext = GetComponent<TextMeshPro>();
        if (!GUIManager.instance.TurboBool)
            Invoke("startScrolling", 1.5f);
        else
            Invoke("startScrolling", 1f);
        winningtext.text = "";

    }
    void startScrolling()
    {
     
        if (!GUIManager.instance.TurboBool)
            MyWinningScroll.inst_myscroll.ScrollTo(winningtext, 0, BonusRoundManager.instance.CurrentBonusWinnings, 2, 0);
        else
            MyWinningScroll.inst_myscroll.ScrollTo(winningtext, 0, BonusRoundManager.instance.CurrentBonusWinnings, 1, 0);


    }

}

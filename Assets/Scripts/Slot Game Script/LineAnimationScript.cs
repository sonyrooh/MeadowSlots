using UnityEngine;
using System.Collections;

public class LineAnimationScript : MonoBehaviour
{
    internal static LineAnimationScript instance;
    public GameObject BlackBg;
    public GameObject SlotItemSpiritesAnim;
    LineItem[] rewardedLines;
    private int bonusItemCount;
    private int bonusPayLine;
   // private SlotItem[] matchedSlots;
    void Awake()
    {
        instance = this;
        BlackBg.SetActive(false);
    }

    void Start()
    {
        bonusPayLine = 0;
        bonusItemCount = 0;
    }

    void GetRewardedLineScripts()
    {
        int count = 0;

        for (int i = 0; i < LineManager.instance.lineItemScripts.Length; i++)
        {
            if (LineManager.instance.lineItemScripts[i].totalWin > 0)
                count++;
        }

        rewardedLines = new LineItem[count];

        count = 0;
        for (int i = 0; i < LineManager.instance.lineItemScripts.Length; i++)
        {
            if (LineManager.instance.lineItemScripts[i].totalWin > 0)
            {
                rewardedLines[count] = LineManager.instance.lineItemScripts[i];
                count++;
            }
        }
    }

    internal void ShowLineAnimations()
    {
        

        if (SlotManager.instance.currentSpinWinningAmount != 0)
        {

            GetRewardedLineScripts();
            CreateBoxesForLines();
            StartCoroutine(DoOneCompleteAnimation());
            SoundFxManager.instance.singleLineIndicationSound.Play();

        }
        else
        {
            GUIManager.instance.ChangeLineInfoToTotalBet();
        }


    }

    private void CreateBoxesForLines()
    {
        for (int i = 0; i < rewardedLines.Length; i++)
        {
            CreateBoxesOnALine(rewardedLines[i]);
        }
    }


    void CreateBoxesOnALine(LineItem lineItemScript)
    {
        GameObject temp;
        GameObject AnimObject;
       // GameObject[] MatchedSlots = new GameObject[5];
        lineItemScript.normalItemBoxContainer = new GameObject();
        lineItemScript.normalItemBoxContainer.tag = "ItemInication";
        int counter = 0;
        for (int i = 0; i < lineItemScript.NoOfItemMatched(); i++)
        {
            counter++;

            if (!lineItemScript.lineSlotItems[i].GetComponentInParent<ColumnScript>().IsColumnWild)
            {
                AnimObject = (GameObject)Instantiate(SlotItemSpiritesAnim, lineItemScript.lineSlotItems[i].transform.position, Quaternion.identity);
                lineItemScript.matchedslots[i] = lineItemScript.lineSlotItems[i].gameObject;

                // Vector3 newtrans = temp.transform.position;

                // newtrans.z = -3.3f;
                // temp.transform.position = newtrans;
                // temp.transform.parent = lineItemScript.normalItemBoxContainer.transform;

                Vector3 newtransAnim = AnimObject.transform.position;

                newtransAnim.z = -1.3f;
                AnimObject.transform.position = newtransAnim;
                AnimObject.transform.parent = lineItemScript.normalItemBoxContainer.transform;
            }
        }
        lineItemScript.normalItemBoxContainer.SetActive(false);
    }

    public float blinkTime = 0.2f;
    public float showslotTime = 5f;
    IEnumerator DoOneCompleteAnimation()
    {
        for (int i = 0; i < 3; i++)
        {
            ShowAllLines();
            yield return new WaitForSeconds(blinkTime);
            HideAllLines();
            yield return new WaitForSeconds(blinkTime);
        }

        for (int i = 0; i < rewardedLines.Length; i++)
        {

            GUIManager.instance.ShowPerLineWinAmount("Line " + (rewardedLines[i].lineNumberIndex + 1).ToString() + " pays " + rewardedLines[i].totalWin.ToString());
            for (int j = 0; j < 2; j++)
            {
                ShowALine(rewardedLines[i]);
               
                yield return new WaitForSeconds(blinkTime);
                HideALine(rewardedLines[i]);
               
                yield return new WaitForSeconds(blinkTime);
            }

            rewardedLines[i].lineGfx.transform.localPosition = new Vector3(rewardedLines[i].lineGfx.transform.localPosition.x, rewardedLines[i].lineGfx.transform.localPosition.y, rewardedLines[i].lineGfx.transform.localPosition.z);
        
            rewardedLines[i].ShowSlotItemAnimation();
            for (int j = 0; j < 2; j++)
            {
                ShowBoxes(rewardedLines[i]);
               
                ShowALine(rewardedLines[i]);
                yield return new WaitForSeconds(showslotTime);
               
                HideBoxes(rewardedLines[i]);
                HideALine(rewardedLines[i]);
                yield return new WaitForSeconds(blinkTime);
            }
            rewardedLines[i].StopSlotitemAnimation();
        }

        StartCoroutine(DoOneCompleteAnimation());
    }

    //void showmatchedSlots(LineItem lineScript) {
    //    for (int i = 0; i < 5; i++)
    //    {
    //        if (lineScript.matchedslots[i] != null)
    //            lineScript.matchedslots[i].SetActive(true);
    //    }
    //}
    //void HideMatchedSlots(LineItem lineScript) {
    //    for (int i = 0; i < 5; i++)
    //    {
    //        if (lineScript.matchedslots[i] != null)

    //            lineScript.matchedslots[i].SetActive(false);
    //    }
    //}
    void HideBoxes(LineItem lineScript)
    {
        lineScript.normalItemBoxContainer.SetActive(false);
        BlackBg.SetActive(false);
    }

    void ShowBoxes(LineItem lineScript)
    {
        BlackBg.SetActive(true);
        lineScript.normalItemBoxContainer.SetActive(true);
       
    }


    void HideALine(LineItem lineScript)
    {
        lineScript.lineGfx.SetActive(false);
    }

    void ShowALine(LineItem lineScript)
    {
       // lineScript.lineGfx.SetActive(true);
        for (int i = 0; i < 5; i++)
        {
            if (lineScript.matchedslots[i] != null)
                lineScript.matchedslots[i].SetActive(true);
        }
    }

    void ShowAllLines()
    {
        GUIManager.instance.ChangeLineInfoToTotalBet();
        for (int i = 0; i < rewardedLines.Length; i++)
        {
            rewardedLines[i].lineGfx.transform.localPosition = new Vector3(rewardedLines[i].lineGfx.transform.localPosition.x, rewardedLines[i].lineGfx.transform.localPosition.y, 0);
            rewardedLines[i].lineGfx.SetActive(true);
           // showmatchedSlots(rewardedLines[i]);
        }
    }

    void HideAllLines()
    {
        for (int i = 0; i < rewardedLines.Length; i++)
        {
            rewardedLines[i].lineGfx.SetActive(false);
           
        }
    }


    internal void StopAllAnimationAndEffect()
    {

        GUIManager.instance.ChangeLineInfoToTotalBet();
        StopAllCoroutines();
        _DestroyAllBoxes();
        bonusPayLine = 0;
        bonusItemCount = 0;
    }

    void _DestroyAllBoxes()
    {
       
        BlackBg.SetActive(false);
        if (rewardedLines != null)
        {
            for (int i = 0; i < rewardedLines.Length; i++)
            {
                Destroy(rewardedLines[i].normalItemBoxContainer);
               

            }
            
        }
    }

    private int GetNoOfBonusItemOnPayLine()
    {
        for (int i = 0; i < LineManager.instance.lineItemScripts.Length; i++)
        {
            if (LineManager.instance.lineItemScripts[i].bonusSlotItemCount >= 3)
            {
                bonusPayLine = i;
                bonusItemCount = LineManager.instance.lineItemScripts[i].bonusSlotItemCount;
                break;
            }
        }
        return bonusItemCount;
    }

}

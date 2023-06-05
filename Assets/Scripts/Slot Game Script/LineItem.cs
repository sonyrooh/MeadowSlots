using UnityEngine;
using System.Collections;

public class LineItem : MonoBehaviour
{
    public static LineItem Instance;
    public int[] rowIndex;
    Vector2[] lineCoordinates;
    internal SlotItem[] lineSlotItems;
    public GameObject lineGfx;
    int firstItemIndex = -1;
    int firstIndexWild = 0;
    internal int count = 0;
    internal int bonusSlotItemCount;
    int WildInSequanceCount;
    internal GameObject normalItemBoxContainer, wildItemBoxContainer, bonusItemContainer;
    private int animationSelection;
    private GameObject wildItemIndication;
    private bool showEffect;
    internal int winingMultiplyer;
    internal float totalWin;
    internal int lineNumberIndex = 0;
    public GameObject[] matchedslots = new GameObject[5];
    public GameObject InfoObj;

    void Awake()
    {
        Instance = this;
        WildInSequanceCount = 0;
    }


    void Start()
    {
        lineCoordinates = new Vector2[5];

        for (int i = 0; i < rowIndex.Length; i++)
        {
            lineCoordinates[i] = new Vector2(i, rowIndex[i]);
        }

        lineSlotItems = new SlotItem[5];
        lineGfx.SetActive(false);

    }

    internal void Reset()
    {
      
        winingMultiplyer = 0;
        totalWin = 0;
        StopAllCoroutines();
        StopSlotitemAnimation();
        showEffect = false;
        count = 0;
        lineGfx.transform.localPosition = new Vector3(lineGfx.transform.localPosition.x, lineGfx.transform.localPosition.y, 0);
        bonusSlotItemCount = 0;
        WildInSequanceCount = 0;
        GameOperations.instance.ScatterSlotItemCount = 0;
        GameEffects.onceBonus = false;
        GameOperations.instance.DestroyIndication();
        if (InfoObj != null) {
            Destroy(InfoObj);
            OnSlotItemClicked.ItemClicked = false;

        }
        for (int i = 0; i < 5; i++)
        {
            matchedslots[i] = null;
        }
    }


    internal void SetCurrentLineItems()
    {
        for (int i = 0; i < 5; i++)
        {
            lineSlotItems[i] = ColumnManager.instance.GetSlotItemAt((int)lineCoordinates[i].x, (int)lineCoordinates[i].y);
        }
    }



    internal void TraceForCombinations()
    {
        TraceForWildOnly();
        TraceForNormalCombinations();
        if (count != 0)
            CalculateReward();
        else
            CalculateRewardForWild();

        TraceForBonus();
    }


    internal void TraceForNormalCombinations()
    {
        firstItemIndex = -1;
        count = 0;
        for (int i = 0; i < 5; i++)
        {
            if (WildInSequanceCount < 2)
            {
                if (lineSlotItems[i].itemType == SlotItemType.Wild)
                {
                    count++;
                    continue;
                }
                if (firstItemIndex == -1 && lineSlotItems[i].itemType == SlotItemType.Normal)
                {
                    
                    count++;
                    firstItemIndex = lineSlotItems[i].animationIndex;
                }
                else if (lineSlotItems[i].animationIndex == firstItemIndex)
                {
                    count++;
                }
                else
                    break;
            }
        }
    }


    void CalculateReward()
    {
        winingMultiplyer = PayTable.instance.CkeckInPayTable(firstItemIndex, count);
        totalWin = winingMultiplyer * SlotManager.instance.betPerLineAmount;
        animationSelection = Random.Range(0, 3);


    }

    void CalculateRewardForWild()
    {
        if (WildInSequanceCount >= 2)
        {
            PayTable.instance.SetMultiplier(WildInSequanceCount, PayTable.instance.WildInSequanceAmount);
            winingMultiplyer = PayTable.instance.WiningMultiPlyerOnALine();
            totalWin = winingMultiplyer * SlotManager.instance.betPerLineAmount;
            animationSelection = 6;

        }
    }

    /// Trace For Bonus Item
    internal void TraceForBonus()
    {

        bonusSlotItemCount = 0;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 5; j++)
            {
              SlotItem  Item = (SlotItem)ColumnManager.instance.GetSlotItemAt(j, i);
                if (Item.itemType == SlotItemType.Bonus)
                {
                    bonusSlotItemCount++;

                }
            }
        }
        if (bonusSlotItemCount >= 3 && !SlotManager.instance.IsFreeSpinsEnabled)
        {
            SlotManager.CanSpinAgain = false;
            SlotManager.instance.IsBonusEnabled = true;
            OnSlotItemClicked.CanShowSlotInfo = false;
            GameEffects.instance.ShowBonusPopupEffect();

          
        }
    }

    // Trace For Wild Item..
    internal void TraceForWildOnly()
    {
        for (int i = 0; i < 5; i++)
        {
            if (lineSlotItems[i].itemType == SlotItemType.Wild)
            {
                WildInSequanceCount++;
         
            }
            else
            {
                break;
            }
        }
    }

    internal int NoOfItemMatched()
    {
        if (count == 0)
            return WildInSequanceCount;
        else
        {
            return count;
        }
    }
  public  void showmatchedSlots()
    {
        for (int i = 0; i < 5; i++)
        {
            if (matchedslots[i] != null)
                matchedslots[i].GetComponent<MeshRenderer>().enabled = true;
         
        }
    }
   public void HideMatchedSlots()
    {
        for (int i = 0; i < 5; i++)
        {
            if (matchedslots[i] != null)
              matchedslots[i].GetComponent<MeshRenderer>().enabled = false;
         
        }
    }
    // Show Slot Item Animation
    internal void ShowSlotItemAnimation()
    {
        for (int i = 0; i < NoOfItemMatched(); i++) {
            if (!lineSlotItems[i].ParentColumn.IsColumnWild)
            {
                Vector3 newtrans = lineSlotItems[i].transform.position;
                newtrans.z = -3f;
                lineSlotItems[i].transform.position = newtrans;
            }
          //  iTween.PunchScale(lineSlotItems[i].gameObject, new Vector3(0.3f, 0.3f, 0.3f), 2);
    }
       // HideMatchedSlots();

        //for (int i = 0; i < NoOfItemMatched(); i++)
        //{
        //    lineSlotItems[i].myEffectScript.Highlight(animationSelection);
        //}
    }

    internal void ShowBonusItemAnimation()
    {

        for (int i = 0; i < 5; i++)
        {
            if (lineSlotItems[i].itemType == SlotItemType.Bonus)
                lineSlotItems[i].myEffectScript.Highlight(animationSelection);
        }
    }

    internal void StopSlotitemAnimation()
    {

        for (int i = 0; i < NoOfItemMatched(); i++)
        {
            Vector3 newtrans = lineSlotItems[i].transform.position;
            newtrans.z = 1f;
            lineSlotItems[i].transform.position = newtrans;
           

        }
       //  showmatchedSlots();
        //for (int i = 0; i < NoOfItemMatched(); i++)
        //{

        //    lineSlotItems[i].myEffectScript.UnHighlight();
        //}
    }

    internal void EnableLine(bool enable)
    {
        lineGfx.SetActive(enable);
    }

}// End Script

using UnityEngine;
using System.Collections;

public class LineManager : MonoBehaviour
{

    /// LineManager Script Instance..
    public static LineManager instance;
    public LineItem []lineItemScripts;
    public bool BigBigWins = false;
    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        //for (int i = 0; i < lineItemScripts.Length; i++)
        //{
        //    for (int j = 0; j < lineItemScripts[i].lineGfx.transform.childCount; j++)
        //    {
        //        lineItemScripts[i].lineGfx.transform.GetChild(j).GetComponent<Renderer>().material.SetColor("_TintColor", LineColors.instance.lineColors[i]);
        //    }
        //}
    }
  
    internal void SetLinesItems()
    {
        for (int i = 0; i < SlotManager.instance.noOfLinesSelected; i++)
        {
            lineItemScripts[i].SetCurrentLineItems();
            lineItemScripts[i].lineNumberIndex = i;
        }   
    }

    internal void TraceForCombinations()
    {
        for (int i = 0; i < SlotManager.instance.noOfLinesSelected; i++)
        {
            lineItemScripts[i].TraceForCombinations();   
        }   
    }

    internal void CheckForSpecialEffect()
    {

        if (CheckForJackPot())
            GameEffects.instance.JackPotShow();
        else
        if (CheckForCleanSweep())
            GameEffects.instance.CleanSweepShow();
        else if (CheckForSuperWin())
            GameEffects.instance.SuperWinShow();
        else if (CheckForSpecialWin())
            GameEffects.instance.SpecialWinShow();
        else if (CheckForMegaWin())
            GameEffects.instance.MegaWinShow();
        else if (CheckForBigWin())
           GameEffects.instance.BigWinShow();  
     
       
    
    }

  


   



    bool CheckForJackPot()
    {
        bool isJackpot = false;

        float totalBet = SlotManager.instance.totalBetAmount;
        if (SlotManager.instance.currentSpinWinningAmount >= (totalBet * 100))
        {
            isJackpot = true;
            BigBigWins = true;
            GUIManager.instance.UpdateWiningAmount();

           // Game.noOfCoinsLeft += SlotManager.instance.currentSpinWinningAmount;
            PlayerPrefs.SetFloat("LastWinAmm", SlotManager.instance.currentSpinWinningAmount);
            PlayerPrefs.Save();
            GUIManager.instance.UpdateGUI();

        }
        return isJackpot;
    }

    bool CheckForCleanSweep()
    {
        bool isCleanSweep = false;

        float totalBet = SlotManager.instance.totalBetAmount;
        if (SlotManager.instance.currentSpinWinningAmount >= (totalBet * 50))
        {
            isCleanSweep = true;
            BigBigWins = true;
            GUIManager.instance.UpdateWiningAmount();

          //  Game.noOfCoinsLeft += SlotManager.instance.currentSpinWinningAmount;
            PlayerPrefs.SetFloat("LastWinAmm", SlotManager.instance.currentSpinWinningAmount);
            PlayerPrefs.Save();
            GUIManager.instance.UpdateGUI();

        }

        return isCleanSweep;
    }

    bool CheckForSuperWin()
    {
        bool IsSuperWin = false;

        float totalBet = SlotManager.instance.totalBetAmount;
        if (SlotManager.instance.currentSpinWinningAmount >= (totalBet * 40))
        {
            IsSuperWin = true;
            BigBigWins = true;
            GUIManager.instance.UpdateWiningAmount();

          //  Game.noOfCoinsLeft += SlotManager.instance.currentSpinWinningAmount;
            PlayerPrefs.SetFloat("LastWinAmm", SlotManager.instance.currentSpinWinningAmount);
            PlayerPrefs.Save();
            GUIManager.instance.UpdateGUI();

        }

        return IsSuperWin;
    }
    bool CheckForSpecialWin()
    {
        bool IsSpecialWin = false;

        float totalBet = SlotManager.instance.totalBetAmount;
        if (SlotManager.instance.currentSpinWinningAmount >= (totalBet * 30))
        {
            IsSpecialWin = true;
            BigBigWins = true;
            GUIManager.instance.UpdateWiningAmount();

         //   Game.noOfCoinsLeft += SlotManager.instance.currentSpinWinningAmount;
            PlayerPrefs.SetFloat("LastWinAmm", SlotManager.instance.currentSpinWinningAmount);
            PlayerPrefs.Save();
            GUIManager.instance.UpdateGUI();

        }

        return IsSpecialWin;
    }
    bool CheckForMegaWin()
    {
        bool IsMegaWin = false;
   

        float totalBet = SlotManager.instance.totalBetAmount;
        if (SlotManager.instance.currentSpinWinningAmount >= (totalBet * 20))
        {
            IsMegaWin = true;
            BigBigWins = true;
            GUIManager.instance.UpdateWiningAmount();

          //  Game.noOfCoinsLeft += SlotManager.instance.currentSpinWinningAmount;
            PlayerPrefs.SetFloat("LastWinAmm", SlotManager.instance.currentSpinWinningAmount);
            PlayerPrefs.Save();
            GUIManager.instance.UpdateGUI();

        }

        return IsMegaWin;
    }
    bool CheckForBigWin()
    {
        bool IsBigWin = false;

        float totalBet = SlotManager.instance.totalBetAmount;
        if (SlotManager.instance.currentSpinWinningAmount >= (totalBet * 15))
        {
            IsBigWin= true;
            BigBigWins = true;
            GUIManager.instance.UpdateWiningAmount();

         //   Game.noOfCoinsLeft += SlotManager.instance.currentSpinWinningAmount;
            PlayerPrefs.SetFloat("LastWinAmm", SlotManager.instance.currentSpinWinningAmount);
            PlayerPrefs.Save();
            GUIManager.instance.UpdateGUI();

        }

        return IsBigWin;
    }

    bool CheckForBonusWin()
    {
        bool isBonusWin = false;

        
       
            for (int i = 0; i < SlotManager.instance.noOfLinesSelected; i++)
            {
                if (lineItemScripts[i].bonusSlotItemCount >= 12)
                {
                    if (!SlotManager.instance.IsFreeSpinsEnabled)
                    {
                        isBonusWin = true;
                    BigBigWins = true;
               
                    }
                    else
                    {
                        SlotManager.instance.bonusinFreeSpin = true;
                    }
                    break;
                }
            }

      
        return isBonusWin;
    }


    

    internal void ResetAllLines()
    {   
        for (int i = 0; i < SlotManager.instance.noOfLinesSelected; i++)
        {
            lineItemScripts[i].Reset();
        }
    }





  
   

    internal void ShowEabledLine(bool enable)
    {
        for (int i = 0; i < SlotManager.instance.noOfLinesSelected;i++ )

        {
            lineItemScripts[i].EnableLine(enable);
        }
    }

}// End Script..

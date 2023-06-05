using UnityEngine;
using System.Collections;
//using System;
using CodeStage.AntiCheat.ObscuredTypes;
using gm.api.domain;
using gm.api.domain.Models;

public class SlotManager : GamePlaySocketManager
{
    /// SlotManager Script Instance ..
    internal static SlotManager instance;
    /// Slot Item Prefeb GameObject
    /// 
    public GameObject themeSlotItemPrefab;
    public GameObject themePayTablePrefab;

    public float probality;
    public GameObject spinButton;
    public GameObject BonusMenuPrefab;

    public GameObject WinningFly;

    internal GameObject slotItemPrefab;
    /// Item Spining Speed.
    public float spinSpeed = 18;
    /// Vertical Gap B/W Slot Item..
    public float verticalGap = 0.5f;
    /// Item Matched In Line Info
    private int countElement;
    /// Slot Machine Rolling Time..
    public float timeOfSpin = 4;
    internal bool bonusinFreeSpin;
    /// Slot Is Spining Or Not..
    internal bool isSpinning = false;

    internal ObscuredInt noOfLinesSelected = 25;
    internal ObscuredFloat betPerLineAmount = 1f;
    public ObscuredFloat totalBetAmount;
    public ObscuredFloat currentSpinWinningAmount;

    [Header("Min Limits")]
    private float minForGambling;
    private float minForWheel;
    private float minForTeasure;
    public static bool CanSpinAgain;
    internal ObscuredFloat totalNoOfLines = 25;
    public GameObject GuiCam;
    public ObscuredFloat FreeSpinsWinSum;
    public ObscuredFloat TotalFreeSpins;
    public bool spinagainability;
    public TextMesh FreespinsTotalSumMesh;
    public GameObject WinShowPanel;
    public GameObject FadePanel;
    public bool IsFreeSpinsEnabled, IsBonusEnabled;

    public int playerSpins;
   private string spinType = "null";

    public int GameSessionFreeSpins = 0;

    internal float game_nakha;
    internal float pakar_nakha;
    internal bool nakha_shta = false;
    internal int winnings_session;
    internal int totalgamebets;
    public GmPlayModel currentPlay;
    //testing
    void Awake()
    {
       // Game.noOfCoinsLeft = 1000;
        playerSpins = 0;
        totalgamebets = 0;
        instance = this;
        CanSpinAgain = true;
       
          //  Application.LoadLevelAdditive("My Theme");
            slotItemPrefab = themeSlotItemPrefab;
        //   GUIManager.instance.payTablePopupPrefab = themePayTablePrefabArray[2];
        noOfLinesSelected = 25;


    }

    private void Start()
    {

        Connect();
    }
    private void Update()
    {
        spinagainability = CanSpinAgain;
       
    }

    //public void sendDummyBalance() {

    //    InvokePlayGame(gameId, gameName, 0);
    //   currentSpinWinningAmount = 10000;

    // InvokeCompleteGame(gameId, gameName, currentPlay.SecurityToken, currentSpinWinningAmount);
    //}

    internal void SpinButtonClicked()
    {
        if (WinShowPanel != null)
        {
            Destroy(WinShowPanel);
        }
        if (FadePanel != null)
        {
            Destroy(FadePanel);
        }
        GameSessionFreeSpins = 0;

        if (ColumnManager.instance.wildclones.Count != 0 && !ColumnManager.instance.isStickyWild)
        {
            for (int i = 0; i < ColumnManager.instance.wildclones.Count; i++)
            {

                Destroy(ColumnManager.instance.wildclones[i]);

            }
            ColumnManager.instance.wildclones.Clear();

        }

        if (isSpinning)
            return;


        if (IsFreeSpinsEnabled)
            spinType = "bonus";
        else
            spinType = "paid";


        SoundFxManager.instance.spinSound.Play();

        ResetAll();

        GUIManager.instance.ShowPerLineWinAmount("Good Luck !!");
        isSpinning = true;
        Game.currentGameState = GameState.isSpining;
        OnSlotItemClicked.CanShowSlotInfo = false;
        if (!IsFreeSpinsEnabled)
        {
            Game.noOfCoinsLeft -= totalBetAmount;
            // send spin info

            playerSpins++;
            totalgamebets += (int)totalBetAmount;
        }

        ResetAllAmount();

        if (!IsFreeSpinsEnabled)                             // if player plays with a paid spin
            InvokePlayGame(gameId, gameName, totalBetAmount);
        else
            InvokePlayGame(gameId, gameName, 0);

        StartCoroutine(ColumnManager.instance.StartSpinningColumn());
       // GUIManager.instance.StopSpinButton.SetActive(true);

               // if player plays with a free spin


    }

    public override void OnPlayGame(GmWebSocketResponse<GmPlayModel> res)
    {
        if (res.HasError)
        {
            //res.ErrorMessage
        }
        currentPlay = res.Data;
        GUIManager.instance.SetGuiButtonState(false);
        if (Game.noOfCoinsLeft+totalBetAmount != (float) currentPlay.BalanceBefore)
        {
            Time.timeScale = 0;
            DisplayErrorOnScreen.Instance.NetworkError("Wallet balance mismatched!", "Kindly Login again to fetch your wallet balance");
            Game.noOfCoinsLeft = (float)currentPlay.BalanceBefore;

            GUIManager.instance.UpdateGUI();
        }

    }

    public override void OnCompleteGame(GmWebSocketResponse<GmPlayModel> res)
    {
        if (res.HasError) { 
        
        }
        currentPlay = res.Data;

        print("wallet balance is " + currentPlay.BalanceAfter.ToString() + "\n game balance is " + Game.noOfCoinsLeft.ToString());
       
        // check for user balance on server and on the game
        if (currentPlay.BalanceAfter == (double)Game.noOfCoinsLeft)
        {
            print("Balance Matched");
        }
        else
        {
            if (Game.noOfCoinsLeft < currentPlay.BalanceAfter)
            {
                DisplayErrorOnScreen.Instance.NetworkError("Wallet balance mismatched!", "Kindly Login again to fetch your wallet balance");
                  Time.timeScale = 0;
            }

            Game.noOfCoinsLeft = (float) currentPlay.BalanceAfter;
            GUIManager.instance.UpdateGUI();

            
        }

    }


    /// Call Back When Spining Complete..
    internal void OnSpinComplete()
    {

     

            Invoke("_StopSpinSound",0.3f);
            isSpinning = false;
            Invoke("_SpinCompleted", 1f);
      
    }

 

  

 

    public void MybonusReward() {
        CanSpinAgain = false;
        StartCoroutine(ShowBonusMenu());
        GUIManager.instance.SetGuiButtonState(false);
    }
    

    
    public IEnumerator ShowBonusMenu()
    {
        Instantiate(BonusMenuPrefab, BonusMenuPrefab.transform.position, Quaternion.identity);

       

        yield return null;
    }

   

    void _StopSpinSound()
    {
        SoundFxManager.instance.spinSound.Stop();
    }

    void _SpinCompleted()
    {
        //  AutoMoveAndRotate.instance.isRotate = false;

        LineManager.instance.SetLinesItems(); //keep references of slot items on each selected lines

        LineManager.instance.TraceForCombinations();

     

        // trace for free spins only if Bonus round is not applicable
        if (!SlotManager.instance.IsBonusEnabled)
        {


            GameOperations.instance.TraceForScatterItem();
        

        }

        CalculateWinningAmount();

        LineAnimationScript.instance.ShowLineAnimations();



        if (!IsFreeSpinsEnabled)
        {
            LineManager.instance.CheckForSpecialEffect();
        }


        Game.currentGameState = GameState.isSpiningComplete;
        //  currentSpinWinningAmount = 3000;
        if (currentSpinWinningAmount > 0 && !LineManager.instance.BigBigWins)
        {
             if (!SlotManager.instance.IsBonusEnabled)
                Instantiate(WinningFly, WinningFly.transform.position, Quaternion.identity);
   
           // GameEffects.instance.BigWinShow();
   
        }
        SlotItem.onceCheck = true;

        
        if (GUIManager.instance.SpinNumbers > 0 && CanSpinAgain && !SlotManager.instance.IsBonusEnabled && !LineManager.instance.BigBigWins && !GameOperations.instance.FreeSpinBoolDelay)
        {
            if (currentSpinWinningAmount > 0)
                this.Invoke("SpinAgain", 3f);
            else
                this.Invoke("SpinAgain", 1f);
        }
        else if (GUIManager.instance.SpinNumbers == 0)
        {
            GUIManager.instance.StopAutoSpin();
            OnSlotItemClicked.CanShowSlotInfo = true;
            //   GUIManager.instance.SetGuiButtonState(true);

        }

     

    }

    public void SpinAgain() {
        if (!IsFreeSpinsEnabled && CanSpinAgain) {
            GUIManager.instance.SpinNumbers--;
           
            GUIManager.instance.AutoSpinsCounting();
            CanSpinAgain = false;
            GUIManager.instance.OnSpinButtonClicked();
            CancelInvoke("SpinAgain");
         }
        
    }

    

 
    public void CalculateWinningAmount()
    {
        CanSpinAgain = true;
        currentSpinWinningAmount = 0;

        for (int i = 0; i < LineManager.instance.lineItemScripts.Length; i++)
        {

            currentSpinWinningAmount += LineManager.instance.lineItemScripts[i].totalWin;
        }

      //Kaleemchange  InvokeCompleteGame(gameId,gameName,currentPlay.SecurityToken,currentSpinWinningAmount);

        winnings_session += (int)currentSpinWinningAmount;
            GUIManager.instance.UpdateWiningAmount();
      

        GUIManager.instance.AddWinningsToTotalCredit(currentSpinWinningAmount);
        PlayerPrefs.SetFloat("LastWinAmm", currentSpinWinningAmount);
        PlayerPrefs.Save();
        GUIManager.instance.UpdateGUI();
  
        if (currentSpinWinningAmount <1 && !IsFreeSpinsEnabled && !IsBonusEnabled) {
            print("freespins is " + IsFreeSpinsEnabled);
            GUIManager.instance.SetGuiButtonState(Game.currentGameState != GameState.isSpining);
            GUIManager.instance.StopSpinButton.SetActive(false);

        }
        string SpinResult = "Null";
        float resultamount = 0;
        int SpinBet = 0;


        if (spinType == "paid")
        {

            resultamount = currentSpinWinningAmount;
            SpinResult = "won";




            SpinBet = (int)betPerLineAmount;
        }
        else
        {
            SpinResult = "won";
            resultamount = currentSpinWinningAmount;
            SpinBet = 0;
        }


        if (totalgamebets != 0)
            game_nakha = ((float)winnings_session / (float)totalgamebets * 100);
        else
            game_nakha = (float)winnings_session / 2f;



    }

    public void SendServerBonusWin(float BonusWin) {

        InvokeCompleteGame(gameId, gameName, currentPlay.SecurityToken, BonusWin);

    }

    internal void ResetAll()
    {
        LineManager.instance.ResetAllLines();
        LineAnimationScript.instance.StopAllAnimationAndEffect();
    }

    internal void ResetAllAmount()
    {
        countElement = 0;
        currentSpinWinningAmount = 0;
       // if (GameOperations.instance.noOfFreeSpin == 0)
       
       GUIManager.instance.UpdateGUI();
        GUIManager.instance.UpdateWiningAmount();
        
    }

  

    public void UpdateFreespinsTotal() {
        ScrollTextScript.Scroll(FreespinsTotalSumMesh, float.Parse(FreespinsTotalSumMesh.text.ToString()), FreeSpinsWinSum, 2f, 0f);
    }
    public void ClearFreespinsPreviousSum() {
        FreespinsTotalSumMesh.text = "0";
    }


} // End Script..

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class GUIManager : MonoBehaviour
{
    public static GUIManager instance;

    public Camera guiCamera;

    public TextMesh enabledLineText;
    public TextMesh betAmountText;
    public TextMesh totalBetAmountText;
    public TextMesh winingAmountText;
    public TextMesh totalCoinText;
    public TextMesh perLineWinText;

    public GameObject specialPackPrefeb;
    public GameObject payTablePopupPrefab;
    public GameObject storeScreenPopupPrefab;

    public GameObject[] AllGuiButton;
    public Button[] CanvasButtons;


    private float[] betAmmountArray = { 1f, 2f, 3f, 4f, 5f, 6f, 7f, 8f, 9f, 10f, 11f, 12f, 13f, 14f, 15f, 16f, 17f, 18f };
    private int betAmmountIndex = 6;
    public int SpinNumbers;
    public TextMeshPro AutoSpinText;
    public GameObject autostartButton, autostopbutton,SpinButton, StopSpinButton;
    public GameObject SpinEffects;
    public GameObject AutoSpinPanel;
    public bool SpinEnable;
    public GameObject AutoSpinInfo;
    public GameObject TurboButton;
    public bool TurboButtonStatus = false;
    public bool TurboBool = false;
    public Texture2D TurboOff,TurboON;
    private float currenTotalBet;
    private bool profileshow = true;
    public Transform profileShowT, ProfileHideT;
    public Animator TurboAnimator;
    private bool paylineShowing = false;
    internal bool Isauto = false;
    public GameObject AddtoCreditTextprefab;
    public float ratio;
    public GameObject LobbyPanel;
    public GameObject SceneLoadingBar;
    void Awake()
    {
        AutoSpinInfo.SetActive(false);
        SpinEnable = true;
        AutoSpinPanel.SetActive(false);
        SpinNumbers = 0;
        instance = this;
        LobbyPanel.SetActive(false);



        //Camera.main.aspect = 1.5f;
        //guiCamera.aspect = 1.77f; 
      //  Camera.main.aspect = Screen.width / Screen.height;
      //  guiCamera.aspect = Screen.width / Screen.height;

        betAmmountIndex = 0;
    }



    void Start()
    {

        autostopbutton.SetActive(false);
        StopSpinButton.SetActive(false);

        totalCoinText.text = ""+Game.noOfCoinsLeft.ToString("#,##0");
        betAmountText.text = ""+SlotManager.instance.betPerLineAmount.ToString("#,##0");
        winingAmountText.text = "0";
     

        enabledLineText.text = SlotManager.instance.noOfLinesSelected.ToString();
        ChangeLineInfoToTotalBet();
        CalculateTotalBetAmount();
       

    }

    internal void ChangeLineInfoToTotalBet()
    {
        perLineWinText.text = "Playing " + SlotManager.instance.noOfLinesSelected.ToString() + " Lines " + "( Total Bet = " + (SlotManager.instance.noOfLinesSelected * SlotManager.instance.betPerLineAmount).ToString() + " )";
    }

    public void OnLobbyButton()
    {
      
            SetGuiButtonState(false);
            LobbyPanel.SetActive(true);

        
    }
    public void LobbyYes()
    {
        SceneLoadingBar.SetActive(true);

      //  SceneManager.LoadScene("02_GameList");
    }
    public void LobbyNo()
    {
        SetGuiButtonState(true);
        LobbyPanel.SetActive(false);

    }

    public void OnRechargeButton()
    {

      //  Application.OpenURL(ApiManager.instance.DashboardUrl);
    }
    public void OnTurboButtonClicked()
    {
        TurboButtonStatus = !TurboButtonStatus;
        if (!TurboButtonStatus)
        {

            SoundFxManager.instance.TurboOff.Play();


        }
        else
        {
            SoundFxManager.instance.TurboOn.Play();

        }
        TurboMode();
    }

    public void TurboMode() {
        TurboBool = !TurboBool;

        if (!TurboBool)
        {

     
            SlotManager.instance.timeOfSpin = 0.5f;
            TurboButton.GetComponent<MeshRenderer>().material.mainTexture = TurboOff;
            //  TurboAnimator.SetTrigger("TurboOn");
        }
        else
        {
            SlotManager.instance.timeOfSpin = 1;
            TurboButton.GetComponent<MeshRenderer>().material.mainTexture = TurboON;
           // TurboAnimator.SetTrigger("TurboOff");
        }

    }

    public void GetFreeCoins() {
      //  if (Game.noOfCoinsLeft < 1000) {
      if(Game.noOfCoinsLeft < 5000)
            Game.noOfCoinsLeft = 5000;
        totalCoinText.text = ""+Game.noOfCoinsLeft.ToString("#,##0");

        // }
    }
 
    
   
    void Update()
    {
     

      
        if (SpinEnable && !SlotManager.instance.IsFreeSpinsEnabled)
        {
            SpinButton.GetComponent<MyButtonScript>().ClearTheImage();

        

        }
        else {
           
            SpinButton.GetComponent<MyButtonScript>().blurTheImage();

        
        }
     
     //   AutoSpinText.text = SpinNumbers.ToString();
        if (SpinNumbers < 0)
            SpinNumbers = 0;
        if (SpinNumbers > 0) {
            SpinButton.GetComponent<MyButtonScript>().blurTheImage();

            //SpinButton.SetActive(false);
        }

        if (Game.noOfCoinsLeft < 0)
            Game.noOfCoinsLeft = 0;

       
       
    }

      public void OnSpinButtonClicked()
    {
        if (Game.currentGameState == GameState.isSpining || LineManager.instance.BigBigWins)
            return;
        if (SlotManager.instance.totalBetAmount <= Game.noOfCoinsLeft)
        {




            DisableAllLineOnSpinStart();
           // SpinEffects.SetActive(true);
            SetGuiButtonState(false);
            SpinButton.GetComponent<MyButtonScript>().blurTheImage();
         //   StopSpinButton.SetActive(true);


            //SpinButton.SetActive(false);
            SlotManager.instance.SpinButtonClicked();
            Game.currentGameState = GameState.isReady;
            LineManager.instance.BigBigWins = false;
         
            AutoSpinPanel.GetComponent<Animator>().SetTrigger("AutoDown");

        }
        else
        {

            if (specialPackPrefeb)
            {
               
                Instantiate(specialPackPrefeb, specialPackPrefeb.transform.position, Quaternion.identity);
            }
            StopSpinButton.SetActive(false);
        }

    }

    public void GenerateFlyingBalance(float Amount) {
        GameObject CreditClone = (GameObject)Instantiate(AddtoCreditTextprefab, SpinButton.transform.position + new Vector3(0,2,0), Quaternion.identity);
        CreditClone.transform.GetChild(0).GetComponent<TextMeshPro>().text = "+ " + Amount.ToString();
        iTween.MoveTo(CreditClone, totalCoinText.transform.position + new Vector3(0.5f,0f,0f), 1f);
        iTween.ScaleTo(CreditClone, new Vector3(0.5f, 0.5f, 0.5f), 1);
        Invoke("UpdateGUI", 0.5f);
        Destroy(CreditClone, 1f);
    }

    public void AutoSpinButtonPressed() {
        AutoSpinPanel.SetActive(true);
        SoundFxManager.instance.singleLineIndicationSound.Play();
        if (AutoSpinPanel.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsTag("Idle") || AutoSpinPanel.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsTag("Down"))
        {
            SoundFxManager.instance.AutoSpinUp.Play();
            AutoSpinPanel.GetComponent<Animator>().SetTrigger("AutoUp");
        }
        else
        {
            SoundFxManager.instance.AutoSpinDown.Play();

            AutoSpinPanel.GetComponent<Animator>().SetTrigger("AutoDown");
        }
    }
    public int totalAutoSpins;

    public void AutoSpinsCounting() {

        AutoSpinInfo.transform.GetChild(0).GetComponent<TextMeshPro>().text = (SpinNumbers).ToString(); // auto left
        AutoSpinInfo.transform.GetChild(1).GetComponent<TextMeshPro>().text = (totalAutoSpins-SpinNumbers).ToString(); // auto used
    }

    public void OnAutoSpinClicked()
    {

        if (SlotManager.instance.totalBetAmount <= Game.noOfCoinsLeft)
        {
            SoundFxManager.instance.AutoSpinDown.Play();

            AutoSpinPanel.GetComponent<Animator>().SetTrigger("AutoDown");
            AutoSpinInfo.SetActive(true);


            SpinNumbers--;
            SpinButton.GetComponent<MyButtonScript>().blurTheImage();
            AutoSpinInfo.transform.GetChild(0).GetComponent<TextMeshPro>().text = (SpinNumbers).ToString(); // auto left
            AutoSpinInfo.transform.GetChild(1).GetComponent<TextMeshPro>().text = (totalAutoSpins - SpinNumbers).ToString(); // auto used
            Isauto = true;
            //SpinNumbers = spinNumberSelected;
            autostopbutton.SetActive(true);
        }
        this.OnSpinButtonClicked();



    }
    public void IncrementSpinNumber(int spin) {
        
            SpinNumbers += spin;
       

        }
    public void DecrementSpinNumber(int spin)
    {
        if(SpinNumbers>0)
        SpinNumbers -= spin;


    }
    public void StopAutoSpin() {
    
        autostopbutton.SetActive(false);
        SpinNumbers = 0;
        AutoSpinInfo.SetActive(false);
        StopSpinButton.SetActive(false);
        Isauto = false;
        //ColumnManager.instance.StopSpinnigButtonClicked();

    }
    public void StopSpin()
    {

        StopSpinButton.SetActive(false);

        ColumnManager.instance.StopSpinnigButtonClicked();

    }
    public void AddWinningsToTotalCredit(float amount)
    {

        Game.noOfCoinsLeft += amount;

     

        float ProfitCoins = amount - SlotManager.instance.totalBetAmount;
        if (ProfitCoins > 0 && !LineManager.instance.BigBigWins)
        {
            GenerateFlyingBalance(amount);
        }
        

        UpdateGUI();
    }
    public void AddBonusWinToTotalCredit(float amount)
    {
        SlotManager.instance.winnings_session += (int)amount;

        Game.noOfCoinsLeft += amount;
      
      
          
         

        GenerateFlyingBalance(amount);

        UpdateGUI();

    }

    public void OnPayTableButtonClicked()
    {


             
        SoundFxManager.instance.buttonTapSound.Play();
        OnSlotItemClicked.CanShowSlotInfo = false;
        Instantiate(payTablePopupPrefab, Vector3.zero, Quaternion.identity);
        SetGuiButtonState(false);
        if (Game.currentGameState == GameState.isSpiningComplete)
        {
            LineManager.instance.ShowEabledLine(false);
            Game.currentGameState = GameState.isReady;
        }
    }

    public void OnBuyButtonClicked()
    {
        if (StoreScreen.instance)
            return;

        SoundFxManager.instance.buttonTapSound.Play();
        SlotManager.instance.ResetAll();
        Instantiate(storeScreenPopupPrefab, Vector3.zero, Quaternion.identity);

    }

    public void OnMaxLineButtonClicked()
    {
        SlotManager.instance.ResetAll();
        SoundFxManager.instance.MaxLineButtonSound.Play();
        SlotManager.instance.noOfLinesSelected = 25;
        betAmmountIndex = betAmmountArray.Length - 1;
        SlotManager.instance.betPerLineAmount = betAmmountArray[betAmmountIndex];
        if (!paylineShowing)
        {
            paylineShowing = true;
            LineManager.instance.ShowEabledLine(true);
        }
        else {

            paylineShowing = false;
            LineManager.instance.ShowEabledLine(false);
        }
        
        enabledLineText.text = SlotManager.instance.noOfLinesSelected.ToString();
        CalculateTotalBetAmount();
        ChangeLineInfoToTotalBet();
        Game.currentGameState = GameState.isReady;
        UpdateGUI();
    }

    public void OnLinePlusButtonClicked()
    {
        currenTotalBet = SlotManager.instance.totalBetAmount;

        SlotManager.instance.ResetAll();
        SoundFxManager.instance.buttonTapSound.Play();

        //########################
        if (SlotManager.instance.noOfLinesSelected < 25)
            SlotManager.instance.noOfLinesSelected += 1;
        else
            return;
        //########################

        LineManager.instance.ShowEabledLine(true);
        enabledLineText.text = SlotManager.instance.noOfLinesSelected.ToString();
        CalculateTotalBetAmount();
        ChangeLineInfoToTotalBet();
        Game.currentGameState = GameState.isReady;
    }

    public void OnLineMinusButtonClicked()
    {
       
        currenTotalBet = SlotManager.instance.totalBetAmount;

        SoundFxManager.instance.buttonTapSound.Play();     

        SlotManager.instance.ResetAll();
        LineManager.instance.ShowEabledLine(false);

        //##############
        if (SlotManager.instance.noOfLinesSelected > 1)
            SlotManager.instance.noOfLinesSelected -= 1;
        //else
        //    return;
        //##############

        LineManager.instance.ShowEabledLine(true);
        enabledLineText.text = SlotManager.instance.noOfLinesSelected.ToString();
        CalculateTotalBetAmount();
        ChangeLineInfoToTotalBet();
        Game.currentGameState = GameState.isReady;
    }

    public void OnBetPlusButtonClicked()
    {
        currenTotalBet = SlotManager.instance.totalBetAmount;
        SlotManager.instance.ResetAll();
        SoundFxManager.instance.buttonTapSound.Play();

        if(betAmmountIndex < betAmmountArray.Length-1)
          betAmmountIndex++;

        SlotManager.instance.betPerLineAmount = betAmmountArray[betAmmountIndex];

        betAmountText.text = ""+SlotManager.instance.betPerLineAmount.ToString();
        CalculateTotalBetAmount();
        ChangeLineInfoToTotalBet();
        if (Game.currentGameState == GameState.isSpiningComplete)
        {
            LineManager.instance.ShowEabledLine(false);
            Game.currentGameState = GameState.isReady;
        }
    }

    public void OnBetMinusButtonClicked()
    {
        currenTotalBet = SlotManager.instance.totalBetAmount;

        SlotManager.instance.ResetAll();
        SoundFxManager.instance.buttonTapSound.Play();

        if (betAmmountIndex > 0)
            betAmmountIndex--;

        SlotManager.instance.betPerLineAmount = betAmmountArray[betAmmountIndex];

        betAmountText.text =""+ SlotManager.instance.betPerLineAmount.ToString();
        CalculateTotalBetAmount();
        ChangeLineInfoToTotalBet();
        if (Game.currentGameState == GameState.isSpiningComplete)
        {
            LineManager.instance.ShowEabledLine(false);
            Game.currentGameState = GameState.isReady;
        }
    }





    public void CalculateTotalBetAmount()
    {
        currenTotalBet = SlotManager.instance.totalBetAmount;
        SlotManager.instance.totalBetAmount = (float)(SlotManager.instance.betPerLineAmount * SlotManager.instance.noOfLinesSelected);
      
        ScrollTextScript.Scroll(totalBetAmountText, currenTotalBet, SlotManager.instance.totalBetAmount, 0.1f, 0);
    }

    internal void UpdateWiningAmount()
    {
        winingAmountText.text = ""+(SlotManager.instance.currentSpinWinningAmount).ToString("#,##0");
      //  PlayerAnalytics.Instance.CurrentCredit.text = Game.noOfCoinsLeft.ToString();
    }

    public void UpdateGUI()
    {
        ScrollTextScript.Scroll(totalCoinText, float.Parse(totalCoinText.text.ToString()), Game.noOfCoinsLeft, 1, 0);
        enabledLineText.text = SlotManager.instance.noOfLinesSelected.ToString();
        betAmountText.text = SlotManager.instance.betPerLineAmount.ToString();
        totalBetAmountText.text = SlotManager.instance.totalBetAmount.ToString();
   
    }

    internal void ShowPerLineWinAmount(string winAmount)
    {
        perLineWinText.text = winAmount.ToString();
    }

    internal void DisableAllLineOnSpinStart()
    {
		for (int i = 0; i < SlotManager.instance.noOfLinesSelected; i++)
        {
            LineManager.instance.lineItemScripts[i].EnableLine(false);
        }
    }

    public void SetGuiButtonState(bool enable)
    {
        SpinEnable = enable;
        for (int i = 0; i < AllGuiButton.Length; i++)
        {
            AllGuiButton[i].GetComponent<BoxCollider>().enabled = enable;
        }
       
    }
    public void SetCanvasButtonsState(bool state) {

        for (int i = 0; i < CanvasButtons.Length; i++)
        {
            CanvasButtons[i].interactable = state;
        }
    }

}

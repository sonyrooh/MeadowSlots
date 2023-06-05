using UnityEngine;
using System.Collections;
using UnityEngine.Video;
public class GameEffects : MonoBehaviour
{
    public static GameEffects instance;
    public GameObject coinImagePrefab;
    public GameObject freeSpinPopupPrefeb;
    public GameObject bonusPopupPrefeb;
    public GameObject bigWinPrefeb;  // rank 1
    public GameObject MegaWinPrefab;       // rank 2
    public GameObject SpecialWinPrefab;      // rank 3
    public GameObject SuperWinPrefab;     // rank 4
    public GameObject CleanSweepPrefab;   // rank 5
    public GameObject JackPotPrefab;   // rank 6
    public GameObject AwardedSpinsPrefab;
    [Header("Displays")]
    [Header("VideoPlayers")]

    [Header("Other")]

    public GameObject CoinShower;
    public GameObject FireWorksEmitter;
    public static bool onceBonus = false;
    public bool IsVideoEffect;
    public GameObject WinningsShow;
    public bool WinningsCeleberation =false;
    public bool CanEndCelebration = false;
    public GameObject FreeSpinsInfo;
    GameObject celebClone;
    void Awake()
    {
        IsVideoEffect = false;
        instance = this;



    }

    void Start()
    {
    }
    private void Update()
    {
        if (WinningsCeleberation)
        {
            if (CanEndCelebration)
                if (Input.GetMouseButtonUp(0))
                {
                    WinningsCeleberation = false;
                    CancelInvoke("CelebrationEnds");
                    CelebrationEnds();
                }
        }

    }


  public  void CreateFreeSpinInfo() {
        Instantiate(FreeSpinsInfo, FreeSpinsInfo.transform.position, Quaternion.identity);
        ColumnManager.instance.isStickyWild = false;
        BgManager.instance.SetFreespinBg();

    }

    public void CreateAwardedSpinMessage()
    {
        Instantiate(AwardedSpinsPrefab, AwardedSpinsPrefab.transform.position, Quaternion.identity);
        BgManager.instance.SetFreespinBg();
        ColumnManager.instance.isStickyWild = false;
        GUIManager.instance.TurboMode();

    }
 

   
    void continueAutoSpin()
    {
        SlotManager.CanSpinAgain = true;
        SlotManager.instance.SpinAgain();
    }
 
 
    public void CanEndFunction(bool Confirm) {
        CanEndCelebration = Confirm;
    }
   
 




    public void CelebrationEnds()
    {
     
            CanEndFunction(false);
            WinningsCeleberation = false;
            Invoke("CelebrationEndsLate", 0.5f);
      
    }
    public void BonusEnds(float BonusAmount) {

        SlotManager.instance.IsBonusEnabled = false;
        SlotManager.CanSpinAgain = true;
        GUIManager.instance.SetGuiButtonState(true);
        onceBonus = false;
        if (GUIManager.instance.SpinNumbers > 0 || GameOperations.instance.noOfFreeSpin >0)
        {
            this.Invoke("continueAutoSpin", 1f);


        }

    }

    void CelebrationEndsLate() {
        // SoundFxManager.instance.jackpotPopupSound.Play();
        if(!SlotManager.instance.IsBonusEnabled)
        GUIManager.instance.GenerateFlyingBalance(SlotManager.instance.currentSpinWinningAmount);

        if (!SlotManager.instance.IsFreeSpinsEnabled)
        BgManager.instance.SetNormlBg();

        SlotManager.instance.IsBonusEnabled = false;
    
       onceBonus = false;
       // celebClone = GameObject.FindGameObjectWithTag("Celebration");
       if(celebClone !=null)
        Destroy(celebClone);
        SoundFxManager.instance.WinBigSound.Stop();
        //GameObject coinClone = GameObject.FindGameObjectWithTag("Coins");
        //Destroy(coinClone);
        LineManager.instance.BigBigWins = false;
        if (GUIManager.instance.SpinNumbers > 0 && !SlotManager.instance.IsFreeSpinsEnabled)
        {
            this.Invoke("continueAutoSpin", 1f);


        }
        else if (SlotManager.instance.IsFreeSpinsEnabled)
        {

            GameOperations.instance.NextFreeSpin();
        }
        else {
            SlotManager.CanSpinAgain = true;
            GUIManager.instance.SetGuiButtonState(true);
        }

    }
  

    internal void FreeSpinPopUpEffect()
    {
      
        SoundFxManager.instance.FreeSpinsPopUpSound.Play();
        GUIManager.instance.SetGuiButtonState(false);
        Invoke("ShowfreeSpinsPopup", 0.5f);

       
    }
    void ShowfreeSpinsPopup()
    {
        SoundFxManager.instance.StopBgSoundOnly();
    
        Instantiate(freeSpinPopupPrefeb, freeSpinPopupPrefeb.transform.position, Quaternion.identity);
        SlotManager.instance.ClearFreespinsPreviousSum();
        if(!GUIManager.instance.TurboBool)
        GUIManager.instance.TurboMode();

        SlotManager.CanSpinAgain = false;

    }


   

    internal void ShowBonusPopupEffect()
    {

        if (!onceBonus)
        {
          
            onceBonus = true;
            GUIManager.instance.SetGuiButtonState(false);
          //  ApiManager.instance.PostBonusEventStatus("bonus_saved");
            SoundFxManager.instance.BonusPopUpSound.Play();

            SoundFxManager.instance.StopBgSoundOnly();


            SlotManager.CanSpinAgain = false;
            CanEndFunction(false);
            Invoke("ShowbonusSprites", 0.2f);

       }
    }
    private void ShowbonusSprites() {

        Instantiate(bonusPopupPrefeb, bonusPopupPrefeb.transform.position, Quaternion.identity);
    }
 

 
   


 




    internal void AddToTatalCoin()
    {

    }


    



    // All Celebration Effects

    internal void BigWinShow()          // Rank 1
    {
        Instantiate(FireWorksEmitter, FireWorksEmitter.transform.position, FireWorksEmitter.transform.rotation);
        CanEndFunction(false);
     //   GameObject CoinClone = (GameObject)Instantiate(CoinShower, CoinShower.transform.position, CoinShower.transform.rotation);
        SoundFxManager.instance.WinBigSound.Play();
        WinningsCeleberation = true;
        // CoinShower.SetActive(true);
        GUIManager.instance.SetGuiButtonState(false);
        Invoke("BigWinLate", 2);

      
    
    }
    void BigWinLate() {
        celebClone = (GameObject)Instantiate(bigWinPrefeb, bigWinPrefeb.transform.position, Quaternion.identity);
        Invoke("CelebrationEnds", 7);
    }


    internal void MegaWinShow()    //Rank 2
    {
        Instantiate(FireWorksEmitter, FireWorksEmitter.transform.position, FireWorksEmitter.transform.rotation);
        SoundFxManager.instance.WinBigSound.Play();

        CanEndFunction(false);
      //  GameObject CoinClone = (GameObject)Instantiate(CoinShower, CoinShower.transform.position, CoinShower.transform.rotation);

        WinningsCeleberation = true;

        //  CoinShower.SetActive(true);
        GUIManager.instance.SetGuiButtonState(false);
        Invoke("MegaWinLate", 2);

    }
    void MegaWinLate() {
        celebClone = (GameObject)Instantiate(MegaWinPrefab, MegaWinPrefab.transform.position, Quaternion.identity);
        Invoke("CelebrationEnds", 7);
    }

    internal void SpecialWinShow()         // Rank 3
    {
        Instantiate(FireWorksEmitter, FireWorksEmitter.transform.position, FireWorksEmitter.transform.rotation);
        SoundFxManager.instance.WinBigSound.Play();

        CanEndFunction(false);
     //   GameObject CoinClone = (GameObject)Instantiate(CoinShower, CoinShower.transform.position, CoinShower.transform.rotation);
             
        WinningsCeleberation = true;

        GUIManager.instance.SetGuiButtonState(false);
       

        Invoke("SpecialWinLate", 2);
  
    }
    void SpecialWinLate() {
        celebClone = (GameObject)Instantiate(SpecialWinPrefab, SpecialWinPrefab.transform.position, Quaternion.identity);
        Invoke("CelebrationEnds", 7);

    }


    internal void SuperWinShow()       // Rank 4

    {
        SoundFxManager.instance.WinBigSound.Play();

        Instantiate(FireWorksEmitter, FireWorksEmitter.transform.position, FireWorksEmitter.transform.rotation);

      //  GameObject CoinClone = (GameObject)Instantiate(CoinShower, CoinShower.transform.position, CoinShower.transform.rotation);

        CanEndFunction(false);
        WinningsCeleberation = true;
        SlotManager.CanSpinAgain = false;
        GUIManager.instance.SetGuiButtonState(false);
      
        Invoke("SuperWinLate", 2);

    }
    void SuperWinLate() {
        celebClone = (GameObject)Instantiate(SuperWinPrefab, SuperWinPrefab.transform.position, Quaternion.identity);
        Invoke("CelebrationEnds", 7);

    }
    internal void CleanSweepShow() // Rank 5
    {
        Instantiate(FireWorksEmitter, FireWorksEmitter.transform.position, FireWorksEmitter.transform.rotation);
        SoundFxManager.instance.WinBigSound.Play();

        //  GameObject CoinClone = (GameObject)Instantiate(CoinShower, CoinShower.transform.position, CoinShower.transform.rotation);

        CanEndFunction(false);
        WinningsCeleberation = true;
        GUIManager.instance.SetGuiButtonState(false);
        SlotManager.CanSpinAgain = false;
     
        Invoke("CleanSweepLate", 2);
      //  CoinClone.transform.SetParent(myclone.transform);

    }
    void CleanSweepLate() {
        celebClone = (GameObject)Instantiate(CleanSweepPrefab, CleanSweepPrefab.transform.position, Quaternion.identity);
        Invoke("CelebrationEnds", 7);

    }
    internal void JackPotShow()            // Rank 6
    {
        SoundFxManager.instance.WinBigSound.Play();


        Instantiate(FireWorksEmitter, FireWorksEmitter.transform.position, FireWorksEmitter.transform.rotation);

     //   GameObject CoinClone = (GameObject)Instantiate(CoinShower, CoinShower.transform.position, CoinShower.transform.rotation);

        CanEndFunction(false);
 
        WinningsCeleberation = true;
        GUIManager.instance.SetGuiButtonState(false);

        // CoinClone.transform.SetParent(myclone.transform);
        Invoke("JackpotLate", 2);
    }
    void JackpotLate() {
        celebClone = (GameObject)Instantiate(JackPotPrefab, JackPotPrefab.transform.position, Quaternion.identity);
        Invoke("CelebrationEnds", 7);

    }
}

using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;
using CodeStage.AntiCheat.Detectors;
using AvoEx;
public enum GameState
{
    none,
    isSpining,
    isReady,
    isSpiningComplete
}


public class Game : MonoBehaviour
{
    public static Game Instance;
    public static bool isReadyForAppStore = false;

    public static ObscuredFloat noOfCoinsLeft = 1000;
    public static ObscuredFloat MinBonusWin = 300;

   
    public static GameState currentGameState = GameState.none;

    public TextMeshPro UsernameField;
    public Image userPhoto;
    public bool IsDemoGame;
    public GameObject CheatWarningPanel;

    public GameObject BonusAtStartPanel;

    private void Awake()
    {
        BonusAtStartPanel.SetActive(false);
        Instance = this;
        CheatWarningPanel.SetActive(false);
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
      //KaleemChange  UsernameField.text = GameSocketManager.Instance.auth.Username.ToString();

    }
    private void Start()
    {
        GUIManager.instance.SetGuiButtonState(true);

        OnSlotItemClicked.CanShowSlotInfo = false;
        ObscuredCheatingDetector.StartDetection(OncheaterDetected);


    }


    private void OncheaterDetected()
    {
        GUIManager.instance.SetGuiButtonState(false);
        if (!IsDemoGame)
        {
            CheatWarningPanel.SetActive(true);
          //  ApiManager.instance.PlayerBannedFunc("PlayerBanned");
        }
        Invoke("newdirection", 1.5f);
    }
  public void newdirection()
    {
       
      //  Application.OpenURL(ApiManager.instance.DashboardUrl);
    }


    internal static void Reset()
    {

    }

  

    internal void LoadPlayerData()
    {

     

        GUIManager.instance.SetGuiButtonState(true);

        print("Username is " + GameSocketManager.Instance.auth.Username.ToString());




        GUIManager.instance.UpdateGUI();

        //if (ApiManager.instance.LoadingPage.activeInHierarchy)
        //{
        //    ApiManager.instance.LoadingPage.SetActive(false);
        //}

    }
    void loadPaytableValues() {


        string[] items = new string[] { }; //ApiManager.instance.Mydata.pay_data.Split('/');
  
     
   
       string[] item = items[0].Split(',');
       PayTable.instance.item1 = new Vector2[] { new Vector2(5, int.Parse(item[0])), new Vector2(4, int.Parse(item[1])), new Vector2(3, int.Parse(item[2])) };

        item = items[1].Split(',');
        PayTable.instance.item2 = new Vector2[] { new Vector2(5, int.Parse(item[0])), new Vector2(4, int.Parse(item[1])), new Vector2(3, int.Parse(item[2])) };

        item = items[2].Split(',');
        PayTable.instance.item3 = new Vector2[] { new Vector2(5, int.Parse(item[0])), new Vector2(4, int.Parse(item[1])), new Vector2(3, int.Parse(item[2])) };

        item = items[3].Split(',');
        PayTable.instance.item4 = new Vector2[] { new Vector2(5, int.Parse(item[0])), new Vector2(4, int.Parse(item[1])), new Vector2(3, int.Parse(item[2])) };

        item = items[4].Split(',');
        PayTable.instance.item5 = new Vector2[] { new Vector2(5, int.Parse(item[0])), new Vector2(4, int.Parse(item[1])), new Vector2(3, int.Parse(item[2])) };

        item = items[5].Split(',');
        PayTable.instance.item6 = new Vector2[] { new Vector2(5, int.Parse(item[0])), new Vector2(4, int.Parse(item[1])), new Vector2(3, int.Parse(item[2])) };

        item = items[6].Split(',');
        PayTable.instance.item7 = new Vector2[] { new Vector2(5, int.Parse(item[0])), new Vector2(4, int.Parse(item[1])), new Vector2(3, int.Parse(item[2])) };

        item = items[7].Split(',');
        PayTable.instance.item8 = new Vector2[] { new Vector2(5, int.Parse(item[0])), new Vector2(4, int.Parse(item[1])), new Vector2(3, int.Parse(item[2])) };

        item = items[8].Split(',');
        PayTable.instance.item9 = new Vector2[] { new Vector2(5, int.Parse(item[0])), new Vector2(4, int.Parse(item[1])), new Vector2(3, int.Parse(item[2])) };

        item = items[9].Split(',');
        PayTable.instance.WildInSequanceAmount = new Vector2[] { new Vector2(5, int.Parse(item[0])), new Vector2(4, int.Parse(item[1])), new Vector2(3, int.Parse(item[2])) };

        item = items[10].Split(',');
        PayTable.instance.autoSpinActive = new Vector2[] { new Vector2(5, int.Parse(item[0])), new Vector2(4, int.Parse(item[1])), new Vector2(3, int.Parse(item[2])) };




    }
    void loadProbValues() {


        string[] items_probs = new string[] { }; // ApiManager.instance.Mydata.prob_data.Split(',');

        for (int i = 0; i < items_probs.Length; i++) {

            ProbabilityManager.instance.ItemsProbabilities[i] = int.Parse(items_probs[i]);
        }

        ProbabilityManager.instance.getProbSum();

      

      }
    internal void LoadPlayerPic(Texture2D Pic)
    { 
        userPhoto.sprite = Sprite.Create(Pic, new Rect(x: 0f, y: 0f, Pic.width, Pic.height), new Vector2(0, 0));

    }

    public void PlayBonusAtStart() {

        SlotManager.instance.betPerLineAmount = 10;
        SlotManager.instance.noOfLinesSelected = 20;
        GUIManager.instance.CalculateTotalBetAmount();
        GUIManager.instance.UpdateGUI();
        SlotManager.instance.MybonusReward();






    }
}

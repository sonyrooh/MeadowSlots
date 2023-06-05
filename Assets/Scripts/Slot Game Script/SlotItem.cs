using UnityEngine;
using System.Collections;

public enum SlotItemType
{
    Wild,
    Scatter,
    Normal,
    Bonus
}

public class SlotItem : MonoBehaviour
{
    public AudioClip[] audios;
    
    public Texture2D[] slotItemTextures;
    public Texture2D[] slotItemBlurryTextures;
    private ColumnScript myColumnScript;
    public int indexInColumn;
    public int animationIndex = 0;
    private Transform myTransform;
    internal bool spin = false;
    public SlotItemType itemType = SlotItemType.Normal;
    internal bool luckyItem = false;
    internal float luckChance;
    public static float counter;
    public SlotItemEffects myEffectScript;
    private bool once;
    private int previouscolumn;
    private Vector3 InitialScale;
    public ColumnScript ParentColumn;
    public int MyColumnIndex;
    public GameObject PayBoxPrefab;
    public float randomNo;
    public int[] demoProbs = new int[] {10,20,30,38,46,54,60,65,75,90,95,100};

    void Start()
    {
        ParentColumn = GetComponentInParent<ColumnScript>();
        MyColumnIndex = ParentColumn.columnIndex;
        InitialScale = transform.localScale;
        once = false;
        myTransform = transform;
        myColumnScript = transform.parent.GetComponent<ColumnScript>();
        SetNewAnim();
   
    }

    void Update()
    {
        if (!spin)
            return;

        // Itween Code To Reset Slot Item Position..
        if (myColumnScript.isSpinning == false)
        {


#if UNITY_EDITOR

            SetItemOnProb();
#else
              if (Game.Instance.IsDemoGame)
                SetItemOnProb_ForDemo();
            else
                 SetItemOnProb();
#endif






            if (itemType == SlotItemType.Wild && !ParentColumn.IsColumnWild && indexInColumn == 0)
            {

                ParentColumn.IsColumnWild = true;
          

                ShowWildExpnd();

            }
            else

              CheckForWildColumn();


          





            iTween.ShakeScale(gameObject, new Vector3(0.2f, 0.2f, 1f), 0.3f);

            iTween.Defaults.easeType = iTween.EaseType.elastic;
            iTween.MoveTo(gameObject, new Vector3(transform.position.x, ColumnManager.instance.MyGap + indexInColumn * (-SlotManager.instance.verticalGap), transform.position.z), 0.5f);
            spin = false;
           
           
            if (itemType == SlotItemType.Bonus && !once && indexInColumn < 3)
            {
                once = true;
              
              
                iTween.ScaleFrom(gameObject, new Vector3(2f,2,1), 2f);
              


            }
            
            if (itemType == SlotItemType.Bonus && indexInColumn < 3) {

                if (ParentColumn.columnIndex < 4 && GameOperations.instance.BonusMatched > 1)
                {
                    ColumnManager.instance.LongSpin = true;
                }
                if (ParentColumn.columnIndex < 3 || GameOperations.instance.BonusMatched >= 3)
                {
                      if (!SoundFxManager.instance.BonusItemAppear.isPlaying)
                    SoundFxManager.instance.BonusItemAppear.Play();
                 
                }
            }
          
          
            //}
            return;
        }
       

        // Move Item Down-ward..
        myTransform.position -= new Vector3(0, SlotManager.instance.spinSpeed * Time.deltaTime, 0);
        Vector3 newPos = transform.position;
        newPos.z = 1;
        transform.position = newPos;
        // Move Last Item To First Position..
        if (myTransform.position.y < -3f)
        {
            myColumnScript.MoveLastItem();
            myTransform.position = new Vector3(myTransform.position.x, 3f, myTransform.position.z);
            SetNewAnim();


        }

    }// End Update();


    void CheckForWildColumn() {
        if (ParentColumn.IsColumnWild)
        {
            itemType = SlotItemType.Wild;
            animationIndex = 9;
            GetComponent<Renderer>().material.mainTexture = slotItemTextures[animationIndex];
           
        }
    }
    public void ShowWildExpnd() {

        GameObject WildClone = (GameObject)Instantiate(ParentColumn.WildExpandedPrefab, ParentColumn.WildExpandedPrefab.transform.position, Quaternion.identity);
        ColumnManager.instance.wildclones.Add(WildClone);
        if (!SoundFxManager.instance.WildExpandedSound.isPlaying) {
            SoundFxManager.instance.WildExpandedSound.Play();
        }
    }
    /// Set Random Animation On Slot Item..
    public  static bool onceCheck = false;

    // Zero Win settings in Slots Matching

    internal void SetItemOnProb()
    {
        randomNo = Random.Range(0, 1000);
        if (randomNo <= ProbabilityManager.instance.ItemsWeightsRange[0])
        {
            animationIndex = 0;
            itemType = SlotItemType.Normal;
            GetComponent<Renderer>().material.mainTexture = slotItemTextures[animationIndex];
            goto checking;
        }
        for (int i = 1; i < 12; i++)
        {


            if (randomNo > ProbabilityManager.instance.ItemsWeightsRange[i - 1] && randomNo <= ProbabilityManager.instance.ItemsWeightsRange[i])
            {
                animationIndex = i;
                if (animationIndex < 9)
                    itemType = SlotItemType.Normal;
                else if (animationIndex == 9)
                    itemType = SlotItemType.Wild;

                else if (animationIndex == 10)
                    itemType = SlotItemType.Scatter;
                else
                    itemType = SlotItemType.Bonus;


                if (itemType == SlotItemType.Bonus && indexInColumn < 3)
                {

                    GameOperations.instance.BonusMatched += 1;
                }else
                    if (itemType == SlotItemType.Scatter && indexInColumn < 3)
                {

                    GameOperations.instance.scattersMatched += 1;

                }

                if (itemType == SlotItemType.Scatter && GameOperations.instance.BonusMatched >= 3 && indexInColumn<3)
                {

                    itemType = SlotItemType.Normal;
                   
                         animationIndex = 8;
                  


                }
                else
                              if (itemType == SlotItemType.Bonus && GameOperations.instance.scattersMatched >= 3 && indexInColumn <3)
                {

                    itemType = SlotItemType.Normal;


                    animationIndex = 7;
                    


                }

                break;
            }
        }
        if (animationIndex == 9 && ParentColumn.columnIndex == 0)
        {
            animationIndex = Random.Range(0, 9);
            itemType = SlotItemType.Normal;

        }
        else
            if (SlotManager.instance.IsFreeSpinsEnabled && animationIndex ==11) {
            animationIndex = Random.Range(0, 9);
            itemType = SlotItemType.Normal;
        }

        if (ParentColumn.columnIndex == 0)
        {
            if(indexInColumn <3)
            ParentColumn.column0indexes[indexInColumn] = animationIndex;

        }
        checking:
        if (SlotManager.instance.nakha_shta && !Game.Instance.IsDemoGame)
        {
            if (SlotManager.instance.game_nakha > SlotManager.instance.pakar_nakha)
            {
                if (Random.value > 0.1f)
                    gata_na();
                

            }
        }
        GetComponent<Renderer>().material.mainTexture = slotItemTextures[animationIndex];


    }

    private void gata_na() {
        if (ParentColumn.columnIndex == 1) {
            while (ColumnManager.instance.columnScripts[0].column0indexes.Contains(animationIndex) || animationIndex == 9)
            {
                animationIndex = Random.Range(0, 9);
                itemType = SlotItemType.Normal;
            }
        }


    }
    internal void SetItemOnProb_ForDemo()
    {
        randomNo = Random.Range(0, 100);
        if (randomNo <= demoProbs[0])
        {
            animationIndex = 0;
            itemType = SlotItemType.Normal;
            GetComponent<Renderer>().material.mainTexture = slotItemTextures[animationIndex];

            return;
        }
        for (int i = 1; i < 12; i++)
        {


            if (randomNo > demoProbs[i - 1] && randomNo <= demoProbs[i])
            {
                animationIndex = i;
                if (animationIndex < 9)
                    itemType = SlotItemType.Normal;
                else if (animationIndex == 9)
                    itemType = SlotItemType.Wild;

                else if (animationIndex == 10)
                    itemType = SlotItemType.Scatter;
                else
                    itemType = SlotItemType.Bonus;


                if (itemType == SlotItemType.Bonus && indexInColumn < 3)
                {

                    GameOperations.instance.BonusMatched += 1;
                }
                else
                   if (itemType == SlotItemType.Scatter && indexInColumn < 3)
                {

                    GameOperations.instance.scattersMatched += 1;

                }

                if (itemType == SlotItemType.Scatter && GameOperations.instance.BonusMatched >= 3 && indexInColumn < 3)
                {

                    itemType = SlotItemType.Normal;

                    animationIndex = 8;



                }
                else
                              if (itemType == SlotItemType.Bonus && GameOperations.instance.scattersMatched >= 3 && indexInColumn < 3)
                {

                    itemType = SlotItemType.Normal;


                    animationIndex = 7;



                }

                break;
            }
        }
        if (animationIndex == 9 && ParentColumn.columnIndex == 0)
        {
            animationIndex = Random.Range(0, 9);
            itemType = SlotItemType.Normal;

        }
        else
            if (SlotManager.instance.IsFreeSpinsEnabled && animationIndex == 11)
        {
            animationIndex = Random.Range(0, 9);
            itemType = SlotItemType.Normal;
        }
        GetComponent<Renderer>().material.mainTexture = slotItemTextures[animationIndex];


    }
    internal void SetNewAnim()
    {
        Vector3 newscale = InitialScale;
        gameObject.transform.localScale = newscale;
     

            animationIndex = Random.Range(0, 9);

        if (animationIndex ==9 && transform.parent.transform.name == "Column 1") {
            animationIndex = Random.Range(0, 9);

        }


        if (animationIndex < 9)
            itemType = SlotItemType.Normal;
        else if (animationIndex == 9)
            itemType = SlotItemType.Wild;

        else if (animationIndex == 10)
            itemType = SlotItemType.Scatter;
        else
            itemType = SlotItemType.Bonus;

        if (ParentColumn.IsColumnWild) {

            itemType = SlotItemType.Wild;
            animationIndex = 9;
        }

    if(!spin)
        GetComponent<Renderer>().material.mainTexture = slotItemTextures[animationIndex];
    else
            GetComponent<Renderer>().material.mainTexture = slotItemBlurryTextures[animationIndex];
        once = false;
    }
}// End Script..

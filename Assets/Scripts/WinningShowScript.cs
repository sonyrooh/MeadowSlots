using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinningShowScript : MonoBehaviour
{
    public iTween.EaseType easeType;
    public GameObject multiPlyerGo;
    public GameObject blackFade;
    private GameObject fadeclone;
    public float ShowNormal = 5f;
    public float ShowTurbo = 3f;
    // Use this for initialization
    void Start()
    {

       // fadeclone = (GameObject)Instantiate(blackFade, blackFade.transform.position, Quaternion.identity);
         multiPlyerGo.SetActive(false);
      //  transform.position = new Vector3(49.17f, 0.11f, -96);
        SlotManager.instance.WinShowPanel = gameObject;
     //   SlotManager.instance.FadePanel = fadeclone;
        Invoke("ShowText", 0.1f);

    }

    void ShowText()
    {
        multiPlyerGo.SetActive(true);

        iTween.Defaults.easeType = easeType;
        if (!GUIManager.instance.TurboBool)
        {
            iTween.ScaleFrom(gameObject, Vector3.zero, 1f);
            Invoke("HideText", ShowNormal);
        }
        else
        {
            iTween.ScaleFrom(gameObject, Vector3.zero, 0.5f);
            Invoke("HideText", ShowTurbo);
        }

    }

    void HideText()
    {
        iTween.Defaults.easeType = easeType;
        iTween.ScaleTo(gameObject, Vector3.zero, 0.5f);
      //  Destroy(fadeclone, 0.1f);

        Destroy(gameObject, 0.6f);
    }
}

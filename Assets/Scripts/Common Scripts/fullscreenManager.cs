using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class fullscreenManager : MonoBehaviour
{
    public static fullscreenManager instance;
    private void Start()
    {
        instance = this;
    }
    public GameObject fullscreenbutton;
    public Sprite fullscreenSp, windowSp;

   
    public void fullscreenOnOff() {

        ResolutionSelector.instance.FullScreenToggle();
        print("fullscreen is " + Screen.fullScreen);
    }

    public void changeImage() {

        fullscreenbutton.GetComponent<Image>().sprite = (Screen.fullScreen ? windowSp : fullscreenSp);
    }
}

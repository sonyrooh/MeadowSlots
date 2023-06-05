using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class MyWinningScroll : MonoBehaviour
{
    float scrollStep = 0;
    float tempScore;
    TextMeshPro scoreText;
    float _initialScore;
    float _difference;
    private AudioSource clickSound;
    public AudioClip incrementSound, FinishedSound;
    public static MyWinningScroll inst_myscroll;

    private void Start()
    {
        clickSound = GetComponent<AudioSource>();
        inst_myscroll = this;
    }
    public void ScrollTo(TextMeshPro text, float initialScore, float finalScore, float duration, float initialDelay)
    {
        tempScore = 0;
        float frequency = .08f;

        _initialScore = initialScore;
        scoreText = text;
        float functionCallPerSecond = 1.0f / frequency;
        float difference = finalScore - initialScore;
        scrollStep = difference / (functionCallPerSecond * duration);
        _difference = difference;
        clickSound.clip = incrementSound;
        InvokeRepeating("_ScrollText", initialDelay, frequency);
    }



    void _ScrollText()
    {
        if (Input.GetMouseButtonUp(0))
        {
            tempScore = _difference;
        }
        if (!clickSound.isPlaying)
            clickSound.Play();
        tempScore += scrollStep;

        if (_difference > 0)
        {

            if (tempScore >= _difference)
            {
                clickSound.clip = FinishedSound;
                clickSound.Play();

                tempScore = _difference;
                CancelInvoke("_ScrollText");
                if(scoreText !=null)
                iTween.PunchScale(scoreText.gameObject, new Vector3(.2f, .2f, .2f), 1f);
                if (!SlotManager.instance.IsFreeSpinsEnabled && !SlotManager.instance.IsBonusEnabled) {
                    GUIManager.instance.SetGuiButtonState(true);
                    GUIManager.instance.StopSpinButton.SetActive(false);
                }
                //  Destroy(GetComponent<ScrollTextScript>());
            }
        }
        else
        {
            if (tempScore <= _difference)
            {
                
                clickSound.clip = FinishedSound;
                clickSound.Play();
                tempScore = _difference;
                CancelInvoke("_ScrollText");
                if (scoreText != null)
                    iTween.PunchScale(scoreText.gameObject, new Vector3(.2f, .2f, .2f), 1f);
                if (!SlotManager.instance.IsFreeSpinsEnabled && !SlotManager.instance.IsBonusEnabled)
                {
                    GUIManager.instance.SetGuiButtonState(true);
                    GUIManager.instance.StopSpinButton.SetActive(false);

                }
                //  Destroy(GetComponent<ScrollTextScript>());
            }
        }
        if (scoreText != null)
            scoreText.text = "" + ((float)(tempScore + _initialScore)).ToString("#,##0");
    }

  
}

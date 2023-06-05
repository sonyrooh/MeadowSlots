using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Bigwinscroll : MonoBehaviour
{
    float scrollStep = 0;
    float tempScore;
    TextMeshPro scoreText;
    float _initialScore;
    float _difference;
   private AudioSource clickSound;
    public AudioClip incrementSound, FinishedSound;
    public static Bigwinscroll instant_Bscroll;

    private void Start()
    {
        clickSound = GetComponent<AudioSource>();
        instant_Bscroll = this;
    }

    public void ScrollTo(TextMeshPro text, float initialScore, float finalScore, float duration, float initialDelay)
    {
        tempScore = 0;
        float frequency = .08f;
        GUIManager.instance.SetGuiButtonState(false);

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
        if (Input.GetMouseButtonUp(0) && tempScore >5)
        {
            tempScore = _difference;
        }
        tempScore += scrollStep;

        if (!clickSound.isPlaying)
            clickSound.Play();

        if (_difference > 0)
        {
            if (tempScore >= _difference)
            {
                clickSound.clip = FinishedSound;
                clickSound.Play();
                tempScore = _difference;
                CancelInvoke("_ScrollText");
                iTween.PunchScale(scoreText.gameObject, new Vector3(0.03f,0.03f,1f), 2f);
                Invoke("CanEndInvoke", 0.5f);
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
                iTween.PunchScale(scoreText.gameObject, new Vector3(0.03f, 0.03f, 1f), 2f);
                Invoke("CanEndInvoke", 0.5f);


            }
        }

        scoreText.text = "" + ((float)(tempScore + _initialScore)).ToString("#,##0");
    }

    void CanEndInvoke() {
        GameEffects.instance.CanEndFunction(true);

    }

}

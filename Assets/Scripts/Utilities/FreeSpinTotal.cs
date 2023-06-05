using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class FreeSpinTotal : MonoBehaviour
{
    float scrollStep = 0;
    float tempScore;
    TextMeshPro scoreText;
    float _initialScore;
    float _difference;
    private AudioSource clickSound;
    public AudioClip incrementSound, FinishedSound;
    public static FreeSpinTotal inst_Freescroll;

    private void Start()
    {
        clickSound = GetComponent<AudioSource>();
        inst_Freescroll = this;
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

    void FinishFreeSpins() {

        GameOperations.instance.FreeSpinsCompleted();
        Destroy(GameObject.FindGameObjectWithTag("FreeSpinsTotal"));
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
                iTween.PunchScale(scoreText.gameObject, new Vector3(.2f, .2f, .2f), 1f);
                if(SlotManager.instance.IsFreeSpinsEnabled)
                Invoke("FinishFreeSpins",3f);
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
                iTween.PunchScale(scoreText.gameObject, new Vector3(.2f, .2f, .2f), 1f);
                if (SlotManager.instance.IsFreeSpinsEnabled)
            Invoke("FinishFreeSpins", 1f);

                //  Destroy(GetComponent<ScrollTextScript>());
            }
        }

        scoreText.text = "" + ((float)(tempScore + _initialScore)).ToString("#,##0");
    }
}

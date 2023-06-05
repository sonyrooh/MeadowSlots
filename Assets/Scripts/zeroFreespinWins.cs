using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class zeroFreespinWins : MonoBehaviour
{
    public TextMeshPro Greetings;
    public GameObject youwonTextmesh;
    public GameObject CoinsParticles;
    // Start is called before the first frame update
    void Start()
    {
        if (SlotManager.instance.FreeSpinsWinSum == 0)
        {
            youwonTextmesh.SetActive(false);
            Greetings.text = "Better Luck Next Time";
            CoinsParticles.SetActive(false);
            Destroy(gameObject, 3f);

        }
        else {
            youwonTextmesh.SetActive(true);
            Greetings.text = "Congratulations";
            youwonTextmesh.GetComponent<TextMeshPro>().text = "you won";
            CoinsParticles.SetActive(true);
        }
    }

}

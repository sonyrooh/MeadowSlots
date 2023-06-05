using UnityEngine;
using System.Collections;
//using ChartboostSDK;

public class MyButtonScript : MonoBehaviour
{
    public MonoBehaviour scriptToCall;
    public string methodToInvoke;

    private Vector3 InitialScale;
    public GameObject spinParticle;
    public Texture2D blurryImage, ClearImage;
    private void Start()
    {
        InitialScale = transform.localScale;
    
    }
    private void OnMouseDown()
    {
     
        if (transform.name == "Spin Button")
        {
          
            GetComponent<AudioSource>().Play();

        }
        else {
            transform.localScale = new Vector3(InitialScale.x + (InitialScale.x / 8), InitialScale.y + (InitialScale.y / 8), 1f);

        }

    }

    public void blurTheImage() {
        GetComponent<Renderer>().material.mainTexture = blurryImage;
    }
    public void ClearTheImage()
    {
        GetComponent<Renderer>().material.mainTexture = ClearImage;
    }

    void OnMouseUp()
    {
        transform.localScale = InitialScale;
     
        if (scriptToCall != null)
            scriptToCall.Invoke(methodToInvoke, .1f);
    }
}

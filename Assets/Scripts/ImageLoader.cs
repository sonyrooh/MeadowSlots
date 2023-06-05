using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.IO;
public class ImageLoader : MonoBehaviour
{
    // Start is called before the first frame update

    //public string[] filenames;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator LoadImage(string imageUrl, Image img, string fileName) {

        UnityWebRequest request = UnityWebRequestTexture.GetTexture(imageUrl);
        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.LogError("Error while loading image");
        }
        else {
            Texture2D myTexture = ((DownloadHandlerTexture)request.downloadHandler).texture;

            Sprite imageSprite = Sprite.Create(myTexture, new Rect(0,0,myTexture.width,myTexture.height),new Vector2(0.5f,0.5f));

            img.sprite = imageSprite;

            byte[] textureBytes = myTexture.EncodeToPNG();
            File.WriteAllBytes(Application.persistentDataPath + fileName, textureBytes);
            Debug.Log("Icon Saved On Disk!");

        }
        StopCoroutine(LoadImage(imageUrl,img,fileName));
    
    }

    public void LoadIconsFromDisk(string fileName, Image img) {

        byte[] textureBytes = File.ReadAllBytes(Application.persistentDataPath + fileName);
        Texture2D myTexture = new Texture2D(0, 0);

        myTexture.LoadImage(textureBytes);
        Sprite imageSprite = Sprite.Create(myTexture, new Rect(0, 0, myTexture.width, myTexture.height), new Vector2(0.5f, 0.5f));
        img.sprite = imageSprite;

        Debug.Log("Image successfully loaded from disk");

    }
}

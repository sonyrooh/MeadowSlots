using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnChoosePackageClicked : MonoBehaviour
{

    public int var1, var2;
    public bool isRandom;
    public bool IsDestory = false;
    private void Start()
    {

    }
    private void OnMouseDown()
    {


    }



    void OnMouseUp()
    {

      
        Invoke("FunctionCaller", 0.1f);
    }

    void FunctionCaller() {

        GameOperations.instance.ContinueToFreeSpins();

        Destroy(gameObject.transform.parent.gameObject, 0.01f);
    }
}

using UnityEngine;
using System.Collections;

public class ResolutionSelector : MonoBehaviour {
    public static ResolutionSelector instance;
    private void Start()
    {
        instance = this;
        DontDestroyOnLoad(this);

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) {
            FullScreenToggle();
        }
    }

    public void FullScreenToggle() {
        if (ResolutionManager.Instance == null) return;
        ResolutionManager resolutionManager = ResolutionManager.Instance;
        resolutionManager.ToggleFullscreen();

        fullscreenManager.instance.changeImage(); // 
    }

   
}

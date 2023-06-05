using UnityEngine;
using UnityEditor;
using System.IO;

public class CreateAssetBundles
{
    [MenuItem("Assets/Build AssetBundles for UnityEditor")]
    static void BuildAllAssetBundles()
    {
        string assetBundleDirectory = "Assets/StreamingAssets/UnityEditor";
        if (!Directory.Exists(Application.streamingAssetsPath))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }
        BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
        Debug.Log("Bundle for UnityEditor Created Successfully");
    }

    [MenuItem("Assets/Build AssetBundles for Android")]
    static void BuildAllAssetBundlesforAndroid()
    {
        string assetBundleDirectory = "Assets/StreamingAssets/Android";
        if (!Directory.Exists(Application.streamingAssetsPath))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }
        BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None, BuildTarget.Android);
        Debug.Log("Bundle for Android Created Successfully");

    }
    [MenuItem("Assets/Build AssetBundles for iOS")]
    static void BuildAllAssetBundlesIos()
    {
        string assetBundleDirectory = "Assets/StreamingAssets/iOS";
        if (!Directory.Exists(Application.streamingAssetsPath))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }
        BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None, BuildTarget.iOS);
        Debug.Log("Bundle for Android Created Successfully");

    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene1 : UIManager
{
    private AssetBundle assetBundle;
    private string[] scenePaths;

    private void Start()
    {
        assetBundle = AssetBundle.LoadFromFile("Assets/Scenes/Level1");
        scenePaths = assetBundle.GetAllScenePaths();
    }

    private void OnGUI()
    {
        SceneManager.LoadScene(scenePaths[0], LoadSceneMode.Single);
    }
}

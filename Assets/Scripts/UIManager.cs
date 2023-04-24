using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject Scene;
    [SerializeField] private GameObject panelStartGame;
    private int sceneIndex;
    private AssetBundle assetBundle;
    private string[] scenePaths;

    private void Start()
    {
        sceneIndex = 0;
        
    }
    public void StartGame( )
    {
        SceneManager.LoadScene(sceneIndex, LoadSceneMode.Single);
    }

    public void Result()
    {

    }

    public void LoadScene(int SIndex)
    {
        panelStartGame.SetActive(false);
        //SceneManager.LoadScene(SIndex, LoadSceneMode.Single);
    }
      
}

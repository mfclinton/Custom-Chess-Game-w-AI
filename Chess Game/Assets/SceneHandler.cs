using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour {

    static int currentScene = 0;
    static int levelcount;
    static SceneHandler Instance = null;
    public void NextScene()
    {
        currentScene++;
        if(currentScene >= levelcount)
        {
            currentScene = 0;
        }
        SceneManager.LoadScene(currentScene);
    }

    void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;
        DontDestroyOnLoad(transform.gameObject);
        levelcount = SceneManager.sceneCountInBuildSettings;
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHandler : MonoBehaviour {

	public void NextScene()
    {
        Debug.Log("SCENE");
        SceneHandler scenehandle = FindObjectOfType<SceneHandler>();
        scenehandle.NextScene();
    }
}

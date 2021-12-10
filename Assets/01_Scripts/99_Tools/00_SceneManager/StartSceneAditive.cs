using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class StartSceneAditive : MonoBehaviour
{
    public ScenesIndex AditiveToOpen;

    private void Awake()
    {
        if(!SceneManager.GetSceneByBuildIndex((int)AditiveToOpen).isLoaded)  
            SceneManager.LoadScene((int)AditiveToOpen, LoadSceneMode.Additive);
    }

	private void Start()
	{
        Application.targetFrameRate = 60;
    }
}

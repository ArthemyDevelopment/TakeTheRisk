using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ChangeScene : MonoBehaviour
{
  
    SceneController SC;

    public ScenesIndex Scene;

    private void Start()
    {
        SC = SceneController.current;
    }

    public void ChangeSceneGoTo()
    {
        SC.GoToScene(Scene);
    }



}

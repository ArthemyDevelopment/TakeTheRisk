using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController current = null;

    public int I_GoToScene;
    
    public string S_PrevSceneName;

    public GameObject G_LoadingObject;

    public ProgressBar Bar;

    public Animator An_LoadingAnimator;
    

    private void Awake()
    {
        if(current == null)
        {
            current = this;
        }
        else if(current != this)
        {
            Destroy(gameObject);
        }
        G_LoadingObject.SetActive(false);
    }
    
    public void GoToScene(ScenesIndex Scene)
    {
        
        S_PrevSceneName = SceneManager.GetActiveScene().name;
        I_GoToScene = (int)Scene;
        G_LoadingObject.SetActive(true);
        An_LoadingAnimator.Play("StartTransition");

    }

    public void UnLoadPrevScene()
    {
        if (S_PrevSceneName != "")
        {
            SceneManager.UnloadSceneAsync(S_PrevSceneName);

        }
    }

    List<AsyncOperation> SceneLoading = new List<AsyncOperation>();

    public void LoadNextScene()
    {
        SceneLoading.Add(SceneManager.LoadSceneAsync(I_GoToScene, LoadSceneMode.Additive));
        StartCoroutine(LoadingStatus());

    }

    float totalSceneProgress;
    public IEnumerator LoadingStatus()
    {
        for (int i = 0; i < SceneLoading.Count; i++)
        {
            while(!SceneLoading[i].isDone)
            {
                totalSceneProgress = 0;

				foreach (AsyncOperation  operation in SceneLoading)
				{
                    totalSceneProgress += operation.progress;
				}

                totalSceneProgress = (totalSceneProgress / SceneLoading.Count) * 100f;

                Bar.I_Current = Mathf.RoundToInt(totalSceneProgress);

                yield return null;
            }
        }

        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(I_GoToScene));
        An_LoadingAnimator.Play("EndTransition");
        

    }

    public void RemoveTransitionScene()
    {
        G_LoadingObject.SetActive(false);

    }



}

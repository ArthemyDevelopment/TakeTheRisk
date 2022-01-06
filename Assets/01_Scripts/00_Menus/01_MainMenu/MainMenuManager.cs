using UnityEngine;
using ArthemyDevelopment.Save;
using UnityEngine.Events;

public class MainMenuManager : MonoBehaviour
{
    private OptionMenuManager OMM;

    public GameObject G_ContinueButton;
    
    public GameObject G_Main;
    public GameObject G_Credits;
    public GameObject G_ContinueMenu;

    public UnityEvent Ev_StartGame;



    // Start is called before the first frame update
    void Start()
    {
        
        OMM = OptionMenuManager.current;
        Debug.Log(SaveData.SaveFileExist);
        G_ContinueButton.SetActive(SaveData.SaveFileExist);
        GoMenu();

    }

    public void GoCredits()
    {
        DeactiveObject();
        G_Credits.SetActive(true);
    }

    public void GoOptions()
    {
        DeactiveObject();
        OMM.OpenMenu();
    }

    public void GoContinue()
    {
        DeactiveObject();
        G_ContinueMenu.SetActive(true);
    }

    public void GoGame()
    {
        Ev_StartGame.Invoke();
    }

    public void GoMenu()
    {
        DeactiveObject();
        G_Main.SetActive(true);
    }

    public void ExitGame()
    {
        
    }

    public void DeactiveObject()
    {
        G_Main.SetActive(false);
        G_Credits.SetActive(false);
        G_ContinueMenu.SetActive(false);
    }


}

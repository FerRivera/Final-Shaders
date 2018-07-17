using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum Scenes
{
    Menu,
    Options,
    Level1,
    PreGame,
    Continue,
}

public class ScreenManager : MonoBehaviour
{
    public Image emptyBar;
    public Image loadingBar;
    public GameObject firstCanvas;
    public GameObject secondCanvas;
    public GameObject optionsCanvas;
    AsyncOperation ao;
    public Text textIndicator;
    public Scenes sceneToLoad;

    public void GoToGame()
    {      
        firstCanvas.SetActive(false);
        secondCanvas.SetActive(true);
        StartCoroutine(LoadLevelWithRealProgress());
    }

    public void GoToOptions()
    {
        // SceneManager.LoadScene((int)Scenes.Options);
        firstCanvas.SetActive(false);
        secondCanvas.SetActive(false);
        optionsCanvas.SetActive(true);
    }
    public void GoToPreGame()
    {
        firstCanvas.SetActive(false);
        //secondCanvas.SetActive(true);
        EventsManager.TriggerEvent(EventsType.changeScene);
        SceneManager.LoadScene("PreGame");
    }
 
    void Start()
    {       
        loadingBar.fillAmount = 0f;
    }

    public void GoToExit()
    {
        Application.Quit();
    }

    IEnumerator LoadLevelWithRealProgress()
    {
        yield return new WaitForSeconds(1);

        ao = SceneManager.LoadSceneAsync((int)sceneToLoad);

        ao.allowSceneActivation = false;

        while (!ao.isDone)
        {

            loadingBar.fillAmount = ao.progress;

            if (ao.progress == 0.9f)
            {
                loadingBar.fillAmount = 1f;

                ao.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}

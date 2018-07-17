using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ScreenDeathManager : MonoBehaviour
{
    public void GoToRestart()
    {

        TextManager.instance.Reset();
   		PickeablesNamesCanvas.instance.Reset();
        pauseGame.Reset();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        EventsManager.TriggerEvent(EventsType.changeScene);
    }



    public void GoToMenu()
    {
        SceneManager.LoadScene("Menu");
        EventsManager.UnsubscribeToEvent(EventsType.spawnNamesText, PickeablesNamesCanvas.instance.spawnNamesText);
        EventsManager.TriggerEvent(EventsType.changeScene);


    }
    public void GoToQuit()
    {

        Application.Quit();
    }
}

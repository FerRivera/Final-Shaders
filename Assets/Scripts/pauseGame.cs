using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class pauseGame : MonoBehaviour
{
    public GameObject pausemenu;
    public static bool paused;
    public static Stack<GameObject> screens = new Stack<GameObject>();
    public GameObject options;
    public GameObject help;
    public GameObject help2;
    public static pauseGame pauseGameInstance;
    [HideInInspector]
    public bool deny;

    void Awake()
    {
        pauseGameInstance = this;
    }
    void Update ()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            //canvasToDissapear.SetActive(true);

            if (!pausemenu.activeSelf && screens.Count == 0)
            {
                pausemenu.SetActive(true);
                deny = true;    
                paused = true;
                Time.timeScale = 0;
            }
            else if(screens.Count >= 1)
            {
                if (screens.Peek().name == "Inventory Panel" || screens.Peek().name == "Shop" || screens.Peek().name == "Inventory")
                {
                    return;
                }
                screens.Pop().SetActive(false);
                
            }
            else
            {

                pausemenu.SetActive(false);              
                //Hero.inputBlock = false;
                paused = false;
                deny = false;
                Time.timeScale = 1;
            }
        }
    }
    public void Resume()
    {
        pausemenu.SetActive(false);        
        paused = false;
        Time.timeScale = 1;
        deny = false;
    }
    public void Pause()
    {
        //paused = true;
        deny = true;
        Time.timeScale = 0;
    }
    public void UnPause()
    {
        deny = false;
        paused = false;
        Time.timeScale = 1;
    }
    public static void Reset()
    {
        paused = false;
        screens.Clear();
    }
    public void Options()
    {
        //pausemenu.SetActive(false);
        options.SetActive(true);
        screens.Push(options.gameObject);
    }
    public void Help()
    {
        help.SetActive(true);
        screens.Push(help.gameObject);
    }
    public void NextHelp()
    {
        help2.SetActive(true);
        screens.Pop().SetActive(false);
        screens.Push(help2.gameObject);

    }
    public void BackHelp()
    {
        help.SetActive(true);        
        help2.SetActive(false);
        screens.Pop();
        screens.Push(help.gameObject);
    }
    public void Back()
    {
        pausemenu.SetActive(true);

        screens.Pop().SetActive(false); 
    }
    public void MenuMenu()
    {
        EventsManager.TriggerEvent(EventsType.changeScene);
        //Hero.inputBlock = false;

        paused = false;
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }
    public void Restart()
    {
        EventsManager.TriggerEvent(EventsType.changeScene);
        SceneManager.LoadScene("Level 1");
    }
}

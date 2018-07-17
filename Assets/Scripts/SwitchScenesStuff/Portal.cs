using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public Scenes sceneToSwitch;
    public Image fadeImage;
    public float time;
    private bool _now;

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<HeroModel>() != null && other.GetComponent<HeroModel>().gameObject.layer == (int)LayersEnum.HERO)
        {
            startFade();
            _now = true;
        }
    }
    void Update()
    {
        if(!_now)
        {
            return;
        }
        time -= Time.deltaTime;
        if (time <= 0)
        {
            SceneManager.LoadScene((int)sceneToSwitch);
        }

    }
    public void startFade()
    {
        fadeImage.CrossFadeAlpha(255, 5, true);
    }
}

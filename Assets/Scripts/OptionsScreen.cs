using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using LitJson;
using System.IO;

public class OptionsScreen : MonoBehaviour {



    public GameObject firstCanvas;
    public GameObject secondCanvas;
    public GameObject optionsCanvas;
    public Dropdown dpOverall;
    public Dropdown dpResolution;
    public Dropdown dpAntiAliasing;
    public Dropdown dpTextureQuality;
    public Toggle tgleFullScreen;
    public Toggle tgleVSync;
    public Toggle tgleAnisotropic;
    public Toggle tgleFog;

    private OptionsSave optionsSave;


    void Start()
    {
        //optionsSave = new OptionsSave();
        optionsSave = JsonMapper.ToObject<OptionsSave>(File.ReadAllText(Application.dataPath + "/SaveOptions.json"));
       

        dpOverall.value = optionsSave.valueOverall;
        dpResolution.value = optionsSave.valueResolution;
        dpAntiAliasing.value = optionsSave.valueAA;
        dpTextureQuality.value = optionsSave.valueTextureQuality;
        tgleFullScreen.isOn = optionsSave.toggleFullScreen;
        tgleVSync.isOn = optionsSave.toggleVSync;
        tgleAnisotropic.isOn = optionsSave.toggleAnisotropic;
        tgleFog.isOn = optionsSave.toggleFog;


        //optionsSave.valueOverall = dpOverall.value;
        //optionsSave.valueResolution = dpResolution.value;
        //optionsSave.valueAA = dpAntiAliasing.value;
        //optionsSave.valueTextureQuality = dpTextureQuality.value;
        //optionsSave.toggleFullScreen = tgleFullScreen.isOn;
        //optionsSave.toggleVSync = tgleVSync.isOn;
        //optionsSave.toggleAnisotropic = tgleAnisotropic.isOn;








    }

    public void SaveFinish()
    {
        optionsSave = new OptionsSave();

        optionsSave.valueOverall = dpOverall.value;
        optionsSave.valueResolution = dpResolution.value;
        optionsSave.valueAA = dpAntiAliasing.value;
        optionsSave.valueTextureQuality = dpTextureQuality.value;
        optionsSave.toggleFullScreen = tgleFullScreen.isOn;
        optionsSave.toggleVSync = tgleVSync.isOn;
        optionsSave.toggleAnisotropic = tgleAnisotropic.isOn;
        optionsSave.toggleFog = tgleFog.isOn;


        string saveJson = JsonMapper.ToJson(optionsSave);
        File.WriteAllText(Application.dataPath + "/SaveOptions.json", saveJson);

    
    }

	public void ChangeQuality()
    {
        switch (dpOverall.value)
        {
            case 0:
                QualitySettings.SetQualityLevel(0);
                break;
            case 1:
                QualitySettings.SetQualityLevel(1);
                break;
            case 2:
                QualitySettings.SetQualityLevel(2);
                break;
            case 3:
                QualitySettings.SetQualityLevel(3);
                break;
            case 4:
                QualitySettings.SetQualityLevel(4);
                break;
            case 5:
                QualitySettings.SetQualityLevel(5);
                break;
        }
        SaveFinish();
    }
    public void ChangeAntiAliasing()
    {
        switch (dpAntiAliasing.value)
        {
            case 0:
                QualitySettings.antiAliasing = 0;
                break;
            case 1:
                QualitySettings.antiAliasing = 2;
                break;
            case 2:
                QualitySettings.antiAliasing = 4;
                break;
            case 3:
                QualitySettings.antiAliasing = 8;
                break;
        }
    }
    public void ChangeTextureQuality()
    {
        switch (dpTextureQuality.value)
        {
            case 0:
                QualitySettings.masterTextureLimit = 2;
                break;
            case 1:
                QualitySettings.masterTextureLimit = 1;
                break;
            case 2:
                QualitySettings.masterTextureLimit = 0;
                break;
        }
        SaveFinish();
    }
    public void ChangeResolution()
    {
        switch (dpResolution.value)
        {
            case 0:
                Screen.SetResolution(800,600, tgleFullScreen.isOn);
                break;
            case 1:
                Screen.SetResolution(1024, 768, tgleFullScreen.isOn);
                break;
            case 2:
                Screen.SetResolution(1152, 648, tgleFullScreen.isOn);
                break;
            case 3:
                Screen.SetResolution(1152, 864, tgleFullScreen.isOn);
                break;
            case 4:
                Screen.SetResolution(1280, 720, tgleFullScreen.isOn);
                break;
            case 5:
                Screen.SetResolution(1280, 768, tgleFullScreen.isOn);
                break;
            case 6:
                Screen.SetResolution(1280, 800, tgleFullScreen.isOn);
                break;
            case 7:
                Screen.SetResolution(1280, 960, tgleFullScreen.isOn);
                break;
            case 8:
                Screen.SetResolution(1280, 1024, tgleFullScreen.isOn);
                break;
            case 9:
                Screen.SetResolution(1360, 720, tgleFullScreen.isOn);
                break;
            case 10:
                Screen.SetResolution(1600, 900, tgleFullScreen.isOn);
                break;
            case 11:
                Screen.SetResolution(1680, 1050, tgleFullScreen.isOn);
                break;
            case 12:
                Screen.SetResolution(1776, 1000, tgleFullScreen.isOn);
                break;
            case 13:
                Screen.SetResolution(1920, 1080, tgleFullScreen.isOn);
                break;

        }
        SaveFinish();
    }
    public void ChangeFog()
    {
        if (tgleFog.isOn)
        {

            RenderSettings.fog = true;
        }
        else
        {
            RenderSettings.fog = false;
        }
        SaveFinish();
    }
    public void GoBack()
    {
        //SceneManager.LoadScene("Menu");
        firstCanvas.SetActive(true);
        secondCanvas.SetActive(false);
        optionsCanvas.SetActive(false);
        SaveFinish();
    }
    public void ChangeFullScreen()
    {
        if(tgleFullScreen.isOn)
        {
            
            Screen.fullScreen = true;
        }
        else
        {
            Screen.fullScreen = false;
        }
        SaveFinish();
    }
    public void ChangeVSync()
    {
        if (tgleVSync.isOn)
        {

            QualitySettings.vSyncCount = 1;
        }
        else
        {
            QualitySettings.vSyncCount = 0;
        }
        SaveFinish();
    }
    public void ChangeAnisotropic()
    {
        if (tgleAnisotropic.isOn)
        {

            QualitySettings.anisotropicFiltering = AnisotropicFiltering.Enable;
        }
        else
        {
            QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
        }
        SaveFinish();
    }
    void Update () {
	    
	}
}

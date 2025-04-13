using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadingScreen : MonoBehaviour
{
    public GameObject memeSettingImage;
    public GameObject settingWarning;
    bool memeWasShown;

    public void StartGame() 
    {
        SceneManager.LoadScene(1);
        memeWasShown = false;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    public void ActivateMemeImage()
    {
        if(!memeWasShown) 
        {
            memeSettingImage.SetActive(true);
            Invoke("DisableMemeImage", 0.5f);
            memeWasShown = true;
        }
        else
        {
            settingWarning.SetActive(true);
        }
        
    }
    void DisableMemeImage()
    {
        memeSettingImage.SetActive(false);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameMenu : MonoBehaviour
{
    public static GameMenu S;
    public GameObject panel;
    public static bool isGamePaused;
    public GameObject fadingImage;
    public GameObject mainCanvas;
    Color faidingColor;
    public Sprite toBeCont;
    public Sprite gameOver;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (S == null) S = this;
        isGamePaused = false;
        panel.SetActive(false);
        faidingColor = fadingImage.GetComponent<Image>().color;
        DrayEvents.DrayDeath.AddListener(DrayDie);
    }
    public void Init()
    {
        
    }
    

    // Update is called once per frame
    void Update()
    {
        // TO DO make controll of meny (if will be ESC menu)
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isGamePaused)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }
        
        //if (Input.GetKeyDown(KeyCode.T))
        //{

        //}
    }
    public void PauseGame()
    {
        Time.timeScale = 0f;
        panel.SetActive(true);
        isGamePaused = true;
    }
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        panel.SetActive(false);
        isGamePaused = false;
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public static void FirstLevelComplete()
    {
        //Time.timeScale = 0f;
        S.FadingLevel();
    }
    public void FadingLevel()
    {
        StartCoroutine(FADELEVEL());
    }
    void Fade(float n)
    {
        faidingColor.a = n;
        fadingImage.GetComponent<Image>().color = faidingColor;
    }
    void DrayDie()
    {
        Fade(0.55f);
        ChangeCanvasSprite(gameOver);
        Invoke("LoadMainMenu", 4f);
    }
    public IEnumerator FADELEVEL()
    {
        yield return new WaitForSeconds(0.5f);
        Fade(0.45f);
        yield return new WaitForSeconds(0.5f);
        Fade(0.6f);
        yield return new WaitForSeconds(0.5f);
        Fade(0.75f);
        yield return new WaitForSeconds(0.5f);
        Fade(0.85f);
        yield return new WaitForSeconds(1f);
        Fade(1f);
        ChangeCanvasSprite(toBeCont);
        yield return new WaitForSeconds(2f);
        
        SceneManager.LoadScene(0);
    }
    void ChangeCanvasSprite(Sprite spr)
    {
        fadingImage.GetComponent<Image>().color = Color.white;
        fadingImage.GetComponent<Image>().sprite = spr;
    }
    void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }
    
}

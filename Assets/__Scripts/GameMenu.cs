using System.Collections.Generic;
using UnityEngine;

public class GameMenu : MonoBehaviour
{
    public GameObject panel;
    public static bool isGamePaused;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        isGamePaused = false;
        panel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // TO DO make controll of meny (if will be ESC menu)
        if (Input.GetKeyDown(KeyCode.P))
        {
            if(!isGamePaused) 
            { 
                Time.timeScale = 0f;
                panel.SetActive(true);
                isGamePaused = true;
            }
            else 
            {
                Time.timeScale = 1f;
                panel.SetActive(false);
                isGamePaused = false;
            }

        }
       
    }
    
}

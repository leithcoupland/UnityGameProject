using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using XboxCtrlrInput;

public class UIController : MonoBehaviour {

    public GameObject PausedMenu;
    public GameObject ControlMenu;
    public XboxController controller;
    public GameObject gameOver;
    public GameObject player1;
    public GameObject player2;
    public GameObject player3;
    public GameObject player4;
    bool paused;

    // Use this for initialization
    //Sets menus to inactive
    void Start()
    {
        paused = false;
        PausedMenu.SetActive(false);
        ControlMenu.SetActive(false);
        gameOver.SetActive(false);
    }

    // Update is called once per frame
    //Pause the game if start button is pressed on controller or if bool is true
    void Update()
    {
        if (player1 == null && player2 == null && player3 == null && player4 == null)
        {
            gameOver.SetActive(true);
        }
        else
        {
            if (XCI.GetButtonDown(XboxButton.Start, controller))
            {
                paused = !paused;
                if (paused)
                {
                    PausedMenu.SetActive(true);
                }

            }
            if (paused)
            {

                Time.timeScale = 0;
            }
            else
            {
                PausedMenu.SetActive(false);
                ControlMenu.SetActive(false);
                Time.timeScale = 1;

            }
        }
    }

    //Resume game and start time scale
    public void Resume()
    {
        paused = false;
    }

    //Show controls menu and get rid of current menu
    public void Controls()
    {
        PausedMenu.SetActive(false);
        ControlMenu.SetActive(true);
    }

    //Quit and go back to main menu
    public void Quit()
    {
        SceneManager.LoadScene("Menu");
    }

    //Unpause game and return.
    public void Return()
    {
        PausedMenu.SetActive(true);
        ControlMenu.SetActive(false);
    }

    //Set paused bool to true
    public void Pause()
    {
        paused = !paused;
        if (paused)
        {
            PausedMenu.SetActive(true);
        }
    }
}

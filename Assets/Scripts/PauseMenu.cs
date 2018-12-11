using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseMenu;
    [SerializeField]
    private Text pauseTitle;
    [SerializeField]
    private GameObject[] pauseButtons;
    [SerializeField]
    private float secondsPerCharacterInTitleReadout = 0.1f;

    private string fullPauseText;

    void Start()
    {
        pauseMenu.SetActive(false);
        fullPauseText = pauseTitle.text;
    }

    void Update()
    {
        if(Input.GetButtonDown("Pause"))
        {
            SetPause(!pauseMenu.activeSelf);
        }
    }

    void SetPause(bool isPaused)
    {
        Time.timeScale = 0f;
        pauseMenu.SetActive(isPaused);
        if(isPaused)
        {
            for (int i = 0; i < pauseButtons.Length; i++)
            {
                pauseButtons[i].SetActive(false);
            }
            StartCoroutine(DoFancyMenuSetup());
        }
        else
        {
            Time.timeScale = 1f;
            StopAllCoroutines();
        }
    }

    public void ResumeGame()
    {
        SetPause(false);
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }

    private IEnumerator DoFancyMenuSetup()
    {
        for (int charactersDisplayed = 0; charactersDisplayed < fullPauseText.Length; charactersDisplayed++)
        {
            pauseTitle.text = fullPauseText.Substring(0, charactersDisplayed)+ "<color='#d0a9f2'>|</color>";
            yield return new WaitForSecondsRealtime(secondsPerCharacterInTitleReadout);
        }

        StartCoroutine(DoFancyButtonSetup());

        while(true) //Infinite loop - exit condition is stopping the subroutine through SetPause(false)
        {
            pauseTitle.text = fullPauseText + "<color='#d0a9f2'>|</color>";
            yield return new WaitForSecondsRealtime(0.5f);
            pauseTitle.text = fullPauseText;
            yield return new WaitForSecondsRealtime(0.5f);
        }
    }

    private IEnumerator DoFancyButtonSetup()
    {
        for (int i = 0; i < pauseButtons.Length; i++)
        {
            yield return new WaitForSecondsRealtime(0.25f);
            pauseButtons[i].SetActive(true);
        }
    }
}
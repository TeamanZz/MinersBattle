using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreensManager : MonoBehaviour
{
    public static ScreensManager Instance;

    public GameObject loseScreen;
    public GameObject successScreen;

    private void Awake()
    {
        Instance = this;
        Time.timeScale = 1;
    }

    public void ShowLoseScreen()
    {
        Time.timeScale = 0;
        loseScreen.SetActive(true);
    }

    public void ShowSuccessScreen()
    {
        StartCoroutine(IEShowSuccessScreen());
    }

    private IEnumerator IEShowSuccessScreen()
    {
        yield return new WaitForSecondsRealtime(2);
        Time.timeScale = 0;
        successScreen.SetActive(true);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(0);
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(0);
    }
}

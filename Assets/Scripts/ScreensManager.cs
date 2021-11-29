using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreensManager : MonoBehaviour
{
    public static ScreensManager Instance;

    public GameObject loseScreen;
    public GameObject successScreen;
    public GameObject joystickCanvas;
    public AudioSource audioSource;
    public bool endScreenWasInvoked;

    private void Awake()
    {
        Instance = this;
        Time.timeScale = 1;
    }

    public void ShowLoseScreen()
    {
        if (endScreenWasInvoked)
            return;
        joystickCanvas.SetActive(false);
        endScreenWasInvoked = true;
        // Time.timeScale = 0;
        SoundsManager.Instance.DisableBattleMusic();
        loseScreen.SetActive(true);
        audioSource.PlayOneShot(SoundsManager.Instance.endGameSounds[1]);
    }

    public void ShowSuccessScreen()
    {
        if (endScreenWasInvoked)
            return;
        endScreenWasInvoked = true;
        StartCoroutine(IEShowSuccessScreen());
    }

    private IEnumerator IEShowSuccessScreen()
    {
        yield return new WaitForSecondsRealtime(2);
        // Time.timeScale = 0;
        joystickCanvas.SetActive(false);
        SoundsManager.Instance.DisableBattleMusic();
        successScreen.SetActive(true);
        audioSource.PlayOneShot(SoundsManager.Instance.endGameSounds[0]);
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

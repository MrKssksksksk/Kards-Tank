using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PausePageScript : MonoBehaviour
{
    public GameObject pausePage;
    public List<GameObject> otherPages;
    public bool isPause = false;

    private void Start()
    {
        resume();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (isPause) resume();
            else pause();
        }
    }

    public void resume()
    {
        isPause = false;
        pausePage.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void pause()
    {
        isPause = true;
        pausePage.SetActive(true);
        foreach (GameObject page in otherPages)
        {
            page.SetActive(false);
        }
        Time.timeScale = 0f;
    }

    public void quitToMenu()
    {
        StaticData.Instance.initialize();
        SceneManager.LoadScene("StartScene");
    }
}

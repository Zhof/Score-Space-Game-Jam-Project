using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    bool disablesAtStart;
    [SerializeField]
    GameObject otherCanvas;

    private void Start()
    {
        if (disablesAtStart)
            gameObject.SetActive(false);
    }

    public void StartPressed()
    {
        SceneManager.LoadScene(1);
    }
    public void ExitPressed()
    {
        Application.Quit();
    }
    public void HowToPlayPressed()
    {
        otherCanvas.SetActive(true);
        gameObject.SetActive(false);
    }
    public void BackToMenuPressed()
    {
        otherCanvas.SetActive(true);
        gameObject.SetActive(false);
    }
}
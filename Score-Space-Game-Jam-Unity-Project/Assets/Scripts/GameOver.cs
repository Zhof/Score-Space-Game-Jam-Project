using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public void GoAgainPressed()
    {
        SceneManager.LoadScene(1);
    }
    public void BackToMenuPressed()
    {
        SceneManager.LoadScene(0);
    }
}

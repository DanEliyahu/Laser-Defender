using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    [SerializeField] private float gameOverDelay = 2f;
    public void LoadGameOver()
    {
        StartCoroutine(DelayGameOver());
    }

    private IEnumerator DelayGameOver()
    {
        yield return new WaitForSeconds(gameOverDelay);
        SceneManager.LoadScene("Game Over");
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene("Game");
    }

    public void LoadStartMenu()
    {
        SceneManager.LoadScene("Start Menu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

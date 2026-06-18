using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }

    public void OnButtonClicked()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    public void restartgame()
    {
        Scene CurrentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(CurrentScene.name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void GoToScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}

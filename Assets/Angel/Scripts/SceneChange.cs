using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    [SerializeField] private SceneAsset sceneToLoad;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene(sceneToLoad.name);
        }
    }

    public void OnButtonClicked()
    {
        SceneManager.LoadScene(sceneToLoad.name);
    }
}

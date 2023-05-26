using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        StartCoroutine(LoadScene());
    }
    
    private IEnumerator LoadScene()
    {
        AsyncOperation asyncLooad = SceneManager.LoadSceneAsync(1);

        while (!asyncLooad.isDone)
        {
            yield return null;
        }
    }
    
    public void QuitGame()
    {
        // For editor
        UnityEditor.EditorApplication.isPlaying = false;

        // For builds
        Application.Quit();
        
    }
}

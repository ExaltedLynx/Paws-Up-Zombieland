using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController: MonoBehaviour
{
    public static void ChangeLevel(int sceneIndex)
    {
        DataManager.Instance.StartCoroutine(LoadLevelScene(sceneIndex));
        GameManager.Instance.ResetTimeScale();
    }

    public static void RestartLevel()
    {
        // Get the current scene's name
        string currentSceneName = SceneManager.GetActiveScene().name;
        // Reload the current scene
        DataManager.Instance.StartCoroutine(LoadLevelScene(currentSceneName));
        GameManager.Instance.ResetTimeScale();
    }

    //Using the async load scene function lets us add loading screens later
    private static IEnumerator LoadLevelScene(int sceneIndex)
    {
        AsyncOperation asyncOp = SceneManager.LoadSceneAsync(sceneIndex);
        while (!asyncOp.isDone)
        {
            yield return null;
        }
    }

    private static IEnumerator LoadLevelScene(string sceneName)
    {
        AsyncOperation asyncOp = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncOp.isDone)
        {
            yield return null;
        }
    }


}

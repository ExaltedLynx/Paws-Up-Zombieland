using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController: MonoBehaviour
{
    public static void ChangeLevel(int sceneIndex)
    {
        DataManager.Instance.StartCoroutine(LoadLevelScene(sceneIndex));
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
}

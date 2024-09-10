using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public enum SceneType
{
    StartUp,
    MainScene,
    GameScene
}

public class SceneLoadSystem : Singleton<SceneLoadSystem>
{
    private void Start()
    {
        StartCoroutine(LoadMainScene());
    }

    private IEnumerator LoadMainScene()
    {
        yield return new WaitUntil(() => NetworkManager.Singleton != null);
        LoadScene(SceneType.MainScene);
    }

    public void ReLoadScene()
    {
        LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadNextScene()
    {
        LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadScene(SceneType sceneType)
    {
        LoadScene(sceneType.ToString());
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
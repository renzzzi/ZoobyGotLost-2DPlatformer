using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += SceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= SceneLoaded;
    }

    // For Audio
    public event Action<int> OnSceneLoad;

    private void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        OnSceneLoad?.Invoke(scene.buildIndex);
    }

    public int GetCurrentSceneIndex()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }

    public void RetryLevel()
    {
        int currentScene = GetCurrentSceneIndex();
        SceneManager.LoadScene(currentScene);
        SaveManager.Instance.SaveGame(new Vector2(0.5f, 0.5f), 100);
    }

    public void RetrySave()
    {
        SaveManager.Instance.ContinueGame();
    }

    public void LoadNextLevel()
    {
        int currentScene = GetCurrentSceneIndex();
        /* sceneCountInBuildSettings gets the total amount of scenes added 
         * in the build profile. */
        int nextScene = (currentScene + 1) % SceneManager.sceneCountInBuildSettings;
        SceneManager.LoadScene(nextScene);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}

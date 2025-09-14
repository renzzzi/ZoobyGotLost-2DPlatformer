using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using System;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance {  get; private set; }

    private GameData loadedData;
    private string saveFilePath;
    private bool isLoadingGame = false;

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
        
        saveFilePath = Path.Combine(Application.persistentDataPath, "zoobysavedata.json");
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        DoorPortal.OnPlayerWin += DoorPortalSaveGame;    
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        DoorPortal.OnPlayerWin -= DoorPortalSaveGame;
    }

    public void NewGame()
    {
        loadedData = new GameData();
        isLoadingGame = true;
        SceneManager.LoadScene(loadedData.GetRecentSceneName());
    }

    private void LoadGame()
    {
        if (HasSavedData())
        {
            string json = File.ReadAllText(saveFilePath);
            loadedData = JsonUtility.FromJson<GameData>(json);
            Debug.Log("Game loaded successfully");
        }
        else
        {
            Debug.LogWarning("Loading failed, no save file found.");
        }
    }

    public void ContinueGame()
    {
        if (HasSavedData())
        {
            LoadGame();
            isLoadingGame = true;
            SceneManager.LoadScene(loadedData.GetRecentSceneName());
        }
        else
        {
            Debug.LogWarning("No save data found. Create a new game instead.");
        }
    }

    // Default SaveGame function
    public void SaveGame(Vector2 savePosition)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerStats playerStats = player.GetComponent<PlayerStats>();
        if (player == null)
        {
            Debug.LogError("Saving failed, player object not found");
            return;
        }

        // Populate the data to be saved
        GameData dataToSave = new();
        dataToSave.SetPlayerHealth(playerStats.GetHealth());
        dataToSave.SetPlayerPosition(savePosition);
        dataToSave.SetRecentSceneName(SceneManager.GetActiveScene().name);
    
        // Serialization
        string json = JsonUtility.ToJson(dataToSave, true);

        File.WriteAllText(saveFilePath, json);
    }

    // Used for when retrying a level
    public void SaveGame(Vector2 savePosition, int health)
    {
        // Populate the data to be saved
        GameData dataToSave = new();
        dataToSave.SetPlayerHealth(health);
        dataToSave.SetPlayerPosition(savePosition);
        dataToSave.SetRecentSceneName(SceneManager.GetActiveScene().name);

        // Serialization
        string json = JsonUtility.ToJson(dataToSave, true);

        File.WriteAllText(saveFilePath, json);
    }

    // Used for the door portal when player wins
    public void DoorPortalSaveGame()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        int nextScene = currentScene + 1;

        if (nextScene >= SceneManager.sceneCountInBuildSettings)
        {
            File.Delete(saveFilePath);
            return;
        }

        // Populate the data to be saved
        GameData dataToSave = new();
        dataToSave.SetPlayerHealth(100);
        dataToSave.SetPlayerPosition(new Vector2(0.5f, 0.5f));

        
        string sceneName = "MainMenu";
        string scenePath = SceneUtility.GetScenePathByBuildIndex(nextScene);
        sceneName = Path.GetFileNameWithoutExtension(scenePath);
        dataToSave.SetRecentSceneName(sceneName);

        // Serialization
        string json = JsonUtility.ToJson(dataToSave, true);
        File.WriteAllText(saveFilePath, json);

        
    }

    // For updating health bar upon scene loaded
    public event Action<int> SceneLoaded;
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (isLoadingGame)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player == null)
            {
                Debug.LogError("Cannot find the player object in the new scene.");
                return;
            }
            
            PlayerStats playerStats = player.GetComponent<PlayerStats>();
            player.transform.position = loadedData.GetPlayerPosition();
            playerStats.SetHealth(loadedData.GetPlayerHealth());
            SceneLoaded?.Invoke(loadedData.GetPlayerHealth());
            isLoadingGame = false;
        }
    }

    public bool HasSavedData()
    {
        return File.Exists(saveFilePath);
    }
}

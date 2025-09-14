using UnityEngine;

[System.Serializable]
public class GameData
{
    public int playerHealth;
    public Vector2 playerPosition;
    public string recentSceneName;

    public GameData()
    {
        this.playerHealth = 100;
        this.playerPosition = new(0.5f, 0.5f);
        this.recentSceneName = "GrassyLands";
    }

    public int GetPlayerHealth()
    {
        return this.playerHealth;
    }

    public Vector2 GetPlayerPosition()
    {
        return this.playerPosition;
    }

    public string GetRecentSceneName()
    {
        return this.recentSceneName;
    }

    public void SetPlayerHealth(int newPlayerHealth)
    {
        this.playerHealth = newPlayerHealth;
    }

    public void SetPlayerPosition(Vector2 newPlayerPosition)
    {
        this.playerPosition = newPlayerPosition;
    }

    public void SetRecentSceneName(string newRecentSceneName)
    {
        this.recentSceneName = newRecentSceneName;
    }
}

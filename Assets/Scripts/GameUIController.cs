using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

public class GameUIController : MonoBehaviour
{
    // --- UI Element Variables ---
    [SerializeField]
    private UIDocument uiDocument;

    private VisualElement pauseMenuContainer;
    private Button resumeButton;
    private Button pauseExitButton;

    private VisualElement winMenuContainer;
    private Button nextLevelButton;
    private Button winExitButton;
    private Button winRetryButton;

    private VisualElement gameoverMenuContainer;
    private Button gameoverRetryButton;
    private Button gameoverExitButton;

    // -- HUD Variable --
    private VisualElement HUDContainer;
    private Label keyCount;
    private VisualElement healthBarFill;
    private Label healthBarValue;

    // --- Input System Variables ---
    private PlayerInput playerInput; // Reference to the PlayerInput component
    private InputAction pauseAction;   // Reference to our specific "Pause" action

    // --- State Variable ---
    private bool isPaused = false;

    // Awake is called before OnEnable. It's a great place to get components.
    void Awake()
    {
        // Get the PlayerInput component from this same GameObject
        playerInput = GetComponent<PlayerInput>();

        // Find the "UI" action map and then the "Pause" action within it
        pauseAction = playerInput.actions.FindAction("Pause");
    }

    // OnEnable is for setting up UI and subscribing to events
    private void OnEnable()
    {
        // --- UI Setup (same as before) ---
        var root = uiDocument.rootVisualElement;

        // Pause Menu
        pauseMenuContainer = root.Q<VisualElement>("pause-menu-container");
        resumeButton = root.Q<Button>("resume-button");
        pauseExitButton = root.Q<Button>("pause-exit-button");

        resumeButton.clicked += ResumeGame;
        pauseExitButton.clicked += ExitGame;

        // Win Menu
        winMenuContainer = root.Q<VisualElement>("win-menu-container");
        winExitButton = root.Q<Button>("win-exit-button");
        
        winExitButton.clicked += ExitGame;

        // Gameover Menu
        gameoverMenuContainer = root.Q<VisualElement>("gameover-menu-container");
        gameoverRetryButton = root.Q<Button>("gameover-retry-button");
        gameoverExitButton = root.Q<Button>("gameover-exit-button");

        gameoverRetryButton.clicked += LoadExampleScene;
        gameoverExitButton.clicked += ExitGame;
        PlayerStats.Instance.OnPlayerDeath += ShowGameoverMenu;

        // HUD
        HUDContainer = root.Q<VisualElement>("hud-container");
        keyCount = root.Q<Label>("score-count");
        PlayerStats.Instance.OnKeyCollect += UpdateKeyCountDisplay;
        healthBarFill = root.Q<VisualElement>("health-bar-fill");
        healthBarValue = root.Q<Label>("health-bar-value");
        PlayerStats.Instance.OnDamageInflicted += UpdateHealthBar;

        DoorPortal.OnPlayerWin += ShowWinMenu;

        pauseAction.performed += TogglePauseMenu;

        // --- Initial State (same as before) ---
        pauseMenuContainer.AddToClassList("hidden");
        Time.timeScale = 1f;
    }

    // OnDisable is called when the object is disabled. It's crucial for cleanup.
    private void OnDisable()
    {
        // Unsubscribe from the events to prevent memory leaks and errors.
        resumeButton.clicked -= ResumeGame;
        pauseExitButton.clicked -= ExitGame;
        DoorPortal.OnPlayerWin -= ShowWinMenu;
        PlayerStats.Instance.OnKeyCollect -= UpdateKeyCountDisplay;
        PlayerStats.Instance.OnDamageInflicted -= UpdateHealthBar;
        PlayerStats.Instance.OnPlayerDeath -= ShowGameoverMenu;
        pauseAction.performed -= TogglePauseMenu;

        gameoverRetryButton.clicked -= LoadExampleScene;
    }

    private void LoadExampleScene()
    {
        GameManager.Instance.LoadLevel();
        AudioManager.Instance.RestartBackgroundMusic();
    }

    private void SetPauseState(bool pauseState)
    {
        isPaused = pauseState;

        if (isPaused)
        {
            Time.timeScale = 0f;
            pauseMenuContainer.RemoveFromClassList("hidden");
        }
        else
        {
            Time.timeScale = 1f;
            pauseMenuContainer.AddToClassList("hidden");
        }
    }

    private void TogglePauseMenu(InputAction.CallbackContext _)
    {
        SetPauseState(!isPaused);
    }

    private void ResumeGame()
    {
        SetPauseState(false);
    }

    private void ShowWinMenu()
    {
        Time.timeScale = 0f;
        winMenuContainer.RemoveFromClassList("hidden");
    }
    private void ShowGameoverMenu()
    {
        Time.timeScale = 0f;
        gameoverMenuContainer.RemoveFromClassList("hidden");
    }

    private void ExitGame()
    {
        Debug.Log("Exit button clicked!");
        Application.Quit();
    }

    private void UpdateKeyCountDisplay(int newKeyCount)
    {
        if (keyCount != null)
        {
            keyCount.text = newKeyCount.ToString();
        }
    }

    private void UpdateHealthBar(float currentHealth)
    {
        if (currentHealth <= 0f)
        {
            healthBarFill.style.width = Length.Percent(0f);
            healthBarValue.text = "0";
        }
        else
        {
            healthBarFill.style.width = Length.Percent(currentHealth);
            healthBarValue.text = currentHealth.ToString();
        }
    }

    public void HideHUD()
    {
        HUDContainer.AddToClassList("hidden");
    }
}
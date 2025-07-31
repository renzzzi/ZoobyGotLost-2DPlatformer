using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

public class GameUIController : MonoBehaviour
{
    // --- UI Element Variables ---
    [SerializeField]
    private UIDocument uiDocument;

    private VisualElement pauseMenuContainer;
    private Button pauseResumeButton;

    private VisualElement winMenuContainer;
    private Button winNextLevelButton;

    private VisualElement gameoverMenuContainer;
    private Button gameoverRetryButton;

    // HUD Variable
    private VisualElement HUDContainer;
    private Label keyCount;
    private VisualElement healthBarFill;
    private Label healthBarValue;

    // Input System Variables
    private PlayerInput playerInput; // Reference to the PlayerInput component
    private InputAction pauseAction;   // Reference to our specific "Pause" action

    // State Variable
    private bool isPaused = false;

    // Awake is called before OnEnable. It's a great place to get components.
    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        pauseAction = playerInput.actions.FindAction("Pause");
    }

    // OnEnable is for setting up UI and subscribing to events
    private void OnEnable()
    {
        // UI Setup
        var root = uiDocument.rootVisualElement;

        // Pause Menu
        pauseMenuContainer = root.Q<VisualElement>("pause-menu-container");
        pauseResumeButton = root.Q<Button>("pause-resume-button");

        pauseResumeButton.clicked += ResumeGame;

        // Win Menu
        winMenuContainer = root.Q<VisualElement>("win-menu-container");
        winNextLevelButton = root.Q<Button>("win-next-level-button");

        winNextLevelButton.clicked += GoToNextLevel;

        // Gameover Menu
        gameoverMenuContainer = root.Q<VisualElement>("gameover-menu-container");
        gameoverRetryButton = root.Q<Button>("gameover-retry-button");

        gameoverRetryButton.clicked += RetryCurrentLevel;
        PlayerStats.Instance.AfterPlayerDeathAnim += ShowGameoverMenu;

        // HUD
        HUDContainer = root.Q<VisualElement>("hud-container");
        keyCount = root.Q<Label>("score-count");
        healthBarFill = root.Q<VisualElement>("health-bar-fill");
        healthBarValue = root.Q<Label>("health-bar-value");
        PlayerStats.Instance.OnKeyCollect += UpdateKeyCountDisplay;
        PlayerStats.Instance.OnDamageInflicted += UpdateHealthBar;

        // SFX for buttons
        var allSFXButtons = root.Query<Button>(null, "button").ToList();

        foreach (var button in allSFXButtons)
        {
            button.clicked += ButtonClickSFX;
            button.RegisterCallback<PointerEnterEvent>(e => ButtonHoverSFX());
        }

        // Exit Buttons
        var allExitButtons = root.Query<Button>(null, "exit").ToList();

        foreach (var button in allExitButtons)
        {
            button.clicked += ExitGame;
        }

        // Back Buttons
        var allBackButtons = root.Query<Button>(null, "back").ToList();
        /*as
        foreach (var button in allBackButtons)
        {
            button.clicked += 
        }
        */
        // Extras
        DoorPortal.OnPlayerWin += ShowWinMenu;
        pauseAction.performed += TogglePauseMenu;
        pauseMenuContainer.AddToClassList("hidden");
        Time.timeScale = 1f;
    }

    private void OnDisable()
    {
        pauseResumeButton.clicked -= ResumeGame;
        winNextLevelButton.clicked -= GoToNextLevel;
        gameoverRetryButton.clicked -= RetryCurrentLevel;

        DoorPortal.OnPlayerWin -= ShowWinMenu;
        PlayerStats.Instance.OnKeyCollect -= UpdateKeyCountDisplay;
        PlayerStats.Instance.OnDamageInflicted -= UpdateHealthBar;
        PlayerStats.Instance.AfterPlayerDeathAnim -= ShowGameoverMenu;
        pauseAction.performed -= TogglePauseMenu;
    }

    private void RetryCurrentLevel()
    {
        GameManager.Instance.RetryLevel();
        AudioManager.Instance.RestartBackgroundMusic();
    }

    private void GoToNextLevel()
    {
        GameManager.Instance.LoadNextLevel();
        AudioManager.Instance.RestartBackgroundMusic();
    }

    private void SetPauseState(bool pauseState)
    {
        isPaused = pauseState;

        if (isPaused)
        {
            AudioManager.Instance.PauseMusic();
            AudioManager.Instance.PlaySFX(SoundType.PauseGame);
            Time.timeScale = 0f;
            pauseMenuContainer.RemoveFromClassList("hidden");
        }
        else
        {
            AudioManager.Instance.UnpauseMusic();
            AudioManager.Instance.PlaySFX(SoundType.ResumeGame);
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

    private void ButtonClickSFX()
    {
        AudioManager.Instance.PlaySFX(SoundType.ButtonClick);
    }

    private void ButtonHoverSFX()
    {
        AudioManager.Instance.PlaySFX(SoundType.ButtonHover);
    }
}
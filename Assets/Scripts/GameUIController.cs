using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;
using System.Collections.Generic;

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

    private List<Button> allButtons;
    private List<Button> allQuitButtons;
    private List<Button> allMenuButtons;
    private List<Button> allRetryButtons;

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

        pauseResumeButton.RegisterCallback<ClickEvent>(e => ResumeGame());

        // Win Menu
        winMenuContainer = root.Q<VisualElement>("win-menu-container");
        winNextLevelButton = root.Q<Button>("win-next-level-button");

        winNextLevelButton.RegisterCallback<ClickEvent>(e => NextLevel());

        // Gameover Menu
        gameoverMenuContainer = root.Q<VisualElement>("gameover-menu-container");
        PlayerStats.Instance.AfterPlayerDeathAnim += ShowGameoverMenu;

        // HUD
        HUDContainer = root.Q<VisualElement>("hud-container");
        keyCount = root.Q<Label>("score-count");
        healthBarFill = root.Q<VisualElement>("health-bar-fill");
        healthBarValue = root.Q<Label>("health-bar-value");
        PlayerStats.Instance.OnKeyCollect += UpdateKeyCountDisplay;
        PlayerStats.Instance.OnDamageInflicted += UpdateHealthBar;

        // SFX and Hover Cursor for buttons
        allButtons = root.Query<Button>(null, "button").ToList();

        foreach (var button in allButtons)
        {
            button.RegisterCallback<ClickEvent>(e => ButtonClickSFX());
            button.RegisterCallback<PointerEnterEvent>(e => ButtonHoverEnter());
            button.RegisterCallback<PointerLeaveEvent>(e => ButtonHoverExit());
        }

        // Quit Buttons
        allQuitButtons = root.Query<Button>(null, "quit").ToList();

        foreach (var button in allQuitButtons)
        {
            button.RegisterCallback<ClickEvent>(e => QuitGame());
        }

        // Menu Buttons
        allMenuButtons = root.Query<Button>(null, "menu").ToList();

        foreach (var button in allMenuButtons)
        {
            button.RegisterCallback<ClickEvent>(e => GoToMainMenu());
        }
  

        // Retry Buttons
        allRetryButtons = root.Query<Button>(null, "retry").ToList();
        foreach ( var button in allRetryButtons)
        {
            button.RegisterCallback<ClickEvent>(e => RetryLevel());
        }

        // Extras
        DoorPortal.OnPlayerWin += ShowWinMenu;
        pauseAction.performed += TogglePauseMenu;
        pauseMenuContainer.AddToClassList("hidden");
        Time.timeScale = 1f;
    }

    private void OnDisable()
    {
        pauseResumeButton.UnregisterCallback<ClickEvent>(e => ResumeGame());
        winNextLevelButton.UnregisterCallback<ClickEvent>(e => NextLevel());

        foreach (var button in allButtons)
        {
            button.UnregisterCallback<ClickEvent>(e => ButtonClickSFX());
            button.UnregisterCallback<PointerEnterEvent>(e => ButtonHoverEnter());
            button.UnregisterCallback<PointerLeaveEvent>(e => ButtonHoverExit());
        }

        foreach (var button in allQuitButtons)
        {
            button.RegisterCallback<ClickEvent>(e => QuitGame());
        }

        foreach (var button in allMenuButtons)
        {
            button.UnregisterCallback<ClickEvent>(e => GoToMainMenu());
        }

        foreach (var button in allRetryButtons)
        {
            button.UnregisterCallback<ClickEvent>(e => RetryLevel());
        }

        DoorPortal.OnPlayerWin -= ShowWinMenu;
        PlayerStats.Instance.OnKeyCollect -= UpdateKeyCountDisplay;
        PlayerStats.Instance.OnDamageInflicted -= UpdateHealthBar;
        PlayerStats.Instance.AfterPlayerDeathAnim -= ShowGameoverMenu;
        pauseAction.performed -= TogglePauseMenu;
    }

    // For resume buttons
    private void ResumeGame()
    {
        SetPauseState(false);
    }

    // For retry buttons
    private void RetryLevel()
    {
        GameManager.Instance.RetryLevel();
        AudioManager.Instance.RestartBackgroundMusic();
    }

    // For menu buttons
    private void GoToMainMenu()
    {
        GameManager.Instance.LoadMainMenu();
    }

    // For quit buttons
    private void QuitGame()
    {
        Debug.Log("Quit button clicked!");
        Application.Quit();
    }

    private void NextLevel()
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

    // For pausing when pressing the escape key
    private void TogglePauseMenu(InputAction.CallbackContext _)
    {
        SetPauseState(!isPaused);
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

    private void ButtonHoverEnter()
    {
        AudioManager.Instance.PlaySFX(SoundType.ButtonHover);
        CursorManager.Instance.SetCursorState(CursorManager.CursorState.Hover);
    }

    private void ButtonHoverExit()
    {
        CursorManager.Instance.SetCursorState(CursorManager.CursorState.Default);
    }
}
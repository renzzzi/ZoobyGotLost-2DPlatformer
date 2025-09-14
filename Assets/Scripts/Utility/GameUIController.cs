using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class GameUIController : BaseUIController
{
    // --- UI Element Variables ---
    [SerializeField]
    private UIDocument uiDocument;

    private VisualElement pauseMenuContainer;
    private Button pauseResumeButton;

    private VisualElement winMenuContainer;
    private Button winNextLevelButton;

    private VisualElement gameoverMenuContainer;

    private VisualElement quitModal;
    private Button quitYesButton;
    private Button quitNoButton;

    private VisualElement menuModal;
    private Button menuYesButton;
    private Button menuNoButton;

    private VisualElement retrySaveModal;
    private Button retrySaveYesButton;
    private Button retrySaveNoButton;

    private VisualElement retryLevelModal;
    private Button retryLevelYesButton;
    private Button retryLevelNoButton;

    // HUD Variable
    private VisualElement HUDContainer;
    private VisualElement healthBarFill;
    private Label healthBarValue;

    private List<Button> allButtons;
    private List<Button> allQuitButtons;
    private List<Button> allMenuButtons;
    private List<Button> allRetrySaveButtons;
    private List<Button> allRetryLevelButtons;

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

        // Modals
        quitModal = root.Q<VisualElement>("quit-modal");
        quitYesButton = root.Q<Button>("quit-yes-button");
        quitNoButton = root.Q<Button>("quit-no-button");
        quitYesButton.RegisterCallback<ClickEvent>(e => QuitGame());
        quitNoButton.RegisterCallback<ClickEvent>(e => HideQuitModal());

        menuModal = root.Q<VisualElement>("menu-modal");
        menuYesButton = root.Q<Button>("menu-yes-button");
        menuNoButton = root.Q<Button>("menu-no-button");
        menuYesButton.RegisterCallback<ClickEvent>(e => GoToMainMenu());
        menuNoButton.RegisterCallback<ClickEvent>(e => HideMenuModal());

        retrySaveModal = root.Q<VisualElement>("retry-save-modal");
        retrySaveYesButton = root.Q<Button>("retry-save-yes-button");
        retrySaveNoButton = root.Q<Button>("retry-save-no-button");
        retrySaveYesButton.RegisterCallback<ClickEvent>(e => RetrySave());
        retrySaveNoButton.RegisterCallback<ClickEvent>(e => HideRetrySaveModal());

        retryLevelModal = root.Q<VisualElement>("retry-level-modal");
        retryLevelYesButton = root.Q<Button>("retry-level-yes-button");
        retryLevelNoButton = root.Q<Button>("retry-level-no-button");
        retryLevelYesButton.RegisterCallback<ClickEvent>(e => RetryLevel());
        retryLevelNoButton.RegisterCallback<ClickEvent>(e => HideRetryLevelModal());

        // HUD
        HUDContainer = root.Q<VisualElement>("hud-container");
        healthBarFill = root.Q<VisualElement>("health-bar-fill");
        healthBarValue = root.Q<Label>("health-bar-value");
        PlayerStats.Instance.OnDamageInflicted += UpdateHealthBar;
        PlayerStats.Instance.OnPlayerHealed += UpdateHealthBar;

        // SFX and Hover Cursor for buttons
        allButtons = root.Query<Button>(null, "button").ToList();

        foreach (var button in allButtons)
        {
            button.RegisterCallback<ClickEvent>(e => ButtonClickSFX());
            button.RegisterCallback<PointerEnterEvent>(e => ButtonHoverEnter());
            button.RegisterCallback<PointerLeaveEvent>(e => ButtonHoverExit());
        }

        // Quit Buttons
        allQuitButtons = root.Query<Button>(null, "quit-button").ToList();

        foreach (var button in allQuitButtons)
        {
            button.RegisterCallback<ClickEvent>(e => ShowQuitModal());
        }

        // Menu Buttons
        allMenuButtons = root.Query<Button>(null, "menu-button").ToList();

        foreach (var button in allMenuButtons)
        {
            button.RegisterCallback<ClickEvent>(e => ShowMenuModal());
        }

        // Retry Save Buttons
        allRetrySaveButtons = root.Query<Button>(null, "retry-save-button").ToList();
        foreach ( var button in allRetrySaveButtons)
        {
            button.RegisterCallback<ClickEvent>(e => ShowRetrySaveModal());
        }

        // Retry Level Buttons
        allRetryLevelButtons = root.Query<Button>(null, "retry-level-button").ToList();
        foreach (var button in allRetryLevelButtons)
        {
            button.RegisterCallback<ClickEvent>(e => ShowRetryLevelModal());
        }

        // Extras
        DoorPortal.OnPlayerWin += ShowWinMenu;
        pauseAction.performed += TogglePauseMenu;
        pauseMenuContainer.AddToClassList("hidden");
        SaveManager.Instance.SceneLoaded += UpdateHealthBar;
        Time.timeScale = 1f;
    }

    private void OnDisable()
    {
        pauseResumeButton.UnregisterCallback<ClickEvent>(e => ResumeGame());
        winNextLevelButton.UnregisterCallback<ClickEvent>(e => NextLevel());

        quitYesButton.UnregisterCallback<ClickEvent>(e => QuitGame());
        quitNoButton.UnregisterCallback<ClickEvent>(e => HideQuitModal());

        menuYesButton.UnregisterCallback<ClickEvent>(e => GoToMainMenu());
        menuNoButton.UnregisterCallback<ClickEvent>(e => HideMenuModal());

        retrySaveYesButton.UnregisterCallback<ClickEvent>(e => RetrySave());
        retrySaveNoButton.UnregisterCallback<ClickEvent>(e => HideRetrySaveModal());

        retryLevelYesButton.UnregisterCallback<ClickEvent>(e => RetryLevel());
        retryLevelNoButton.UnregisterCallback<ClickEvent>(e => HideRetryLevelModal());

        foreach (var button in allButtons)
        {
            button.UnregisterCallback<ClickEvent>(e => ButtonClickSFX());
            button.UnregisterCallback<PointerEnterEvent>(e => ButtonHoverEnter());
            button.UnregisterCallback<PointerLeaveEvent>(e => ButtonHoverExit());
        }

        foreach (var button in allQuitButtons)
        {
            button.RegisterCallback<ClickEvent>(e => ShowQuitModal());
        }

        foreach (var button in allMenuButtons)
        {
            button.UnregisterCallback<ClickEvent>(e => ShowMenuModal());
        }

        foreach (var button in allRetrySaveButtons)
        {
            button.UnregisterCallback<ClickEvent>(e => ShowRetrySaveModal());
        }

        foreach (var button in allRetryLevelButtons)
        {
            button.UnregisterCallback<ClickEvent>(e => ShowRetryLevelModal());
        }

        DoorPortal.OnPlayerWin -= ShowWinMenu;
        PlayerStats.Instance.OnDamageInflicted -= UpdateHealthBar;
        PlayerStats.Instance.OnPlayerHealed -= UpdateHealthBar;
        PlayerStats.Instance.AfterPlayerDeathAnim -= ShowGameoverMenu;
        pauseAction.performed -= TogglePauseMenu;
        SaveManager.Instance.SceneLoaded -= UpdateHealthBar;
    }

    // For resume buttons
    private void ResumeGame()
    {
        SetPauseState(false);
    }

    // For retry buttons
    private void RetrySave()
    {
        GameManager.Instance.RetrySave();
    }

    private void RetryLevel()
    {
        GameManager.Instance.RetryLevel();
    }

    // For menu buttons
    private void GoToMainMenu()
    {
        AudioManager.Instance.PlaySFX(SoundType.ExitMainMenu);
        GameManager.Instance.LoadMainMenu();
    }

    // For next level buttons
    private void NextLevel()
    {
        GameManager.Instance.LoadNextLevel();
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

    
    // Updating HUD from PlayerStats

    private void UpdateHealthBar(int currentHealth)
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

    // Hiding and Showing UI
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

    public void HideHUD()
    {
        HUDContainer.AddToClassList("hidden");
    }

    private void ShowQuitModal()
    {
        quitModal.RemoveFromClassList("hidden");
    }

    private void HideQuitModal()
    {
        quitModal.AddToClassList("hidden");
    }

    private void ShowMenuModal()
    {
        menuModal.RemoveFromClassList("hidden");
    }

    private void HideMenuModal()
    {
        menuModal.AddToClassList("hidden");
    }

    private void ShowRetrySaveModal()
    {
        retrySaveModal.RemoveFromClassList("hidden");
    }
    private void HideRetrySaveModal()
    {
        retrySaveModal.AddToClassList("hidden");
    }

    private void ShowRetryLevelModal()
    {
        retryLevelModal.RemoveFromClassList("hidden");
    }
    private void HideRetryLevelModal()
    {
        retryLevelModal.AddToClassList("hidden");
    }
}
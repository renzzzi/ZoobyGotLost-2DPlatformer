using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

public class MainMenuUIController : BaseUIController
{
    [SerializeField] private UIDocument uiDocument;
    private Button newGameButton;
    private Button continueButton;
    private Button quitButton;

    private Label continueLevelLabel;

    private List<Button> allButtons;

    private void Awake()
    {
        Time.timeScale = 0f;
        var root = uiDocument.rootVisualElement;

        newGameButton = root.Q<Button>("new-game-button");
        continueButton = root.Q<Button>("continue-button");
        quitButton = root.Q<Button>("quit-button");

        newGameButton.RegisterCallback<ClickEvent>(e => NewGame());
        SubscribeContinueButton();
        quitButton.RegisterCallback<ClickEvent>(e => QuitGame());

        allButtons = root.Query<Button>(null, "button").ToList();
        foreach (var button in allButtons)
        {
            button.RegisterCallback<ClickEvent>(e => ButtonClickSFX());
            button.RegisterCallback<PointerEnterEvent>(e => ButtonHoverEnter());
            button.RegisterCallback<PointerLeaveEvent>(e => ButtonHoverExit());
        }

        continueLevelLabel = root.Q<Label>("continue-level-label");
        if (!SaveManager.Instance.HasSavedData())
        {
            continueLevelLabel.AddToClassList("hidden");
        }
        else
        {
            //continueLevelLabel.text = SaveManager.
            continueLevelLabel.RemoveFromClassList("hidden");
        }
    }

    private void OnDestroy()
    {
        newGameButton.UnregisterCallback<ClickEvent>(e => NewGame());
        UnsubscribeContinueButton();
        quitButton.UnregisterCallback<ClickEvent>(e => QuitGame());

        foreach (var button in allButtons)
        {
            button.UnregisterCallback<ClickEvent>(e => ButtonClickSFX());
            button.UnregisterCallback<PointerEnterEvent>(e => ButtonHoverEnter());
            button.UnregisterCallback<PointerLeaveEvent>(e => ButtonHoverExit());
        }
    }

    private void NewGame() 
    {
        SaveManager.Instance.NewGame();
    }
    
    private void ContinueGame() 
    {
        SaveManager.Instance.ContinueGame();
    }

    private void SubscribeContinueButton()
    {
        if (SaveManager.Instance.HasSavedData())
        {
            continueButton.RemoveFromClassList("continue-button-disabled");
            continueButton.AddToClassList("continue-button");
            continueButton.RegisterCallback<ClickEvent>(e => ContinueGame());
        }
        else
        {
            continueButton.RemoveFromClassList("continue-button");
            continueButton.AddToClassList("continue-button-disabled");
        }
    }

    private void UnsubscribeContinueButton()
    {
        if (SaveManager.Instance.HasSavedData())
        {
            continueButton.UnregisterCallback<ClickEvent>(e => ContinueGame());
        }
    }

}

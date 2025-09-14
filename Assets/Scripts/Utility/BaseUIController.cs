using UnityEngine;

public class BaseUIController : MonoBehaviour
{
    protected void ButtonClickSFX()
    {
        AudioManager.Instance.PlaySFX(SoundType.ButtonClick);
    }

    protected void ButtonHoverEnter()
    {
        AudioManager.Instance.PlaySFX(SoundType.ButtonHover);
        CursorManager.Instance.SetCursorState(CursorManager.CursorState.Hover);
    }

    protected void ButtonHoverExit()
    {
        CursorManager.Instance.SetCursorState(CursorManager.CursorState.Default);
    }

    // For quit buttons
    protected void QuitGame()
    {
        Debug.Log("Quit button clicked!");
        Application.Quit();
    }
}

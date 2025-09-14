using UnityEngine;

public class ShadowManager : MonoBehaviour
{
    // A "Singleton" makes this manager easily accessible from any other script
    public static ShadowManager Instance { get; private set; }

    [SerializeField] private float fadeDuration = 0.5f;

    [Tooltip("Drag the trigger object for the area the player starts in here.")]
    [SerializeField] private ShadowArea startingArea;

    private ShadowArea currentActiveArea;

    private void Awake()
    {
        // Singleton setup
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        // When the game begins, set the starting area as the active one.
        if (startingArea != null)
        {
            SetCurrentArea(startingArea);
        }
    }

    // This is the core logic!
    public void SetCurrentArea(ShadowArea newArea)
    {
        // If the player is just re-entering the area they are already in, do nothing.
        // THIS IS THE LINE THAT SOLVES YOUR PROBLEM.
        if (newArea == currentActiveArea)
        {
            return;
        }

        // If there was a previous area, tell it to fade OUT (alpha 0).
        if (currentActiveArea != null)
        {
            currentActiveArea.Fade(0f, fadeDuration);
        }

        // Tell the new area to fade IN (alpha 1).
        newArea.Fade(1f, fadeDuration);

        // Remember the new area as the current one for next time.
        currentActiveArea = newArea;
    }
}
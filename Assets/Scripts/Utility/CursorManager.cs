using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public enum CursorState { Default, Hover }
    public static CursorManager Instance { get; private set; }

    [SerializeField] private Texture2D cursorDefault;
    [SerializeField] private Texture2D cursorHover;
    ///
    /// Where the "tip" of the cursor is, in this case it is in the middle
    /// 32x32 grid so the middle is at x and y 16
    ///
    private Vector2 hotspot = new(16, 16);

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

    private void Start()
    {
        Cursor.SetCursor(cursorDefault, hotspot, CursorMode.Auto);
    }

    private void Update()
    {
        // Disables the cursor when playing
        if (Time.timeScale > 0.1f)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void SetCursorState(CursorState state)
    {
        if (state == CursorState.Default)
        {
            Cursor.SetCursor(cursorDefault, hotspot, CursorMode.Auto);
        }
        else if (state == CursorState.Hover)
        {
            Cursor.SetCursor(cursorHover, hotspot, CursorMode.Auto);
        }
    }
}

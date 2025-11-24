using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public static CursorManager Instance { get; private set; }

    [Header("Texturas do Cursor")]
    [SerializeField] private Texture2D defaultCursorTexture;
    [SerializeField] private Vector2 defaultCursorHotspot;
    
    [SerializeField] private Texture2D handCursorTexture;
    [SerializeField] private Vector2 handCursorHotspot;

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
        SetDefault();
    }

    public void SetDefault()
    {
        Cursor.SetCursor(defaultCursorTexture, defaultCursorHotspot, CursorMode.Auto);
    }

    public void SetHand()
    {
        Cursor.SetCursor(handCursorTexture, handCursorHotspot, CursorMode.Auto);
    }
}
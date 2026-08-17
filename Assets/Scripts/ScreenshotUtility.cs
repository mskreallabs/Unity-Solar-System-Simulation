using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Handles taking screenshots.
/// </summary>
public class ScreenshotUtility : MonoBehaviour
{
    public static ScreenshotUtility Instance { get; private set; }

    [Header("Screenshot Settings")]

    [Tooltip("Allow screenshots when running a standalone build.")]
    [SerializeField] private bool runOnBuild = false;

    [Tooltip("Key used to capture a screenshot.")]
    [SerializeField] private Key screenshotKey = Key.S;

    [Tooltip("Screenshot resolution scale factor.")]
    [Min(1)]
    [SerializeField] private int scaleFactor = 1;

    private int imageCount;

    private const string ImageCountKey = "IMAGE_CNT";
    private const string ScreenshotFolder = "Screenshots";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        if (!runOnBuild && !Application.isEditor)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        imageCount = PlayerPrefs.GetInt(ImageCountKey, 0);

        if (!Directory.Exists(ScreenshotFolder))
        {
            Directory.CreateDirectory(ScreenshotFolder);
        }
    }

    private void Update()
    {
        if (Keyboard.current == null)
            return;

        if (Keyboard.current[screenshotKey].wasPressedThisFrame)
        {
            TakeScreenshot();
        }
    }

    /// <summary>
    /// Takes a screenshot.
    /// </summary>
    public void TakeScreenshot()
    {
        imageCount++;

        PlayerPrefs.SetInt(ImageCountKey, imageCount);
        PlayerPrefs.Save();

        int width = Screen.width * scaleFactor;
        int height = Screen.height * scaleFactor;

        string fileName = $"Screenshot_{width}x{height}_{imageCount}.png";
        string path = Path.Combine(ScreenshotFolder, fileName);

        ScreenCapture.CaptureScreenshot(path, scaleFactor);

        Debug.Log($"Screenshot saved to: {path}");
    }
}
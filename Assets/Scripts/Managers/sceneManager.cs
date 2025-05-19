using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    public static SceneManager Instance { get; private set; }

    [SerializeField] private float fadeTime = 1.0f;
    [SerializeField] private CanvasGroup fadeCanvasGroup;
    
    private bool isTransitioning = false;

    void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        if (fadeCanvasGroup == null)
        {
            CreateFadeCanvas();
        }
    }

    private void CreateFadeCanvas()
    {
        // Create a canvas for the fade effect
        GameObject fadeCanvas = new GameObject("FadeCanvas");
        fadeCanvas.transform.SetParent(transform);
        Canvas canvas = fadeCanvas.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 999;
        
        fadeCanvasGroup = fadeCanvas.AddComponent<CanvasGroup>();
        fadeCanvasGroup.alpha = 0f;
        
        // Add a black image to fill the screen
        GameObject imageObj = new GameObject("BlackFade");
        imageObj.transform.SetParent(fadeCanvas.transform, false);
        UnityEngine.UI.Image image = imageObj.AddComponent<UnityEngine.UI.Image>();
        image.color = Color.black;
        
        // Set it to fill the screen
        RectTransform rect = imageObj.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0, 0);
        rect.anchorMax = new Vector2(1, 1);
        rect.sizeDelta = Vector2.zero;
    }

    public void LoadScene(string sceneName)
    {
        if (!isTransitioning)
        {
            StartCoroutine(LoadSceneRoutine(sceneName));
        }
    }

    private IEnumerator LoadSceneRoutine(string sceneName)
    {
        isTransitioning = true;

        // Fade to black
        yield return StartCoroutine(FadeRoutine(1f));

        // Load the new scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);

        // Wait a frame to ensure scene is loaded
        yield return null;

        // Fade back in
        yield return StartCoroutine(FadeRoutine(0f));

        isTransitioning = false;
    }

    private IEnumerator FadeRoutine(float targetAlpha)
    {
        float startAlpha = fadeCanvasGroup.alpha;
        float time = 0;

        while (time < fadeTime)
        {
            time += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / fadeTime);
            yield return null;
        }

        fadeCanvasGroup.alpha = targetAlpha;
    }
}
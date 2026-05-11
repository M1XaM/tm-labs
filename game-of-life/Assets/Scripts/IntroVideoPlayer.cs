using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class IntroVideoPlayer : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private string nextSceneName = "GameScene"; 
    [SerializeField] private Camera targetCamera;
    [SerializeField] private float holdDuration = 2f; 
    
    [Header("Skip Text Settings")]
    [SerializeField] private float fadeInDuration = 0.1f;      // Faster fade in
    [SerializeField] private float fadeOutDuration = 0.1f;     // Faster fade out
    [SerializeField] private float visibleDuration = 0.2f;     // Shorter visible time
    [SerializeField] private float hiddenDuration = 0.3f;      // Shorter hidden time
    [SerializeField] private float textDisplayDuration = 5f;

    private float holdTimer = 0f;
    private bool isHolding = false;
    private Text skipText;
    private Canvas canvas;
    private bool isTextFading = false;
    private Coroutine fadeLoopCoroutine;

    private void Start()
    {
        if (videoPlayer == null)
        {
            videoPlayer = GetComponent<VideoPlayer>();
        }

        if (targetCamera == null)
        {
            targetCamera = Camera.main;
        }

        // Set camera background to black
        targetCamera.clearFlags = CameraClearFlags.SolidColor;
        targetCamera.backgroundColor = Color.black;

        // Set up video player
        videoPlayer.renderMode = VideoRenderMode.CameraNearPlane; 
        videoPlayer.targetCamera = targetCamera;
        videoPlayer.aspectRatio = VideoAspectRatio.FitVertically;
        videoPlayer.playOnAwake = true;
        videoPlayer.waitForFirstFrame = true;

        // Ensure the video is visible
        videoPlayer.targetCameraAlpha = 1f;

        videoPlayer.loopPointReached += OnVideoFinished;
        videoPlayer.Play();

        // Create UI elements
        CreateSkipText();

        Debug.Log("Video Player initialized. Video clip: " + (videoPlayer.clip != null ? videoPlayer.clip.name : "null"));
        Debug.Log("Video dimensions: " + (videoPlayer.clip != null ? videoPlayer.clip.width + "x" + videoPlayer.clip.height : "null"));
    }

    private void CreateSkipText()
    {
        canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasObj = new GameObject("SkipCanvas");
            canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();
        }

        // Create Text GameObject
        GameObject textObj = new GameObject("SkipText");
        textObj.transform.SetParent(canvas.transform, false);
        
        // Add and configure Text component
        skipText = textObj.AddComponent<Text>();
        skipText.text = "Hold SPACE to skip";
        skipText.color = new Color(1f, 1f, 1f, 0f); // Start fully transparent
        skipText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        skipText.fontSize = 24;
        skipText.alignment = TextAnchor.LowerRight;
        skipText.horizontalOverflow = HorizontalWrapMode.Overflow;
        skipText.verticalOverflow = VerticalWrapMode.Overflow;
        
        // Position the text
        RectTransform rectTransform = skipText.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(1, 0);
        rectTransform.anchorMax = new Vector2(1, 0);
        rectTransform.pivot = new Vector2(1, 0);
        rectTransform.anchoredPosition = new Vector2(-20, 20);
        rectTransform.sizeDelta = new Vector2(200, 30);

        // Make sure text is initially hidden
        textObj.SetActive(false);
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        SceneManager.LoadScene(nextSceneName);
    }

    private void Update()
    {
        // Check for any input to show the skip text
        if (Input.anyKeyDown && !isTextFading)
        {
            StartSkipTextFadeLoop();
        }

        // Handle hold-to-skip logic
        if (Input.GetKey(KeyCode.Space))
        {
            if (!isHolding)
            {
                isHolding = true;
                holdTimer = 0f;
            }
            else
            {
                holdTimer += Time.deltaTime;
                if (holdTimer >= holdDuration)
                {
                    SceneManager.LoadScene(nextSceneName);
                }
            }
        }
        else
        {
            isHolding = false;
            holdTimer = 0f;
        }
    }

    private void StartSkipTextFadeLoop()
    {
        if (skipText != null)
        {
            // Make sure text is active
            skipText.gameObject.SetActive(true);
            
            // Stop any existing coroutine
            if (fadeLoopCoroutine != null)
            {
                StopCoroutine(fadeLoopCoroutine);
            }
            
            // Start the fade loop
            isTextFading = true;
            fadeLoopCoroutine = StartCoroutine(FadeTextLoopWithTimeout());
        }
    }

    private IEnumerator FadeTextLoopWithTimeout()
    {
        float totalElapsedTime = 0f;
        
        while (totalElapsedTime < textDisplayDuration)
        {
            // Fade in
            yield return StartCoroutine(FadeText(0f, 0.7f, fadeInDuration));
            totalElapsedTime += fadeInDuration;
            
            // Stay visible for a while
            yield return new WaitForSeconds(visibleDuration);
            totalElapsedTime += visibleDuration;
            
            // Fade out
            yield return StartCoroutine(FadeText(0.7f, 0f, fadeOutDuration));
            totalElapsedTime += fadeOutDuration;
            
            // Stay hidden for a while
            yield return new WaitForSeconds(hiddenDuration);
            totalElapsedTime += hiddenDuration;
        }
        
        // Hide the text when we're done
        skipText.gameObject.SetActive(false);
        isTextFading = false;
    }

    private IEnumerator FadeText(float startAlpha, float endAlpha, float duration)
    {
        float elapsedTime = 0f;
        
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float normalizedTime = Mathf.Clamp01(elapsedTime / duration);
            float currentAlpha = Mathf.Lerp(startAlpha, endAlpha, normalizedTime);
            
            if (skipText != null)
            {
                Color newColor = skipText.color;
                newColor.a = currentAlpha;
                skipText.color = newColor;
            }
            
            yield return null;
        }
    }
}
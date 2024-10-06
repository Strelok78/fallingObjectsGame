using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Video;

public class GameController : MonoBehaviour
{
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _totalScoreText;
    [SerializeField] private Button _restartButton; // Reference to the Start Button
    [SerializeField] private Button _startButton; // Reference to the Start Button
    [SerializeField] private Button _menuButton;
    [SerializeField] private Button _resumeButton;
    [SerializeField] private Button _exitButton;
    [SerializeField] private GameObject _panel;
    [SerializeField] private VideoClip _gameEndVideo; // Reference to the video clip
    [SerializeField] private RawImage _videoFrame; // Reference to the RawImage for displaying the video

    public Canvas GameMenuCanvas;
    private VideoPlayer videoPlayer;
    private RenderTexture renderTexture;

    private void Awake()
    {
        // Ensure PlayerController events are set up
        if (_playerController != null)
        {
            _playerController.PlayerDied += OnPlayerDie;
        }

        // Initialize UI canvase and its elements
        GameMenuCanvas.gameObject.SetActive(true);
        _startButton.gameObject.SetActive(true);
        _scoreText.gameObject.SetActive(true);
        _panel.gameObject.SetActive(true);
        _restartButton.gameObject.SetActive(false);
        _menuButton.gameObject.SetActive(false);
        _resumeButton.gameObject.SetActive(false);
        _exitButton.gameObject.SetActive(false);
        _totalScoreText.gameObject.SetActive(false);
        _videoFrame.gameObject.SetActive(false); // Hide the RawImage initially

        // Pause the game initially
        Time.timeScale = 0;
    }

    private void Start()
    {
        videoPlayer = gameObject.AddComponent<VideoPlayer>();
        videoPlayer.clip = _gameEndVideo;
        videoPlayer.renderMode = VideoRenderMode.RenderTexture;
        videoPlayer.isLooping = false; // Do not loop the video
        videoPlayer.Stop(); // Stop the video initially
        videoPlayer.loopPointReached += OnVideoEnded; // Subscribe to the loopPointReached event
        videoPlayer.Prepare();

        // Create a RenderTexture and assign it to the VideoPlayer
        renderTexture = new RenderTexture(Screen.width / 2, Screen.height / 2, 0);
        videoPlayer.targetTexture = renderTexture;

        // Assign the RenderTexture to the RawImage
        _videoFrame.texture = renderTexture;

        // Set the RawImage RectTransform size to match the RenderTexture
        RectTransform rectTransform = _videoFrame.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(renderTexture.width, renderTexture.height);
    }

    private void Update()
    {
        _scoreText.text = Mathf.FloorToInt(Time.timeSinceLevelLoad).ToString();
    }

    private void OnPlayerDie()
    {
        if (_playerController != null)
        {
            _playerController.PlayerDied -= OnPlayerDie;
        }

        Time.timeScale = 0;
        _scoreText.gameObject.SetActive(false);
        _menuButton.gameObject.SetActive(false);
        _panel.gameObject.SetActive(true);
        _restartButton.gameObject.SetActive(true);
        _totalScoreText.gameObject.SetActive(true);
        int score = Mathf.FloorToInt(Time.timeSinceLevelLoad);
        _totalScoreText.text = "Your total score: " + score;

        // Play the video when the player dies
        var playback = StartCoroutine(PlayVideoAndPause());
    }

    private IEnumerator PlayVideoAndPause()
    {
        Debug.Log("Starting video playback coroutine...");
        _videoFrame.gameObject.SetActive(true); // Show the RawImage
        //yield return new WaitForEndOfFrame(); // Ensure the frame is rendered before playing the video

        videoPlayer.enabled = true; // Enable the VideoPlayer
        videoPlayer.Play();
        Debug.Log("Starting video playback...");

        yield return new WaitForSeconds((float)videoPlayer.clip.length); // Wait for the video to finish playing
    }

    private void OnVideoEnded(VideoPlayer vp)
    {
        StopCoroutine(PlayVideoAndPause());
        Debug.Log("Video playback ended.");
        _videoFrame.gameObject.SetActive(false); // Hide the RawImage
        videoPlayer.Stop(); // Stop the VideoPlayer
    }

    public void RestartGameClicked()
    {
        Time.timeScale = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void StartGameClicked()
    {
        Debug.Log("Start is pressed");
        _startButton.gameObject.SetActive(false); // Hide the StartButton
        _panel.gameObject.SetActive(false);
        _menuButton.gameObject.SetActive(true);
        Time.timeScale = 1; // Resume the game
    }

    public void OnMenuClicked()
    {
        Time.timeScale = 0;
        _panel.gameObject.SetActive(true);
        ButtonVisibilityChange(true);
    }

    public void OnResumeClicked()
    {
        Time.timeScale = 1f;
        _panel.gameObject.SetActive(false);
        ButtonVisibilityChange(false);

        // Deactivate the video
        _videoFrame.gameObject.SetActive(false);
        videoPlayer.Stop();
    }

    public void OnExitClicked()
    {
        Time.timeScale = 0f;
        Application.Quit();
    }

    private void ButtonVisibilityChange(bool status)
    {
        _menuButton.gameObject.SetActive(!status);
        _exitButton.gameObject.SetActive(status);
        _resumeButton.gameObject.SetActive(status);
    }
}

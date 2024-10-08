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

    public Canvas GameMenuCanvas;

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

        // Pause the game initially
        PauseGame(true);
    }

    private void PauseGame (bool pause)
    {
        Time.timeScale = pause ? 0 : 1;
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

        PauseGame(true);

        _scoreText.gameObject.SetActive(false);
        _menuButton.gameObject.SetActive(false);
        _panel.gameObject.SetActive(true);
        _restartButton.gameObject.SetActive(true);
        _totalScoreText.gameObject.SetActive(true);
        int score = Mathf.FloorToInt(Time.timeSinceLevelLoad);
        _totalScoreText.text = "Your total score: " + score;
    }

    public void RestartGameClicked()
    {
        PauseGame(true);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void StartGameClicked()
    {
        _startButton.gameObject.SetActive(false); // Hide the StartButton
        _panel.gameObject.SetActive(false);
        _menuButton.gameObject.SetActive(true);

        PauseGame(false);
    }

    public void OnMenuClicked()
    {
        PauseGame(true);
        _panel.gameObject.SetActive(true);
        ButtonVisibilityChange(true);
    }

    public void OnResumeClicked()
    {
        PauseGame(false);
        _panel.gameObject.SetActive(false);
        ButtonVisibilityChange(false);
    }

    public void OnExitClicked()
    {
        PauseGame(true);
        Application.Quit();
    }

    private void ButtonVisibilityChange(bool status)
    {
        _menuButton.gameObject.SetActive(!status);
        _exitButton.gameObject.SetActive(status);
        _resumeButton.gameObject.SetActive(status);
    }
}

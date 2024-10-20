using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Serialization;
using UnityEngine.Video;

public class GameController : MonoBehaviour
{
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _totalScoreText;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _startButton; 
    [SerializeField] private Button _menuButton;
    [SerializeField] private Button _resumeButton;
    [SerializeField] private Button _exitButton;
    [SerializeField] private Button _shootButton;
    [SerializeField] private Canvas _gameMenuCanvas;
    [SerializeField] private GameObject _panel;

    private bool _isWaiting = false;
    private float _delayTimer = 0f;

    private void Awake()
    {
        if (_playerController != null)
        {
            _playerController.PlayerDied += OnPlayerDie;
        }

        _gameMenuCanvas.gameObject.SetActive(true);
        _startButton.gameObject.SetActive(true);
        _scoreText.gameObject.SetActive(true);
        _panel.gameObject.SetActive(true);
        _shootButton.gameObject.SetActive(false);
        _restartButton.gameObject.SetActive(false);
        _menuButton.gameObject.SetActive(false);
        _resumeButton.gameObject.SetActive(false);
        _exitButton.gameObject.SetActive(false);
        _totalScoreText.gameObject.SetActive(false);

        PauseGame();
    }

    private void PauseGame (bool pause = true, bool isDead = false)
    {
        if (isDead)
        {
            _isWaiting = true;
            _delayTimer = 2f;
            return;
        }
        
        Time.timeScale = pause ? 0 : 1;
    }

    private void Update()
    {
        _scoreText.text = Mathf.FloorToInt(Time.timeSinceLevelLoad).ToString();
        
        if (_isWaiting)
        {
            _delayTimer -= Time.deltaTime;
            if (_delayTimer <= 0f)
            {
                Time.timeScale = 0;
                _isWaiting = false; 
            }
        }
    }

    private void OnPlayerDie()
    {
        if (_playerController != null)
        {
            _playerController.PlayerDied -= OnPlayerDie;
        }

        PauseGame(true, true);

        _scoreText.gameObject.SetActive(false);
        _menuButton.gameObject.SetActive(false);
        _shootButton.gameObject.SetActive(false);
        _panel.gameObject.SetActive(true);
        _restartButton.gameObject.SetActive(true);
        _totalScoreText.gameObject.SetActive(true);
        int score = Mathf.FloorToInt(Time.timeSinceLevelLoad);
        _totalScoreText.text = "Your total score: " + score;
    }

    public void RestartGameClicked()
    {
        PauseGame();

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void StartGameClicked()
    {
        _startButton.gameObject.SetActive(false); 
        _panel.gameObject.SetActive(false);
        _menuButton.gameObject.SetActive(true);
        _shootButton.gameObject.SetActive(true);

        PauseGame(false);
    }

    public void OnMenuClicked()
    {
        PauseGame();
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
        PauseGame();
        Application.Quit();
    }

    private void ButtonVisibilityChange(bool status)
    {
        _menuButton.gameObject.SetActive(!status);
        _shootButton.gameObject.SetActive(!status);
        _exitButton.gameObject.SetActive(status);
        _resumeButton.gameObject.SetActive(status);
    }
}

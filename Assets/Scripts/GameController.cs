using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI; // Required for Button

public class GameController : MonoBehaviour
{
    [SerializeField] private PlayerController _playerController;
    public Canvas GameOverCanvas;
    public TMP_Text TimerText; // Score
    [SerializeField] public Canvas StartCanvas; // Canvas with the Start button
    [SerializeField] public Button StartButton; // Reference to the Start Button

    private void Awake()
    {
        // Ensure PlayerController events are set up
        if (_playerController != null)
        {
            _playerController.PlayerDied += OnPlayerDie;
        }

        // Initialize UI canvases
        GameOverCanvas.gameObject.SetActive(false);
        StartCanvas.gameObject.SetActive(true); // Show the StartCanvas on game start

        // Pause the game initially
        Time.timeScale = 0;

        // Set up the Start button
        if (StartButton != null)
        {
            StartButton.onClick.AddListener(StartGameClicked);
        }
        else 
        { 
            Debug.LogError("StartButton is not assigned in the inspector!");
        }
    }

    private void OnPlayerDie()
    {
        if (_playerController != null)
        {
            _playerController.PlayerDied -= OnPlayerDie;
        }

        Time.timeScale = 0;
        GameOverCanvas.gameObject.SetActive(true);
        TimerText.text = "Your total score: " + Math.Round(Time.timeSinceLevelLoad, 2);
    }

    public void RestartGameClicked()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void StartGameClicked()
    {
        StartCanvas.gameObject.SetActive(false); // Hide the StartCanvas
        Time.timeScale = 1; // Resume the game
    }
}
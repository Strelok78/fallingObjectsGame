using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject startCanvas; // Assign in inspector
    public Button startButton; // Assign in inspector

    private void Start()
    {
        if (startCanvas == null || startButton == null)
        {
            Debug.LogError("Start Canvas or Start Button is not assigned!");
            return;
        }

        // Ensure the Canvas is visible at the start
        startCanvas.SetActive(true);

        // Add a listener to the button
        startButton.onClick.AddListener(OnStartButtonClicked);
    }

    private void OnStartButtonClicked()
    {
        Debug.Log("Start Game button clicked.");

        // Hide the Canvas
        startCanvas.SetActive(false);

        // Here you can add additional logic to start the game
        StartGame();
    }

    private void StartGame()
    {
        // Add your game start logic here
        // Example: Enable player controls, start game timers, etc.

        // Debug log to confirm that StartGame() is called
        Debug.Log("Game has started.");
    }
}
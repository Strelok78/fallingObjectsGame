using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] private PlayerController _playerController;
    public Canvas GameOverCanvas;
    public TMP_Text TimerText; //score

    void Awake()
    {
        if(_playerController != null)
        {
            _playerController.PlayerDied += OnPlayerDie;
        }

        if (GameOverCanvas.gameObject.activeSelf)
        {
            GameOverCanvas.gameObject.SetActive(false);
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
}

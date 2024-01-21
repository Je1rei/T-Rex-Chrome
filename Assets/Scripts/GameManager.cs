using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    private readonly string _highScoreKeySave = "HighScore";

    [SerializeField] private float _gameSpeed = 5f;
    [SerializeField] private float _gameSpeedIncrease = 0.1f;

    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private GameObject _surprisePanel;
    [SerializeField] private Button _isDisableSupriseButton;

    [SerializeField] private Button _soundFull;
    [SerializeField] private Button _soundHalf;
    [SerializeField] private Button _soundEmpty;

    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _highScoreText;


    [SerializeField] private AudioSource _mainMusic;
    [SerializeField] private AudioSource _dieSFX;
    [SerializeField] private AudioSource _jumpSFX;

    private float _defaultVolume;

    private bool _isOver = false;
    private Player _player;
    private Spawner _spawner;

    private float _score;

    public static GameManager Instance { get; private set; }
    public float GameSpeed { get; private set; }

    private void Awake()
    {
        _defaultVolume = _mainMusic.volume;

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }

    private void OnDestroy()
    {
        if( Instance == this)
        {
            Instance = null;
        }
    }

    private void Start()
    {
        _player = FindObjectOfType<Player>();
        _spawner = FindObjectOfType<Spawner>();
        NewGame();
    }

    private void Update()
    {
        GameSpeed += _gameSpeedIncrease * Time.deltaTime;

        _score += GameSpeed * Time.deltaTime;

        _scoreText.text = Mathf.FloorToInt(_score).ToString("D5");
    }

    public void NewGame()
    {
        _isOver = false;

        Obstacle[] obstacles = FindObjectsOfType<Obstacle>();

        foreach(var obstacle in obstacles)
        {
            Destroy(obstacle.gameObject);
        }

        _score = 0f;
        GameSpeed = _gameSpeed;
        enabled = true;

        _player.gameObject.SetActive(true);
        _spawner.gameObject.SetActive(true);

        _gameOverPanel.gameObject.SetActive(false);

        UpdateHighScore();
    }

    public void GameOver() 
    {
        _isOver = true;

        _dieSFX.Play();
        GameSpeed = 0f;
        enabled = false;

        _player.gameObject.SetActive(false);
        _spawner.gameObject.SetActive(false);

        _gameOverPanel.gameObject.SetActive(true);

        UpdateHighScore();
    }

    public void JumpSound()
    {
        _jumpSFX.Play();
    }

    public bool GetIsOver()
    {
        return _isOver;
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        _pausePanel.gameObject.SetActive(true);
    }

    public void ContinueGame()
    {
        Time.timeScale = 1f;
        _pausePanel.gameObject.SetActive(false);
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }

    public void EnableSuprise()
    {
        _surprisePanel.gameObject.SetActive(true);
        _isDisableSupriseButton.gameObject.SetActive(false);
    }

    public void DisableSuprise()
    {
        _surprisePanel.gameObject.SetActive(false);
        _isDisableSupriseButton.gameObject.SetActive(true); 
    }

    public void ClickSoundButtons()
    {
        float _currentVolumeMain = _mainMusic.volume;
        float halfVolume = _defaultVolume / 2;

        if (_currentVolumeMain >= _defaultVolume)
        {
            _soundFull.gameObject.SetActive(false);
            _soundHalf.gameObject.SetActive(true);
            _soundEmpty.gameObject.SetActive(false);

            _mainMusic.volume = halfVolume;
        }
        else if (_currentVolumeMain == halfVolume)
        {
            _soundFull.gameObject.SetActive(false);
            _soundHalf.gameObject.SetActive(false);
            _soundEmpty.gameObject.SetActive(true);

            _mainMusic.volume = 0;
        }
        else if (_currentVolumeMain <= 0)
        {
            _soundFull.gameObject.SetActive(true);
            _soundHalf.gameObject.SetActive(false);
            _soundEmpty.gameObject.SetActive(false);

            _mainMusic.volume = _defaultVolume;
        }
    }

    private void UpdateHighScore()
    {
        float highScore = PlayerPrefs.GetFloat(_highScoreKeySave, 0);

        if(_score > highScore)
        {
            highScore = _score;
            PlayerPrefs.SetFloat(_highScoreKeySave, highScore);
        }

        _highScoreText.text = Mathf.FloorToInt(highScore).ToString("D5");
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public event EventHandler OnStateChanged;
    public event EventHandler OnPaused;
    public event EventHandler OnUnpaused;

    public static GameManager Instance { get; private set; }

    private enum State
    {
        WaitingToStart,
        StartCountdown,
        GamePlaying,
        GameOver
    }

    [SerializeField] private float gameActiveTimerMax;

    private State state;
    private float startCountdownTimer = 3f;
    private float gameActiveTimer;
    private bool isGamePaused = false;


    private void Awake()
    {
        Instance = this;
        gameActiveTimer = gameActiveTimerMax;
    }

    private void Start()
    {
        state = State.WaitingToStart;

        GameplayInput.Instance.OnPauseAction += GameplayInput_OnPauseAction;
        GameplayInput.Instance.OnInteractAction += GameplayInput_OnInteractAction;
    }

    private void Update()
    {
        switch (state)
        {
            case State.WaitingToStart:
                break;
            case State.StartCountdown:
                startCountdownTimer -= Time.deltaTime;
                if(startCountdownTimer <= 0f)
                {
                    state = State.GamePlaying;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GamePlaying:
                gameActiveTimer -= Time.deltaTime;
                if(gameActiveTimer <= 0f)
                {
                    state = State.GameOver;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GameOver:
                break;
        }
    }

    private void GameplayInput_OnInteractAction(object sender, EventArgs e)
    {
        if(state == State.WaitingToStart)
        {
            state = State.StartCountdown;
            OnStateChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    private void GameplayInput_OnPauseAction(object sender, EventArgs e)
    {
        TogglePauseGame();
    }

    public void TogglePauseGame()
    {
        isGamePaused = !isGamePaused;
        if (isGamePaused)
        {
            Time.timeScale = 0f;
            OnPaused?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            Time.timeScale = 1f;
            OnUnpaused?.Invoke(this, EventArgs.Empty);
        }
    }

    public bool IsGamePlaying()
    {
        return state == State.GamePlaying;
    }

    public bool IsStartCountdown()
    {
        return state == State.StartCountdown;
    }

    public bool IsGameOver()
    {
        return state == State.GameOver;
    }

    public float GetStartCountdown()
    {
        return startCountdownTimer;
    }

    public float GetGameActiveTimerNormalized()
    {
        return 1 - gameActiveTimer / gameActiveTimerMax;
    }
}

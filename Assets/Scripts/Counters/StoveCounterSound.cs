using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterSound : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;

    private AudioSource audioSource;
    private float playWarningThreshold = .5f;
    private float playWarningInterval = .2f;
    private float playWarningTimer;
    bool playWarningSound;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
        stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;
    }

    private void Update()
    {
        if (playWarningSound)
        {
            playWarningTimer += Time.deltaTime;
            if (playWarningTimer >= playWarningInterval)
            {
                playWarningTimer = 0f;

                SoundManager.Instance.PlayWarningSound(stoveCounter.transform.position);
            }
        }
        
    }

    private void StoveCounter_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        playWarningSound = stoveCounter.IsFried() && e.progressNormalized >= playWarningThreshold;
    }

    private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e)
    {
        if(e.state == StoveCounter.StoveState.Frying || e.state == StoveCounter.StoveState.Fried)
        {
            audioSource.Play();
        }
        else
        {
            audioSource.Pause();
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterVisual : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;
    [SerializeField] private GameObject stoveOn;
    [SerializeField] private GameObject stoveParticles;

    // Start is called before the first frame update
    void Start()
    {
        stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
    }

    private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e)
    {
        bool showEffects = e.state == StoveCounter.StoveState.Frying || e.state == StoveCounter.StoveState.Fried;
        stoveOn.SetActive(showEffects);
        stoveParticles.SetActive(showEffects);
    }
}

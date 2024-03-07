using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class GameStartCountdownUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countdownText;

    private Animator animator;
    private int prevCountdown;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;

        Hide();
    }

    private void Update()
    {
        int currCountdown = Mathf.CeilToInt(GameManager.Instance.GetStartCountdown());

        countdownText.text = currCountdown.ToString();

        if(currCountdown != prevCountdown)
        {
            prevCountdown = currCountdown;
            animator.SetTrigger(AnimationStrings.NumberPopUp);
            SoundManager.Instance.PlayCountdown();
        }


    }

    private void GameManager_OnStateChanged(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsStartCountdown())
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}

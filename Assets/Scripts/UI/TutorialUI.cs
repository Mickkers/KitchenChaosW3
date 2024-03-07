using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI bindText_Kb_MoveUp;
    [SerializeField] private TextMeshProUGUI bindText_Kb_MoveDown;
    [SerializeField] private TextMeshProUGUI bindText_Kb_MoveLeft;
    [SerializeField] private TextMeshProUGUI bindText_Kb_MoveRight;
    [SerializeField] private TextMeshProUGUI bindText_Kb_Interact;
    [SerializeField] private TextMeshProUGUI bindText_Kb_InteractAlt;
    [SerializeField] private TextMeshProUGUI bindText_Kb_Pause;
    [SerializeField] private TextMeshProUGUI bindText_Gamepad_Interact;
    [SerializeField] private TextMeshProUGUI bindText_Gamepad_InteractAlt;
    [SerializeField] private TextMeshProUGUI bindText_Gamepad_Pause;


    private void Start()
    {
        GameplayInput.Instance.onKeybindChange += GameplayInput_onKeybindChange;

        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
        UpdateVisuals();

        Show();
    }

    private void GameManager_OnStateChanged(object sender, EventArgs e)
    {
        if (GameManager.Instance.IsStartCountdown())
        {
            Hide();
        }
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    private void GameplayInput_onKeybindChange(object sender, EventArgs e)
    {
        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        bindText_Kb_MoveUp.text = GameplayInput.Instance.GetBindingText(GameplayInput.Bindings.Move_Up);
        bindText_Kb_MoveDown.text = GameplayInput.Instance.GetBindingText(GameplayInput.Bindings.Move_Down);
        bindText_Kb_MoveLeft.text = GameplayInput.Instance.GetBindingText(GameplayInput.Bindings.Move_Left);
        bindText_Kb_MoveRight.text = GameplayInput.Instance.GetBindingText(GameplayInput.Bindings.Move_Right);
        bindText_Kb_Interact.text = GameplayInput.Instance.GetBindingText(GameplayInput.Bindings.Interact);
        bindText_Kb_InteractAlt.text = GameplayInput.Instance.GetBindingText(GameplayInput.Bindings.InteractAlt);
        bindText_Kb_Pause.text = GameplayInput.Instance.GetBindingText(GameplayInput.Bindings.Pause);
        bindText_Gamepad_Interact.text = GameplayInput.Instance.GetBindingText(GameplayInput.Bindings.Gamepad_Interact);
        bindText_Gamepad_InteractAlt.text = GameplayInput.Instance.GetBindingText(GameplayInput.Bindings.Gamepad_InteractAlt);
        bindText_Gamepad_Pause.text = GameplayInput.Instance.GetBindingText(GameplayInput.Bindings.Gamepad_Pause);
    }
}

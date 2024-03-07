using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionsUI : MonoBehaviour
{
    public static OptionsUI Instance { get; private set; }

    [SerializeField] private Button musicButton;
    [SerializeField] private Button effectsButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private TextMeshProUGUI musicText;
    [SerializeField] private TextMeshProUGUI effectsText;

    [SerializeField] private Button rebindButton_MoveUp;
    [SerializeField] private TextMeshProUGUI rebindText_MoveUp;
    [SerializeField] private Button rebindButton_MoveDown;
    [SerializeField] private TextMeshProUGUI rebindText_MoveDown;
    [SerializeField] private Button rebindButton_MoveLeft;
    [SerializeField] private TextMeshProUGUI rebindText_MoveLeft;
    [SerializeField] private Button rebindButton_MoveRight;
    [SerializeField] private TextMeshProUGUI rebindText_MoveRight;
    [SerializeField] private Button rebindButton_Interact;
    [SerializeField] private TextMeshProUGUI rebindText_Interact;
    [SerializeField] private Button rebindButton_InteractAlt;
    [SerializeField] private TextMeshProUGUI rebindText_InteractAlt;
    [SerializeField] private Button rebindButton_Pause;
    [SerializeField] private TextMeshProUGUI rebindText_Pause;
    [SerializeField] private Button rebindButton_Gamepad_Interact;
    [SerializeField] private TextMeshProUGUI rebindText_Gamepad_Interact;
    [SerializeField] private Button rebindButton_Gamepad_InteractAlt;
    [SerializeField] private TextMeshProUGUI rebindText_Gamepad_InteractAlt;
    [SerializeField] private Button rebindButton_Gamepad_Pause;
    [SerializeField] private TextMeshProUGUI rebindText_Gamepad_Pause;


    [SerializeField] private GameObject rebindPrompt;
    private Action onCloseAction;

    private void Awake()
    {
        Instance = this;

        musicButton.onClick.AddListener(() => 
        {
            MusicManager.Instance.ChangeVolume();
            UpdateVisuals();
        });
        effectsButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.ChangeVolume();
            UpdateVisuals();
        });
        closeButton.onClick.AddListener(() =>
        {
            Hide();
            onCloseAction();
        });

        rebindButton_MoveUp.onClick.AddListener(() => { RebindKey(GameplayInput.Bindings.Move_Up); });
        rebindButton_MoveDown.onClick.AddListener(() => { RebindKey(GameplayInput.Bindings.Move_Down); });
        rebindButton_MoveLeft.onClick.AddListener(() => { RebindKey(GameplayInput.Bindings.Move_Left); });
        rebindButton_MoveRight.onClick.AddListener(() => { RebindKey(GameplayInput.Bindings.Move_Right); });
        rebindButton_Interact.onClick.AddListener(() => { RebindKey(GameplayInput.Bindings.Interact); });
        rebindButton_InteractAlt.onClick.AddListener(() => { RebindKey(GameplayInput.Bindings.InteractAlt); });
        rebindButton_Pause.onClick.AddListener(() => { RebindKey(GameplayInput.Bindings.Pause); });
        rebindButton_Gamepad_Interact.onClick.AddListener(() => { RebindKey(GameplayInput.Bindings.Gamepad_Interact); });
        rebindButton_Gamepad_InteractAlt.onClick.AddListener(() => { RebindKey(GameplayInput.Bindings.Gamepad_InteractAlt); });
        rebindButton_Gamepad_Pause.onClick.AddListener(() => { RebindKey(GameplayInput.Bindings.Gamepad_Pause); });

        

        HideRebindPrompt();
        Hide();
    }

    private void Start()
    {
        GameplayInput.Instance.OnPauseAction += GameplayInput_OnPauseAction;

        UpdateVisuals();
    }

    private void GameplayInput_OnPauseAction(object sender, EventArgs e)
    {
        Hide();
    }

    private void UpdateVisuals()
    {
        musicText.text = "Music Volume: " + Mathf.Round(MusicManager.Instance.GetVolume() * 10);
        effectsText.text = "Effects Volume: " + Mathf.Round(SoundManager.Instance.GetVolume() * 10);

        rebindText_MoveUp.text = GameplayInput.Instance.GetBindingText(GameplayInput.Bindings.Move_Up);
        rebindText_MoveDown.text = GameplayInput.Instance.GetBindingText(GameplayInput.Bindings.Move_Down);
        rebindText_MoveLeft.text = GameplayInput.Instance.GetBindingText(GameplayInput.Bindings.Move_Left);
        rebindText_MoveRight.text = GameplayInput.Instance.GetBindingText(GameplayInput.Bindings.Move_Right);
        rebindText_Interact.text = GameplayInput.Instance.GetBindingText(GameplayInput.Bindings.Interact);
        rebindText_InteractAlt.text = GameplayInput.Instance.GetBindingText(GameplayInput.Bindings.InteractAlt);
        rebindText_Pause.text = GameplayInput.Instance.GetBindingText(GameplayInput.Bindings.Pause);
        rebindText_Gamepad_Interact.text = GameplayInput.Instance.GetBindingText(GameplayInput.Bindings.Gamepad_Interact);
        rebindText_Gamepad_InteractAlt.text = GameplayInput.Instance.GetBindingText(GameplayInput.Bindings.Gamepad_InteractAlt);
        rebindText_Gamepad_Pause.text = GameplayInput.Instance.GetBindingText(GameplayInput.Bindings.Gamepad_Pause);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show(Action onCloseAction)
    {
        this.onCloseAction = onCloseAction;
        gameObject.SetActive(true);
        closeButton.Select();
    }

    private void HideRebindPrompt()
    {
        rebindPrompt.SetActive(false);
    }

    public void ShowRebindPrompt()
    {
        rebindPrompt.SetActive(true);
    }

    private void RebindKey(GameplayInput.Bindings binding)
    {
        ShowRebindPrompt();
        GameplayInput.Instance.RebindBinding(binding, () =>
        {
            HideRebindPrompt();
            UpdateVisuals();
        });
    }
}

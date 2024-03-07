using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameplayInput : MonoBehaviour
{
    public static GameplayInput Instance { get; private set; }

    private const string PLAYER_PREFS_BINDINGS = "InputBindings";

    public enum Bindings
    {
        Move_Up,
        Move_Down,
        Move_Left,
        Move_Right,
        Interact,
        InteractAlt,
        Pause,
        Gamepad_Interact,
        Gamepad_InteractAlt,
        Gamepad_Pause
    }

    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAltAction;
    public event EventHandler OnPauseAction;
    public event EventHandler onKeybindChange;

    private PlayerInput playerInput;
    public PlayerInput.GameplayActions gameplay;


    private void Awake()
    {
        Instance = this;

        playerInput = new PlayerInput();
        gameplay = playerInput.Gameplay;

        if (PlayerPrefs.HasKey(PLAYER_PREFS_BINDINGS))
        {
            playerInput.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PREFS_BINDINGS));
        }

        gameplay.Interact.performed += Interact_performed;
        gameplay.InteractAlt.performed += InteractAlt_performed;
        gameplay.Pause.performed += Pause_performed;
    }

    private void OnDestroy()
    {
        gameplay.Interact.performed -= Interact_performed;
        gameplay.InteractAlt.performed -= InteractAlt_performed;
        gameplay.Pause.performed -= Pause_performed;

        playerInput.Dispose();
    }

    private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPauseAction?.Invoke(this, EventArgs.Empty);
    }

    private void InteractAlt_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAltAction?.Invoke(this, EventArgs.Empty);
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    private void OnEnable()
    {
        gameplay.Enable();
    }

    private void OnDisable()
    {
        gameplay.Disable();
    }

    public Vector2 GetMovementDirection()
    {
        return gameplay.Move.ReadValue<Vector2>();
    }

    public string GetBindingText(Bindings binding)
    {
        switch (binding)
        {
            default:
            case Bindings.Move_Up:
                return gameplay.Move.bindings[1].ToDisplayString();
            case Bindings.Move_Down:
                return gameplay.Move.bindings[2].ToDisplayString();
            case Bindings.Move_Left:
                return gameplay.Move.bindings[3].ToDisplayString();
            case Bindings.Move_Right:
                return gameplay.Move.bindings[4].ToDisplayString();
            case Bindings.Interact:
                return gameplay.Interact.bindings[0].ToDisplayString();
            case Bindings.InteractAlt:
                return gameplay.InteractAlt.bindings[0].ToDisplayString();
            case Bindings.Pause:
                return gameplay.Pause.bindings[0].ToDisplayString();
            case Bindings.Gamepad_Interact:
                return gameplay.Interact.bindings[1].ToDisplayString();
            case Bindings.Gamepad_InteractAlt:
                return gameplay.InteractAlt.bindings[1].ToDisplayString();
            case Bindings.Gamepad_Pause:
                return gameplay.Pause.bindings[1].ToDisplayString();
        }
    }

    public void RebindBinding(Bindings binding, Action onActionRebound)
    {
        gameplay.Disable();

        InputAction inputAction;
        int bindIndex;

        switch (binding)
        {
            default:
            case Bindings.Move_Up:
                inputAction = gameplay.Move;
                bindIndex = 1;
                break;
            case Bindings.Move_Down:
                inputAction = gameplay.Move;
                bindIndex = 2;
                break;
            case Bindings.Move_Left:
                inputAction = gameplay.Move;
                bindIndex = 3;
                break;
            case Bindings.Move_Right:
                inputAction = gameplay.Move;
                bindIndex = 4;
                break;
            case Bindings.Interact:
                inputAction = gameplay.Interact;
                bindIndex = 0;
                break;
            case Bindings.InteractAlt:
                inputAction = gameplay.InteractAlt;
                bindIndex = 0;
                break;
            case Bindings.Pause:
                inputAction = gameplay.Pause;
                bindIndex = 0;
                break;
            case Bindings.Gamepad_Interact:
                inputAction = gameplay.Interact;
                bindIndex = 1;
                break;
            case Bindings.Gamepad_InteractAlt:
                inputAction = gameplay.InteractAlt;
                bindIndex = 1;
                break;
            case Bindings.Gamepad_Pause:
                inputAction = gameplay.Pause;
                bindIndex = 1;
                break;
        }

        inputAction.PerformInteractiveRebinding(bindIndex).OnComplete(callback =>
        {
            callback.Dispose();
            gameplay.Enable();
            onActionRebound();
            onKeybindChange?.Invoke(this, EventArgs.Empty);
        }).Start();

        PlayerPrefs.SetString(PLAYER_PREFS_BINDINGS, playerInput.SaveBindingOverridesAsJson());
        PlayerPrefs.Save();
    }
}

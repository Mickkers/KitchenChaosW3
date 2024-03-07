using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectParent
{
    public static Player Instance { get; private set; }

    public event EventHandler OnObjectPickup;

    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }

    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float interactDistance;
    [SerializeField] private LayerMask counterLayerMask;
    [SerializeField] private Transform kitchenObjectHoldPoint;


    public bool IsWalking { get; private set; }

    private Vector2 inputDir;
    private Vector3 moveDir;
    private Vector3 interactDir;
    private float moveDistance;
    private BaseCounter selectedCounter;
    private KitchenObject kitchenObject;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;
    }


    void Start()
    {
        GameplayInput.Instance.OnInteractAction += GameplayInput_OnInteractAction;
        GameplayInput.Instance.OnInteractAltAction += GameplayInput_OnInteractAltAction;
    }

    private void GameplayInput_OnInteractAltAction(object sender, EventArgs e)
    {
        if (!GameManager.Instance.IsGamePlaying()) return;

        if (selectedCounter != null)
        {
            selectedCounter.InteractAlt(this);
        }
    }

    private void GameplayInput_OnInteractAction(object sender, EventArgs e)
    {
        if (!GameManager.Instance.IsGamePlaying()) return;

        if (selectedCounter != null)
        {
            selectedCounter.Interact(this);
        }
    }

    void Update()
    {
        GetInputDir();
        Movement();
        Interaction();
    }

    private void GetInputDir()
    {
        inputDir = GameplayInput.Instance.GetMovementDirection().normalized;
    }

    private void Interaction()
    {
        if(inputDir != Vector2.zero)
        {
            interactDir = GetDirection(true, true);
        }
        
        if(Physics.Raycast(transform.position, interactDir, out RaycastHit hit, interactDistance, counterLayerMask))
        {
            if (hit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                if(selectedCounter != baseCounter)
                {
                    SetSelectedCounter(baseCounter);
                }
            }
            else
            {
                SetSelectedCounter(null);
            }
        }
        else
        {
            SetSelectedCounter(null);
        }
    }

    private void SetSelectedCounter(BaseCounter baseCounter)
    {
        selectedCounter = baseCounter;
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        {
            selectedCounter = selectedCounter
        });
    }

    private void Movement()
    {
        moveDistance = moveSpeed * Time.deltaTime;
        if (CanMove(true, true))
        {
            moveDir = GetDirection(true, true).normalized;
        }
        else if (CanMove(true, false) && Mathf.Abs(inputDir.x) >= 0.5f)
        {
            moveDir = GetDirection(true, false).normalized;
        }
        else if (CanMove(false, true) && Mathf.Abs(inputDir.y) >= 0.5f)
        {
            moveDir = GetDirection(false, true).normalized;
        }
        else
        {
            moveDir = GetDirection(false, false).normalized;
        }

        transform.position += moveDistance* moveDir;

        IsWalking = moveDir != Vector3.zero;

        transform.forward = Vector3.Slerp(transform.forward, GetDirection(true, true), Time.deltaTime * rotateSpeed);
    }

    private bool CanMove(bool x, bool z)
    {
        float playerRadius = 0.7f;
        float playerHeight = 2f;

        return !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, GetDirection(x, z), moveDistance);
    }

    private Vector3 GetDirection(bool x, bool z)
    {
        if(x && z)
        {
            return new Vector3(inputDir.x, 0f, inputDir.y);
        }
        else if (x && !z)
        {
            return new Vector3(inputDir.x, 0f, 0f);
        }
        else if (z && !x)
        {
            return new Vector3(0f, 0f, inputDir.y);
        }
        return Vector3.zero;
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return kitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
        if (kitchenObject != null)
        {
            OnObjectPickup?.Invoke(this, EventArgs.Empty);
        }
    }

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
}

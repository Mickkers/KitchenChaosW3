using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounter : BaseCounter, IHasProgress
{
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public class OnStateChangedEventArgs : EventArgs
    {
        public StoveState state;
    }
    
    public enum StoveState
    {
        Idle,
        Frying,
        Fried,
        Burnt
    }


    [SerializeField] private FryingRecipeSO[] fryingRecipeArray;
    [SerializeField] private BurningRecipeSO[] burningRecipeArray;

    private StoveState state;

    private float fryingTimer;
    private FryingRecipeSO fryingRecipe;
    private float burningTimer;
    private BurningRecipeSO burningRecipe;

    private void Start()
    {
        state = StoveState.Idle;
    }

    private void Update()
    {
        if (HasKitchenObject())
        {
            switch (state)
            {
                case StoveState.Idle:
                    break;
                case StoveState.Frying:
                    fryingTimer += Time.deltaTime;
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = fryingTimer / fryingRecipe.fryingTimerMax
                    });
                    if(fryingTimer >= fryingRecipe.fryingTimerMax)
                    {
                        GetKitchenObejct().DestroySelf();
                        KitchenObject.SpawnKitchenObject(fryingRecipe.outputKitchenObjectSO, this);
                        state = StoveState.Fried;
                        burningRecipe = GetBurningRecipe(GetKitchenObejct().GetKitchenObjectSO());
                        burningTimer = 0f;
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            state = state
                        });
                    }
                    break;
                case StoveState.Fried:
                    burningTimer += Time.deltaTime;
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = burningTimer / burningRecipe.burningTimerMax
                    });
                    if (burningTimer >= burningRecipe.burningTimerMax)
                    {
                        GetKitchenObejct().DestroySelf();
                        KitchenObject.SpawnKitchenObject(burningRecipe.outputKitchenObjectSO, this);
                        state = StoveState.Burnt;
                        burningTimer = 0f;
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            state = state
                        });
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                        {
                            progressNormalized = 0f
                        });
                    }
                    break;
                case StoveState.Burnt:
                    break;
            }
        }
    }

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            if (player.HasKitchenObject())
            {
                if (HasRecipe(player.GetKitchenObejct().GetKitchenObjectSO()))
                {
                    player.GetKitchenObejct().SetKitchenObjectParent(this);
                    fryingRecipe = GetFryingRecipe(GetKitchenObejct().GetKitchenObjectSO());

                    state = StoveState.Frying;
                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                    {
                        state = state
                    });
                    fryingTimer = 0f;
                }
            }
        }
        else
        {
            if (player.HasKitchenObject())
            {
                if (player.GetKitchenObejct().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    if (plateKitchenObject.TryAddIngredients(GetKitchenObejct().GetKitchenObjectSO()))
                    {
                        GetKitchenObejct().DestroySelf();
                    }
                }

                state = StoveState.Idle;
                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                {
                    state = state
                });
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    progressNormalized = 0f
                });
            }
            else
            {
                GetKitchenObejct().SetKitchenObjectParent(player);

                state = StoveState.Idle;
                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                {
                    state = state
                });
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    progressNormalized = 0f
                });
            }
        }
    }

    private bool HasRecipe(KitchenObjectSO input)
    {
        FryingRecipeSO recipe = GetFryingRecipe(input);
        return recipe != null;
    }

    private KitchenObjectSO GetOutput(KitchenObjectSO input)
    {
        FryingRecipeSO recipe = GetFryingRecipe(input);
        if (recipe != null)
        {
            return recipe.outputKitchenObjectSO;
        }
        else
        {
            return null;
        }
    }

    private FryingRecipeSO GetFryingRecipe(KitchenObjectSO input)
    {
        foreach (FryingRecipeSO fryingRecipe in fryingRecipeArray)
        {
            if (fryingRecipe.inputKitchenObjectSO == input)
            {
                return fryingRecipe;
            }
        }
        return null;
    }

    private BurningRecipeSO GetBurningRecipe(KitchenObjectSO input)
    {
        foreach (BurningRecipeSO burningRecipe in burningRecipeArray)
        {
            if (burningRecipe.inputKitchenObjectSO == input)
            {
                return burningRecipe;
            }
        }
        return null;
    }

    public bool IsFried()
    {
        return state == StoveState.Fried;
    }
}

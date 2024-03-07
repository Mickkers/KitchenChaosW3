using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter, IHasProgress
{
    public static event EventHandler OnAnyCut;
    new public static void ResetStaticData()
    {
        OnAnyCut = null;
    }

    public event EventHandler OnCut;
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    [SerializeField] private CuttingRecipeSO[] cuttingRecipeArray;

    private int cuttingProgress;

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            if (player.HasKitchenObject())
            {
                if (HasRecipe(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    cuttingProgress = 0;

                    CuttingRecipeSO recipe = GetCuttingRecipe(GetKitchenObject().GetKitchenObjectSO());
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = (float)cuttingProgress / (float)recipe.cuttingProgressMax
                    });
                }
            }
        }
        else
        {
            if (player.HasKitchenObject())
            {
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    if (plateKitchenObject.TryAddIngredients(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelf();
                    }
                }
            }
            else
            {
                GetKitchenObject().SetKitchenObjectParent(player);

            }
        }
    }

    public override void InteractAlt(Player player)
    {
        if (!HasKitchenObject() || !HasRecipe(GetKitchenObject().GetKitchenObjectSO()))
        {
            return;
        }
        CuttingRecipeSO recipe = GetCuttingRecipe(GetKitchenObject().GetKitchenObjectSO());

        cuttingProgress++;
        OnCut?.Invoke(this, EventArgs.Empty);

        OnAnyCut?.Invoke(this, EventArgs.Empty);

        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
        {
            progressNormalized = (float)cuttingProgress / (float)recipe.cuttingProgressMax
        });

        if (cuttingProgress >= recipe.cuttingProgressMax)
        {
            KitchenObjectSO output = GetOutput(GetKitchenObject().GetKitchenObjectSO());

            GetKitchenObject().DestroySelf();

            KitchenObject.SpawnKitchenObject(output, this);
        }
    }

    private bool HasRecipe(KitchenObjectSO input)
    {
        CuttingRecipeSO recipe = GetCuttingRecipe(input);
        return recipe != null;
    }

    private KitchenObjectSO GetOutput(KitchenObjectSO input)
    {
        CuttingRecipeSO recipe = GetCuttingRecipe(input);
        if(recipe != null)
        {
            return recipe.outputKitchenObjectSO;
        }
        else
        {
            return null;
        }
    }

    private CuttingRecipeSO GetCuttingRecipe(KitchenObjectSO input)
    {
        foreach (CuttingRecipeSO cuttingRecipe in cuttingRecipeArray)
        {
            if (cuttingRecipe.inputKitchenObjectSO == input)
            {
                return cuttingRecipe;
            }
        }
        return null;
    }
}

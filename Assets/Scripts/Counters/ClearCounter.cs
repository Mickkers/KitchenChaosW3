using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter
{
    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            if (player.HasKitchenObject())
            {
                player.GetKitchenObejct().SetKitchenObjectParent(this);
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
                else
                {
                    if (GetKitchenObejct().TryGetPlate(out plateKitchenObject))
                    {
                        if (plateKitchenObject.TryAddIngredients(player.GetKitchenObejct().GetKitchenObjectSO()))
                        {
                            player.GetKitchenObejct().DestroySelf();
                        }
                    }
                }
            }
            else
            {
                GetKitchenObejct().SetKitchenObjectParent(player);
            }
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeCompleted;
    public event EventHandler OnRecipeFail;
    public event EventHandler OnRecipeSuccess;

    public static DeliveryManager Instance { get; private set; }

    [SerializeField] private RecipeListSO recipeListSO;
    [SerializeField] private float spawnRecipeInterval;
    [SerializeField] private int orderAmountMax;

    private List<RecipeSO> recipeQueueList;
    private float spawnRecipeTimer;
    private int score;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        recipeQueueList = new List<RecipeSO>();

        score = 0;
    }

    private void Update()
    {
        spawnRecipeTimer += Time.deltaTime;
        if(spawnRecipeTimer >= spawnRecipeInterval)
        {
            spawnRecipeTimer = 0f;

            if(GameManager.Instance.IsGamePlaying() && recipeQueueList.Count < orderAmountMax)
            {
                RecipeSO newOrder = recipeListSO.recipeSOList[UnityEngine.Random.Range(0, recipeListSO.recipeSOList.Count)];
                recipeQueueList.Add(newOrder);

                OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        for(int i = 0; i < recipeQueueList.Count; i++)
        {
            RecipeSO recipe = recipeQueueList[i];

            if(recipe.kitchenObjectSOList.Count == plateKitchenObject.GetKitchenObjectSOList().Count)
            {
                bool plateMatchesRecipe = true;
                foreach(KitchenObjectSO recipeKitchenObjectSO in recipe.kitchenObjectSOList)
                {
                    bool ingredientFound = false;
                    foreach(KitchenObjectSO plateKitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList())
                    {
                        if(plateKitchenObjectSO == recipeKitchenObjectSO)
                        {
                            ingredientFound = true;
                            break;
                        }
                    }

                    if (!ingredientFound)
                    {
                        plateMatchesRecipe = false;
                    }
                }

                if (plateMatchesRecipe)
                {
                    recipeQueueList.RemoveAt(i);
                    score++;
                    OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
                    OnRecipeSuccess?.Invoke(this, EventArgs.Empty);
                    return;
                }
            }
        }
        OnRecipeFail?.Invoke(this, EventArgs.Empty);
    }

    public List<RecipeSO> GetOrderQueueList()
    {
        return recipeQueueList;
    }

    public int GetScore()
    {
        return score;
    }
}

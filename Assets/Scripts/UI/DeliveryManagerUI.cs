using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManagerUI : MonoBehaviour
{
    [SerializeField] private Transform container;
    [SerializeField] private Transform recipeTemplate;

    private void Awake()
    {
        recipeTemplate.gameObject.SetActive(false);
    }

    private void Start()
    {
        DeliveryManager.Instance.OnRecipeSpawned += DeliveryManager_OnOrderSpawned;
        DeliveryManager.Instance.OnRecipeCompleted += DeliveryManager_OnOrderCompleted;

        UpdateVisuals();
    }

    private void DeliveryManager_OnOrderCompleted(object sender, System.EventArgs e)
    {
        UpdateVisuals();
    }

    private void DeliveryManager_OnOrderSpawned(object sender, System.EventArgs e)
    {
        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        foreach(Transform child in container)
        {
            if (child == recipeTemplate) continue;
            Destroy(child.gameObject);
        }

        foreach(RecipeSO recipe in DeliveryManager.Instance.GetOrderQueueList())
        {
            Transform orderTransform = Instantiate(recipeTemplate, container);
            orderTransform.gameObject.SetActive(true);
            orderTransform.GetComponent<DeliveryManagerSingleUI>().SetRecpiceSO(recipe);
        }
    }  
}

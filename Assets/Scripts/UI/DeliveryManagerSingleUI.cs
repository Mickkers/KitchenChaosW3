using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DeliveryManagerSingleUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recipeNameText;
    [SerializeField] private Transform iconContainer;
    [SerializeField] private Transform iconTemplate;

    private void Awake()
    {
        iconTemplate.gameObject.SetActive(false);
    }

    public void SetRecpiceSO(RecipeSO recipe)
    {
        recipeNameText.text = recipe.recipeName;

        foreach (Transform child in iconContainer)
        {
            if (child == iconTemplate) continue;
            Destroy(child.gameObject);
        }

        foreach(KitchenObjectSO ingridient in recipe.kitchenObjectSOList)
        {
            Transform ingridientIcon = Instantiate(iconTemplate, iconContainer);
            ingridientIcon.gameObject.SetActive(true);

            ingridientIcon.GetComponent<Image>().sprite = ingridient.sprite;
        }
    }
}

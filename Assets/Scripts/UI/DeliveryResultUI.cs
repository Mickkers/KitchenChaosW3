using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DeliveryResultUI : MonoBehaviour
{
    private Animator animator;

    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI resultText;

    [SerializeField] private Color successColor;
    [SerializeField] private Color failColor;

    [SerializeField] private Sprite successIcon;
    [SerializeField] private Sprite failIcon;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        DeliveryManager.Instance.OnRecipeSuccess += DeliveryManager_OnRecipeSuccess;
        DeliveryManager.Instance.OnRecipeFail += DeliveryManager_OnRecipeFail;

        gameObject.SetActive(false);
    }

    private void DeliveryManager_OnRecipeFail(object sender, System.EventArgs e)
    {
        gameObject.SetActive(true);
        animator.SetTrigger(AnimationStrings.PopUp);

        backgroundImage.color = failColor;
        iconImage.sprite = failIcon;
        resultText.text = "Delivery\nFailed";
    }

    private void DeliveryManager_OnRecipeSuccess(object sender, System.EventArgs e)
    {
        gameObject.SetActive(true);
        animator.SetTrigger(AnimationStrings.PopUp);

        backgroundImage.color = successColor;
        iconImage.sprite = successIcon;
        resultText.text = "Delivery\nSuccess";
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnWarningUI : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;

    private float showWarningThreshold = .5f;

    private void Start()
    {
        stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;

        Hide();
    }

    private void StoveCounter_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        bool show = stoveCounter.IsFried() && e.progressNormalized >= showWarningThreshold;
        if (show)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
}

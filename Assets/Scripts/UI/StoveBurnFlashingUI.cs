using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveBurnFlashingUI : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;

    private Animator animator;

    private float showWarningThreshold = .5f;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;

        animator.SetBool(AnimationStrings.IsFlashing, false);
    }

    private void StoveCounter_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        bool show = stoveCounter.IsFried() && e.progressNormalized >= showWarningThreshold;
        animator.SetBool(AnimationStrings.IsFlashing, show);
    }
}

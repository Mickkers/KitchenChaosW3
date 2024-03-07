using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;
    private Player player;


    void Start()
    {
        animator = GetComponent<Animator>();
        player = GetComponentInParent<Player>();
    }


    void Update()
    {
        animator.SetBool(AnimationStrings.IsWalking, player.IsWalking);
    }
}

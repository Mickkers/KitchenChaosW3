using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    [SerializeField] private float footstepInterval;

    private Player player;
    private float footstepTimer;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void Update()
    {
        footstepTimer += Time.deltaTime;
        if(footstepTimer >= footstepInterval)
        {
            footstepTimer = 0f;

            if (player.IsWalking)
            {
                float volume = 1f;
                SoundManager.Instance.PlayFootsteps(transform.position, volume);
            }
        }
    }
}

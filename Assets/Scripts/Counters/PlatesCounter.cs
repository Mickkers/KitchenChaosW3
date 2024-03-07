using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounter : BaseCounter
{
    public event EventHandler OnPlatesSpawned;
    public event EventHandler OnPlatesRemoved;

    [SerializeField] private KitchenObjectSO plateKitchenObjectSO;
    
    [SerializeField] private float plateSpawnInterval;
    [SerializeField] private int plateCountMax;

    private float plateSpawnTimer;
    private int plateCount;
    private void Update()
    {
        plateSpawnTimer += Time.deltaTime;
        if(GameManager.Instance.IsGamePlaying() && plateSpawnTimer > plateSpawnInterval)
        {
            plateSpawnTimer = 0f;

            if(plateCount < plateCountMax)
            {
                OnPlatesSpawned?.Invoke(this, EventArgs.Empty);
                plateCount++;
            }
        }
    }

    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject())
        {
            if(plateCount > 0)
            {
                plateCount--;

                KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, player);

                OnPlatesRemoved?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}

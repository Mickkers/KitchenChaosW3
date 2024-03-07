using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    private const string PLAYER_PREFS_SFX = "SfxVol";

    [SerializeField] private AudioRefsSO audioRefsSO;

    private float volume;

    private void Awake()
    {
        Instance = this;
    }


    private void Start()
    {
        DeliveryManager.Instance.OnRecipeFail += DeliveryManager_OnRecipeFail;
        DeliveryManager.Instance.OnRecipeSuccess += DeliveryManager_OnRecipeSuccess;
        CuttingCounter.OnAnyCut += CuttingCounter_OnAnyCut;
        Player.Instance.OnObjectPickup += Player_OnObjectPickup;
        BaseCounter.OnAnyObjectPlacement += BaseCounter_OnAnyObjectPlacement;
        TrashCounter.OnAnyObjectTrashed += TrashCounter_OnAnyObjectTrashed;

        volume = PlayerPrefs.GetFloat(PLAYER_PREFS_SFX, .5f);
    }

    public void PlayFootsteps(Vector3 position, float volume)
    {
        PlaySound(audioRefsSO.footstep, position, volume);
    }

    public void PlayWarningSound(Vector3 position)
    {
        PlaySound(audioRefsSO.warning, position);
    }

    public void PlayCountdown()
    {
        PlaySound(audioRefsSO.warning, Vector3.zero);
    }

    private void TrashCounter_OnAnyObjectTrashed(object sender, System.EventArgs e)
    {
        TrashCounter trashCounter = sender as TrashCounter;
        PlaySound(audioRefsSO.trash, trashCounter.transform.position);
    }

    private void BaseCounter_OnAnyObjectPlacement(object sender, System.EventArgs e)
    {
        BaseCounter baseCounter = sender as BaseCounter;
        PlaySound(audioRefsSO.drop, baseCounter.transform.position);
    }

    private void Player_OnObjectPickup(object sender, System.EventArgs e)
    {
        PlaySound(audioRefsSO.pickup, Player.Instance.transform.position);
    }

    private void CuttingCounter_OnAnyCut(object sender, System.EventArgs e)
    {
        CuttingCounter cuttingCounter = sender as CuttingCounter;
        PlaySound(audioRefsSO.chop, cuttingCounter.transform.position);
    }

    private void DeliveryManager_OnRecipeSuccess(object sender, System.EventArgs e)
    {
        PlaySound(audioRefsSO.deliverySuccess, DeliveryCounter.Instance.transform.position);
    }

    private void DeliveryManager_OnRecipeFail(object sender, System.EventArgs e)
    {
        PlaySound(audioRefsSO.deliveryFail, DeliveryCounter.Instance.transform.position);
    }

    private void PlaySound(AudioClip[] audioClip, Vector3 position, float volume = 1f)
    {
        PlaySound(audioClip[Random.Range(0, audioClip.Length)], position, volume);
    }

    private void PlaySound(AudioClip audioClip, Vector3 position, float volumeMultiplier = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volumeMultiplier);
    }

    public void ChangeVolume()
    {
        volume += .1f;

        if(volume > 1f)
        {
            volume = 0f;
        }

        PlayerPrefs.SetFloat(PLAYER_PREFS_SFX, volume);
    }

    public float GetVolume()
    {
        return volume;
    }
}

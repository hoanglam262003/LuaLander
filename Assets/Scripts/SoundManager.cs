using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private const int SOUND_VOLUME_MAX = 11;
    private static int soundVolume = 6;
    [SerializeField] private AudioClip fuelPickUp;
    [SerializeField] private AudioClip coinPickUp;
    [SerializeField] private AudioClip landingSuccess;
    [SerializeField] private AudioClip crash;

    public static SoundManager Instance { get; private set; }
    public event EventHandler OnSoundVolumeChanged;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        Lander.Instance.OnFuelPickUp += Lander_OnFuelPickUp;
        Lander.Instance.OnCoinPickUp += Lander_OnCoinPickUp;
        Lander.Instance.OnLanded += Lander_OnLanded;
    }

    private void Lander_OnLanded(object sender, Lander.OnLandedEventArgs e)
    {
        switch (e.landingType)
        {
            case Lander.LandingType.Success:
                AudioSource.PlayClipAtPoint(landingSuccess, Camera.main.transform.position, GetSoundVolumeNormalize());
                break;
            default:
                AudioSource.PlayClipAtPoint(crash, Camera.main.transform.position, GetSoundVolumeNormalize());
                break;
        }
    }

    private void Lander_OnCoinPickUp(object sender, EventArgs e)
    {
        AudioSource.PlayClipAtPoint(coinPickUp, Camera.main.transform.position, GetSoundVolumeNormalize());
    }

    private void Lander_OnFuelPickUp(object sender, EventArgs e)
    {
        AudioSource.PlayClipAtPoint(fuelPickUp, Camera.main.transform.position, GetSoundVolumeNormalize());
    }

    public void ChangeSoundVolume()
    {
        soundVolume = (soundVolume + 1) % SOUND_VOLUME_MAX;
        OnSoundVolumeChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetSoundVolume()
    {
        return soundVolume;
    }

    public float GetSoundVolumeNormalize()
    {
        return ((float)soundVolume) / SOUND_VOLUME_MAX;
    }
}

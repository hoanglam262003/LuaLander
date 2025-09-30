using System;
using UnityEngine;

public class LanderAudio : MonoBehaviour
{
    [SerializeField] private AudioSource thruster;

    private Lander lander;

    private void Awake()
    {
        lander = GetComponent<Lander>();
    }

    private void Start()
    {
        lander.OnBeforeForce += Lander_OnBeforeForce;
        lander.OnUpForce += Lander_OnUpForce;
        lander.OnRightForce += Lander_OnRightForce;
        lander.OnLeftForce += Lander_OnLeftForce;

        SoundManager.Instance.OnSoundVolumeChanged += SoundManager_OnSoundVolumeChanged;
        thruster.Pause();
    }

    private void SoundManager_OnSoundVolumeChanged(object sender, EventArgs e)
    {
        thruster.volume = SoundManager.Instance.GetSoundVolumeNormalize();
    }

    private void Lander_OnLeftForce(object sender, EventArgs e)
    {
        if (!thruster.isPlaying)
        {
            thruster.Play();
        }
    }

    private void Lander_OnRightForce(object sender, EventArgs e)
    {
        if (!thruster.isPlaying)
        {
            thruster.Play();
        }
    }

    private void Lander_OnUpForce(object sender, EventArgs e)
    {
        if (!thruster.isPlaying)
        {
            thruster.Play();
        }
    }

    private void Lander_OnBeforeForce(object sender, EventArgs e)
    {
        thruster.Pause();
    }
}

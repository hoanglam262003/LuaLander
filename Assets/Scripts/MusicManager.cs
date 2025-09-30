using System;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private const int MUSIC_VOLUME_MAX = 11;
    private static float musicTime;
    private static int musicVolume = 6;

    private AudioSource music;

    public event EventHandler OnMusicVolumeChanged;
    public static MusicManager Instance {  get; private set; }

    private void Awake()
    {
        Instance = this;
        music = GetComponent<AudioSource>();
        music.time = musicTime;
    }

    private void Start()
    {
        music.volume = GetMusicVolumeNormalize();
    }

    private void Update()
    {
        musicTime = music.time;
    }

    public void ChangeMusicVolume()
    {
        musicVolume = (musicVolume + 1) % MUSIC_VOLUME_MAX;
        music.volume = GetMusicVolumeNormalize();
        OnMusicVolumeChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetMusicVolume()
    {
        return musicVolume;
    }

    public float GetMusicVolumeNormalize()
    {
        return ((float)musicVolume) / MUSIC_VOLUME_MAX;
    }
}

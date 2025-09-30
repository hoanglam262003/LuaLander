using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    [SerializeField] private Button resume;
    [SerializeField] private Button mainMenu;
    [SerializeField] private Button soundVolume;
    [SerializeField] private TextMeshProUGUI soundVolumeText;
    [SerializeField] private Button musicVolume;
    [SerializeField] private TextMeshProUGUI musicVolumeText;

    private void Awake()
    {
        soundVolume.onClick.AddListener(() =>
        {
            SoundManager.Instance.ChangeSoundVolume();
            soundVolumeText.text = "SOUND " + SoundManager.Instance.GetSoundVolume();
        });
        musicVolume.onClick.AddListener(() =>
        {
            MusicManager.Instance.ChangeMusicVolume();
            musicVolumeText.text = "MUSIC " + MusicManager.Instance.GetMusicVolume();
        });
        resume.onClick.AddListener(() =>
        {
            GameManager.Instance.ResumeGame();
        });
        mainMenu.onClick.AddListener(() =>
        {
            SceneLoader.LoadScene(SceneLoader.Scene.MainMenuScene);
        });
    }

    private void Start()
    {
        GameManager.Instance.onPause += GameManager_OnPause;
        GameManager.Instance.onResume += GameManager_OnResume;
        soundVolumeText.text = "SOUND " + SoundManager.Instance.GetSoundVolume();
        musicVolumeText.text = "MUSIC " + MusicManager.Instance.GetMusicVolume();
        Hide();
    }

    private void GameManager_OnPause(object sender, EventArgs e)
    {
        Show();
    }

    private void GameManager_OnResume(object sender, EventArgs e)
    {
        Hide();
    }

    private void Show()
    {
        gameObject.SetActive(true);
        resume.Select();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
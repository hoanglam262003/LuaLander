using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button start;
    [SerializeField] private Button quit;

    private void Awake()
    {
        Time.timeScale = 1f;
        start.onClick.AddListener(() =>
        {
            GameManager.ResetStaticData();
            SceneLoader.LoadScene(SceneLoader.Scene.GameScene);
        });
        quit.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }

    private void Start()
    {
        start.Select();
    }
}
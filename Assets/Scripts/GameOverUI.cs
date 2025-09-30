using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private Button mainMenu;
    [SerializeField] private TextMeshProUGUI scoreText;

    private void Awake()
    {
        mainMenu.onClick.AddListener(() => 
        {
            SceneLoader.LoadScene(SceneLoader.Scene.MainMenuScene);
        });
    }

    private void Start()
    {
        scoreText.text = "FINAL SCORE:" + GameManager.Instance.GetTotalScore().ToString();
        mainMenu.Select();
    }
}

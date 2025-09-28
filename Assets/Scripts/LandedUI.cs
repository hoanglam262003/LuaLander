using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LandedUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI statsText;
    [SerializeField] private Button button;

    private void Awake()
    {
        button.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(0);
        });
    }
    private void Start()
    {
        Lander.Instance.OnLanded += Lander_OnLanded;
        Hide();
    }

    private void Lander_OnLanded(object sender, Lander.OnLandedEventArgs e)
    {
        string title;
        switch (e.landingType)
        {
            default:
            case Lander.LandingType.Success:
                title = "SUCCESSFUL LANDING!";
                break;
            case Lander.LandingType.Crash:
                title = "<color=#ff0000>YOU CRASHED!</color>";
                break;
            case Lander.LandingType.BadAngle:
                title = "<color=#ff0000>Bad Angle!</color>";
                break;
            case Lander.LandingType.WrongArea:
                title = "<color=#ff0000>WRONG AREA!</color>";
                break;
        }
        titleText.text = title;
        statsText.text = $"{Mathf.Round(e.landingSpeed * 10f)}\n" +
                         $"{Mathf.Round(e.landingAngle * 100f)}\n" +
                         "x" + $"{e.scoreMultiplier}\n" +
                         $"{e.score}";
        Show();
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statsTextMesh;
    [SerializeField] private GameObject speedUpArrow;
    [SerializeField] private GameObject speedDownArrow;
    [SerializeField] private GameObject speedLeftArrow;
    [SerializeField] private GameObject speedRightArrow;
    [SerializeField] private Image fuel;
    private void Update()
    {
        UpdateStatsText();
    }

    private void UpdateStatsText()
    {
        speedUpArrow.SetActive(Lander.Instance.GetSpeedY() >= 0f);
        speedDownArrow.SetActive(Lander.Instance.GetSpeedY() < 0f);
        speedLeftArrow.SetActive(Lander.Instance.GetSpeedX() < 0f);
        speedRightArrow.SetActive(Lander.Instance.GetSpeedX() >= 0f);
        fuel.fillAmount = Lander.Instance.GetFuelAmountNormalized();
        statsTextMesh.text = GameManager.Instance.GetScore() + "\n" +
                             Mathf.Round(GameManager.Instance.GetTime()) + "\n" +
                             Mathf.Abs(Mathf.Round(Lander.Instance.GetSpeedX() * 10f)) + "\n" +
                             Mathf.Abs(Mathf.Round(Lander.Instance.GetSpeedY() * 10f));                       
    }
}

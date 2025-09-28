using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private int score;
    private float time;
    private bool isTimerActive;
    
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        Lander.Instance.OnCoinPickUp += Lander_OnCoinPickUp;
        Lander.Instance.OnLanded += Lander_OnLanded;
        Lander.Instance.OnStateChanged += Lander_OnStateChanged;
    }

    private void Update()
    {
        if (isTimerActive) 
        {
            time += Time.deltaTime;
        }
    }

    public void AddScore(int scoreAmount)
    {
        score += scoreAmount;
    }

    private void Lander_OnCoinPickUp(object sender, System.EventArgs e)
    {
        AddScore(100);
    }

    private void Lander_OnLanded(object sender, Lander.OnLandedEventArgs e)
    {
        AddScore(e.score);
    }

    private void Lander_OnStateChanged(object sender, Lander.OnStateChangedEventArgs e)
    {
        isTimerActive = e.state == Lander.State.Normal;
    }

    public int GetScore()
    {
        return score;
    }

    public float GetTime()
    {
        return time;
    }
}

using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public float SurvivedTime { get; private set; }
    public int TotalKillCount { get; private set; }

    public bool GameFinished = false;

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (GameMainManager.Instance.IsPlayerStartedToMove && !GameFinished)
        {
            SurvivedTime += Time.deltaTime;
        }
    }

    public void OnAgentKilled()
    {
        if (!GameFinished)
        {
            TotalKillCount++;
        }
    }
}
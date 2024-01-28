using TMPro;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainUIManager : MonoBehaviour
{
    public static MainUIManager Instance;

    //Final
    public GameObject FinalPanel;
    public TextMeshProUGUI FinalText;
    public TextMeshProUGUI FinalSurviveTimeText;
    public TextMeshProUGUI FinalKillCountText;

    //In Game
    public GameObject InGamePanel;
    public Image InGameFoodProgressImage;
    public TextMeshProUGUI InGameFoodProgressText;

    private void Awake()
    {
        Instance = this;

        if (FinalPanel.activeInHierarchy)
        {
            FinalPanel.SetActive(false);
        }

        if (!InGamePanel.activeInHierarchy)
        {
            InGamePanel.SetActive(true);
        }
    }

    public void GameFinished(string explanation)
    {
        InGamePanel.SetActive(false);
        Destroy(InGamePanel);
        FinalPanel.SetActive(true);
        FinalText.text = explanation;

        FinalSurviveTimeText.text = GenericDataManager.SurvivalTimeText + ScoreManager.Instance.SurvivedTime.ToString("0.00");
        FinalKillCountText.text = GenericDataManager.TotalBlockedAgentCountText + ScoreManager.Instance.TotalKillCount;
    }

    public void RestartButtonClicked()
    {
        SceneManager.LoadScene(0);
    }

    public void UpdateFoodStatusInGame(int currentFoodStock)
    {
        InGameFoodProgressImage.fillAmount = (currentFoodStock / 100f);
        InGameFoodProgressText.text = $"{currentFoodStock}%";
    }

    public void ExitGameButtonClicked()
    {
        Application.Quit();
    }
}
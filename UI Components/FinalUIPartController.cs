using UnityEngine;

public class FinalUIPartController : MonoBehaviour
{
    public void RestartButtonClicked()
    {
        MainUIManager.Instance.RestartButtonClicked();
    }

    public void ExitGameButtonClicked()
    {
        MainUIManager.Instance.ExitGameButtonClicked();
    }
}

using UnityEngine;

public class SceneAgentManager : MonoBehaviour
{
    public static SceneAgentManager Instance;
    public GameObject agent;

    private void Awake()
    {
        Instance = this;
    }

}

using System.Collections.Generic;

using UnityEngine;

public class GameMainManager : MonoBehaviour
{
    public static GameMainManager Instance;

    [SerializeField]
    private GameObject MainPlayer;

    [SerializeField]
    private Transform AgentsParent;

    public bool IsPlayerStartedToMove = false;

    private void Awake()
    {
        Instance = this;

        if (MainPlayer == null)
        {
            MainPlayer = GameObject.FindGameObjectWithTag("MainVehicle");
        }

        Debug.Assert(MainPlayer != null, "Target Transform null in agent controller this should not be possible");
    }

    private void Start()
    {
        AgentManager.Instance.GenerateAgent(GenericDataManager.AgentsCountToSpawnOnStart);
    }

    public void PlayerDestroyed(string explanation)
    {
        MainPlayer.GetComponent<CarMovementController>().PlayerIsDestroyed = true;
        DetachCamerasFromPlayerObject();
        EffectsManager.Instance.OnPlayerDestroyed(MainPlayer.transform.position);
        Destroy(MainPlayer);
        MainUIManager.Instance.GameFinished(explanation);
        ScoreManager.Instance.GameFinished = true;
        GroundAndRoadManager.Instance.StopGroundAndRoadCheck = true;
        BuildingManager.Instance.StopBuildingCheck = true;
        ProjectileController.Instance.DisableProjectileCreation = true;
        AgentManager.Instance.IsPlayerDestroyed = true;
        MusicManager.Instance.OnGameFinished();
        DisableAgents();
    }

    private void DetachCamerasFromPlayerObject()
    {
        var cameraToUpdate = new List<GameObject>();
        for (int i = 0; i < MainPlayer.transform.childCount; i++)
        {
            var child = MainPlayer.transform.GetChild(i);

            if (child.gameObject.GetComponent<Camera>() != null)
            {
                cameraToUpdate.Add(child.gameObject);
            }
        }

        foreach (var camera in cameraToUpdate)
        {
            camera.GetComponent<CameraTracker>().DisableCamera = true;
            camera.transform.parent = null;
        }

        cameraToUpdate.Clear();
    }

    public void DisableAgents()
    {
        foreach (Transform agentTransform in AgentsParent.transform)
        {
            agentTransform.GetComponent<AgentController>().PlayerIsDestroyed();
        }
    }
}
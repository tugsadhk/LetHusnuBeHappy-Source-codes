using System.Collections.Generic;

using UnityEngine;

public class AgentManager : MonoBehaviour
{
    public static AgentManager Instance;

    [SerializeField]
    private Transform MainVehicle;

    public Transform AgentParent;

    private int _currentAgentCount;
    private int _counterForNaming;
    private readonly float _creationYPos = 0.095f;

    public GameObject[] NormalAgents;
    public GameObject[] SlowAgents;
    public GameObject[] FastAgents;

    public bool IsPlayerDestroyed = false;

    private readonly List<KeyValuePair<float, float>> _agentCreationPositionLimits = new List<KeyValuePair<float, float>>() {
          new KeyValuePair<float, float>(-7f, 7f),
          new KeyValuePair<float, float>(9f, 9f + GenericDataManager.AgentCreationAfterBuildingDistance),
          new KeyValuePair<float, float>(-9f, -9f - GenericDataManager.AgentCreationAfterBuildingDistance)
    };

    private void Awake()
    {
        Instance = this;
    }

    public void GenerateAgent(int spawnCount)
    {
        if (!IsPlayerDestroyed)
        {
            for (int i = 0; i < spawnCount; i++)
            {
                if (AgentParent.childCount < GenericDataManager.MaximumAgentCount)
                {
                    var limitsToUseForCreation = _agentCreationPositionLimits[Random.Range(Random.Range(0f, 1f) < 0.1f ? 0 : 1, _agentCreationPositionLimits.Count)];

                    var agentPosZ = Random.Range(limitsToUseForCreation.Key, limitsToUseForCreation.Value);
                    var mainVehiclePosX = MainVehicle.position.x;

                    var agentPosX = Random.Range(mainVehiclePosX - GenericDataManager.AgentCreationDistanceForBackwardFromPlayer,
                        mainVehiclePosX + GenericDataManager.AgentCreationDistanceForForwardFromPlayer);

                    var _direction = (MainVehicle.position - transform.position).normalized;

                    var agentProbability = Random.Range(0f, 1f);
                    GameObject agentToCreate;

                    if (agentProbability <= GenericDataManager.SlowAgentCreationProbability)
                    {
                        agentToCreate = SlowAgents[Random.Range(0, SlowAgents.Length)];
                    }
                    else if (agentProbability > GenericDataManager.SlowAgentCreationProbability && agentProbability < GenericDataManager.SlowAgentCreationProbability + GenericDataManager.FastAgentCreationProbability)
                    {
                        agentToCreate = FastAgents[Random.Range(0, FastAgents.Length)];
                    }
                    else
                    {
                        agentToCreate = NormalAgents[Random.Range(0, NormalAgents.Length)];
                    }

                    var createdAgent = Instantiate(agentToCreate, new Vector3(agentPosX, _creationYPos, agentPosZ), Quaternion.LookRotation(_direction), AgentParent);
                    createdAgent.name = "Agent " + _counterForNaming++;

                    var scaleToApply = 1 + Random.Range(-GenericDataManager.AgentScaleOffsetLimit, GenericDataManager.AgentScaleOffsetLimit);
                    createdAgent.transform.localScale = new Vector3(scaleToApply, scaleToApply, scaleToApply);

                    _currentAgentCount++;
                }
                else
                {
                    break;
                }
            }
        }
    }

    public void AgentDestroyed()
    {
        _currentAgentCount--;

        if (Random.Range(0f, 1f) <= 0.1f)
        {
            GenerateAgent(1);
        }

        if (_currentAgentCount % 25 == 0)
        {
            GenerateAgent(Random.Range(1, GenericDataManager.MaxNumberOfAgentCreatedAtOnCreation));
        }

        if (_currentAgentCount <= GenericDataManager.MinimumAcceptableAgentCount)
        {
            //Create bunch of agent at one time
            GenerateAgent(Random.Range(1, GenericDataManager.MinimumAcceptableAgentCount));
        }
    }
}
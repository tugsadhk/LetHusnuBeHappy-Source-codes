using System.Linq;

using UnityEngine;
using UnityEngine.AI;

public class AgentController : MonoBehaviour
{
    public bool IsFemale = true;

    public enum AgentTypes { Normal, Slow, Fast }

    public AgentTypes AgentType = AgentTypes.Normal;

    enum AgentState { Idle, Chasing }

    private AgentState _agentState = AgentState.Idle;

    [SerializeField]
    private bool StopAgent = false;

    [SerializeField]
    private Transform _targetTransform;

    private NavMeshAgent _agent;
    private Vector3 _cachedTargetPos;
    private int _optimizationCount = 0;
    private readonly int _controlCount = 25;
    private float _agentTypeVisionBonusMultiplier = 1;

    private AgentAnimationController _agentAnimationController;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();

        switch (AgentType)
        {
            case AgentTypes.Normal:
                _agentTypeVisionBonusMultiplier = 1.25f;
                break;
            case AgentTypes.Slow:
                break;
            case AgentTypes.Fast:
                _agentTypeVisionBonusMultiplier = 1.5f;
                break;
        }
    }

    void Start()
    {
        _cachedTargetPos = transform.position;

        if (_targetTransform == null)
        {
            _targetTransform = GameObject.FindGameObjectWithTag("MainVehicle").transform;
        }

        _agentAnimationController = GetComponent<AgentAnimationController>();

        if (_agentAnimationController != null)
        {
            _agentAnimationController.IsFemale = IsFemale;
            _agentAnimationController.OnAgentCreationFinished();
        }
        else
        {
            Debug.LogError("There is animation script missing in agent");
        }

        Debug.Assert(_targetTransform != null, "Target Transform null in agent controller this should not be possible");
    }

    void Update()
    {
        if (!StopAgent && GameMainManager.Instance.IsPlayerStartedToMove)
        {
            _optimizationCount++;
            if (_optimizationCount == _controlCount)
            {
                _optimizationCount = 0;

                if (Mathf.Abs(Vector3.Distance(_targetTransform.position, transform.position)) >= GenericDataManager.DistanceThresholdToPlayerForDestroyItself)
                {
                    MakeAgentDead(true);
                    return;
                }

                switch (_agentState)
                {
                    case AgentState.Idle:
                        var mainVehicleInVisionRange =
                            Physics.OverlapSphere(transform.position, GenericDataManager.AgentDefaultVisionRange * _agentTypeVisionBonusMultiplier)
                            .Where(x => x.tag == "MainVehicle");

                        if (mainVehicleInVisionRange.Any() && Random.Range(0, 1f) <= GenericDataManager.AgentChaseProbability)
                        {
                            _agentState = AgentState.Chasing;
                            _agentAnimationController.AgentStartedChasingPlayer();
                            StartChasing();
                        }
                        break;
                    case AgentState.Chasing:
                        ControlAndUpdateChasingIfNeeded();
                        break;
                }
            }
        }
    }

    private void StartChasing()
    {
        _cachedTargetPos = _targetTransform.position;
        _agent.SetDestination(_cachedTargetPos);
        _agent.speed = GenericDataManager.AgentMinSpeed + Random.Range(0f, GenericDataManager.AgentStartSpeedOffset);
    }

    private void ControlAndUpdateChasingIfNeeded()
    {
        if (_agent.speed < GenericDataManager.AgentMaxSpeed)
        {
            _agent.speed += Time.deltaTime * _agentTypeVisionBonusMultiplier;
        }

        if (_targetTransform.position != _cachedTargetPos)
        {
            _cachedTargetPos = _targetTransform.position;
            _agent.SetDestination(_cachedTargetPos);

        }
    }

    public void MakeAgentDead(bool destroyImmediately = false)
    {
        AgentManager.Instance.AgentDestroyed();
        _agent.isStopped = true;
        MusicManager.Instance.OnAgentDead();
        ScoreManager.Instance.OnAgentKilled();

        if (!destroyImmediately)
        {
            Destroy(gameObject, GenericDataManager.DestroyingTime);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayerIsDestroyed()
    {
        _agentAnimationController.PlayerIsDestroyed();
        _agent.isStopped = true;
        StopAgent = true;
    }
}
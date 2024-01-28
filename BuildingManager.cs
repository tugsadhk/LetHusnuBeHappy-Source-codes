using System.Collections.Generic;

using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager Instance;

    public List<GameObject> Buildings = new List<GameObject>();

    public float BuildingCreationThresholdFromPlayer = 50f;
    public Transform BuildingParent;
    public Transform PlayerVehicle;

    [SerializeField]
    private float ZDistanceForBuildingCreation = -9.84f;

    [SerializeField]
    private float MinDistanceBetweenBuilding = 3.7f;

    [SerializeField]
    private float MaxDistanceBetweenBuilding = 5.6f;

    private float _lastCreatedBuildingXPos = float.MinValue;

    private int _creationCount = 0;
    private readonly int _buildingControlInterval = 30;
    private int _buildingControlCount = 0;
    private readonly float _buildingCreationIntervalLimit = 1.5f;


    public bool StopBuildingCheck = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        CreateBuildingsOnStart();
        NavMeshManager.Instance.BuildNavMesh();
    }

    private void CreateBuildingsOnStart()
    {
        var initialCreationPointX = PlayerVehicle.position.x - BuildingCreationThresholdFromPlayer;

        while (_lastCreatedBuildingXPos < (PlayerVehicle.position.x + BuildingCreationThresholdFromPlayer))
        {
            var buildingWillCreate = Buildings[UnityEngine.Random.Range(0, Buildings.Count)];
            var xScale = buildingWillCreate.transform.localScale.x / 2 + 0.5f;

            var creationPosition = new Vector3(initialCreationPointX, buildingWillCreate.transform.position.y, buildingWillCreate.transform.position.z);

            if (_lastCreatedBuildingXPos != float.MinValue)
            {
                var control = _lastCreatedBuildingXPos + xScale + UnityEngine.Random.Range(MinDistanceBetweenBuilding, MaxDistanceBetweenBuilding);
                creationPosition.x = control;
            }

            var createdBuilding = Instantiate(buildingWillCreate, creationPosition, Quaternion.identity, BuildingParent);
            createdBuilding.name = "Building" + _creationCount++;

            _lastCreatedBuildingXPos = creationPosition.x;

            //Create other side to as symmetric
            buildingWillCreate = Buildings[UnityEngine.Random.Range(0, Buildings.Count)];
            creationPosition = new Vector3(_lastCreatedBuildingXPos, buildingWillCreate.transform.position.y, -buildingWillCreate.transform.position.z);
            createdBuilding = Instantiate(buildingWillCreate, creationPosition, Quaternion.identity, BuildingParent);
            createdBuilding.name = "Building_S" + _creationCount;
        }
    }

    private void FixedUpdate()
    {
        if (!StopBuildingCheck)
        {
            _buildingControlCount++;
            if (_buildingControlCount == _buildingControlInterval)
            {
                _buildingControlCount = 0;
                CheckExistingBuildingsForRemoval();
            }
        }
    }

    private void CheckExistingBuildingsForRemoval()
    {
        for (int i = 0; i < BuildingParent.childCount; i++)
        {
            var createdBuilding = BuildingParent.GetChild(i);

            if (createdBuilding.position.x >= PlayerVehicle.transform.position.x)
            {
                return;
            }

            if (createdBuilding.transform.position.x < PlayerVehicle.transform.position.x - BuildingCreationThresholdFromPlayer
                && Mathf.Abs((PlayerVehicle.transform.position.x - BuildingCreationThresholdFromPlayer) - createdBuilding.transform.position.x) > _buildingCreationIntervalLimit)
            {
                Destroy(createdBuilding.gameObject);
                CreateNewBuildingOnPlayerMove(createdBuilding.transform.position.z > PlayerVehicle.transform.position.z);
            }
            else
            {
                break;
            }
        }

        NavMeshManager.Instance.UpdateNavMesh();
    }

    private void CreateNewBuildingOnPlayerMove(bool isZPlus)
    {
        var lastCreatedBuildingPosX = BuildingParent.GetChild(BuildingParent.childCount - 1).transform.position.x;

        var buildingWillCreate = Buildings[UnityEngine.Random.Range(0, Buildings.Count)];
        var xScale = buildingWillCreate.transform.localScale.x / 2 + 0.5f;

        var creationPosition =
            new Vector3(lastCreatedBuildingPosX + xScale + UnityEngine.Random.Range(MinDistanceBetweenBuilding, MaxDistanceBetweenBuilding),
            buildingWillCreate.transform.position.y,
            isZPlus ? -buildingWillCreate.transform.position.z : buildingWillCreate.transform.position.z);

        var createdBuilding = Instantiate(buildingWillCreate, creationPosition, Quaternion.identity, BuildingParent);
        createdBuilding.name = isZPlus ? "Building_S" : "Building" + _creationCount++;
    }


    //#if UNITY_EDITOR

    //    private void OnDrawGizmos()
    //    {
    //        Gizmos.color = Color.green;

    //        var tempvec = PlayerVehicle.position;
    //        tempvec.x += BuildingCreationThresholdFromPlayer;
    //        Gizmos.DrawLine(PlayerVehicle.position, tempvec);
    //        tempvec.x -= BuildingCreationThresholdFromPlayer * 2;
    //        Gizmos.DrawLine(PlayerVehicle.position, tempvec);
    //    }
    //#endif
}
using Unity.AI.Navigation;

using UnityEngine;

public class GroundAndRoadManager : MonoBehaviour
{
    public static GroundAndRoadManager Instance;

    private float _groundAndRoadLength = float.MinValue;

    public GameObject GroundAndRoad;
    public Transform GroundAndRoadParent;
    public Transform PlayerVehicle;

    private float _lastCreationXPosition;

    private readonly int _controlInterval = 50;
    private int _controlCounter = 0;

    private int _newOneCreationControlPositionX = int.MinValue;
    private int _destroyOldOneControlPositionX = int.MinValue;
    private bool _firstCreationHappened = false;

    private int _creationCounter = 0;

    public bool StopGroundAndRoadCheck = false;


    private void Awake()
    {
        Instance = this;
        var groundObject = GroundAndRoad.transform.GetChild(0);
        _groundAndRoadLength = groundObject.transform.localScale.x * 10;
        _lastCreationXPosition = groundObject.transform.localPosition.x;

        var xPositionOfPlayerVehicleOnStart = PlayerVehicle.transform.position.x;
        _newOneCreationControlPositionX = (int)(xPositionOfPlayerVehicleOnStart + _groundAndRoadLength / 4);
        _destroyOldOneControlPositionX = (int)(xPositionOfPlayerVehicleOnStart + _groundAndRoadLength * 3 / 4);
    }

    private void FixedUpdate()
    {
        if (!StopGroundAndRoadCheck && _groundAndRoadLength != float.MinValue)
        {
            _controlCounter++;
            if (_controlCounter == _controlInterval)
            {
                _controlCounter = 0;
                var xPositionOfPlayerVehicle = PlayerVehicle.transform.position.x;

                if (xPositionOfPlayerVehicle >= _newOneCreationControlPositionX)
                {
                    if (!_firstCreationHappened)
                    {
                        _firstCreationHappened = true;
                    }

                    var creationPositionOfX = _lastCreationXPosition + _groundAndRoadLength;

                    var createdGroundAndRoad = Instantiate(GroundAndRoad, new Vector3(creationPositionOfX, 0, 0), Quaternion.identity, GroundAndRoadParent);
                    createdGroundAndRoad.name = "GroundAndRoad " + _creationCounter++;

                    NavMeshManager.Instance.AddNewNavMeshSurface(createdGroundAndRoad.transform.GetChild(0).GetComponent<NavMeshSurface>());

                    _lastCreationXPosition = creationPositionOfX;
                    _newOneCreationControlPositionX += (int)_groundAndRoadLength;
                }

                if (_firstCreationHappened && xPositionOfPlayerVehicle >= _destroyOldOneControlPositionX)
                {
                    NavMeshManager.Instance.RemoveNavMeshSurface(GroundAndRoadParent.GetChild(0).transform.GetChild(0).GetComponent<NavMeshSurface>());
                    Destroy(GroundAndRoadParent.GetChild(0).gameObject);
                    _destroyOldOneControlPositionX += (int)_groundAndRoadLength;
                }
            }
        }
    }
}
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class TreeCreator : MonoBehaviour
{
    [SerializeField]
    private Transform MainVehicle;

    public Transform TreeParent;

    public GameObject[] Trees;

    private readonly float _treeScaleOffset = 1.5f;
    private readonly int _treeCount = 300;

    private readonly List<KeyValuePair<float, float>> _treeCreationPositionLimits = new List<KeyValuePair<float, float>>() {
          new KeyValuePair<float, float>(9f, 9f + GenericDataManager.AgentCreationAfterBuildingDistance*3),
          new KeyValuePair<float, float>(-9f, -9f - GenericDataManager.AgentCreationAfterBuildingDistance*3)
    };

    void Start()
    {
        for (int i = 0; i < _treeCount; i++)
        {
            var areaToSpawn = _treeCreationPositionLimits[Random.Range(0, _treeCreationPositionLimits.Count)];

            var treePosZ = Random.Range(areaToSpawn.Key, areaToSpawn.Value);

            var treePosX = Random.Range(MainVehicle.transform.position.x - GenericDataManager.AgentCreationDistanceForForwardFromPlayer
                , MainVehicle.transform.position.x + GenericDataManager.AgentCreationDistanceForForwardFromPlayer);

            var createdTree = Instantiate(Trees[Random.Range(0, Trees.Count())], new Vector3(treePosX, 0, treePosZ), Quaternion.identity);
        }
    }
}
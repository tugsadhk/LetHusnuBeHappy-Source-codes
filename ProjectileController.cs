using System.Collections.Generic;

using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public static ProjectileController Instance;

    [SerializeField]
    private Transform DefenderFirePoint;

    [SerializeField]
    private Transform ProjectileParent;

    [SerializeField]
    private int MaxProjectileCountInTheScene = 50;

    [SerializeField]
    private int ProjectileCountToBeRemovedWhenExceeds = 10;

    public GameObject[] DefaultProjectiles;
    public float LaunchVelocity = 250f;
    public float RandomnessOffset = 50f;
    private readonly List<GameObject> _createdProjectiles = new List<GameObject>();

    public bool DisableProjectileCreation = false;

    private readonly float _forceYReductionMax = 150;

    private float _timeCounter = GenericDataManager.DefaultAttackCooldownTime;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        _timeCounter += Time.deltaTime;
        if (_timeCounter >= GenericDataManager.DefaultAttackCooldownTime && !DisableProjectileCreation && Input.GetButtonDown("Fire1"))
        {
            _timeCounter = 0;

            GameObject createdProjectile = Instantiate(DefaultProjectiles[Random.Range(0, DefaultProjectiles.Length)],
                DefenderFirePoint.position,
                DefenderFirePoint.rotation,
                ProjectileParent);

            createdProjectile.transform.localEulerAngles = new Vector3(0, -90, 0);
            _createdProjectiles.Add(createdProjectile);
            CheckCreatedProjectilesForRemoval();
            createdProjectile.GetComponent<Rigidbody>()
                .AddRelativeForce(new Vector3(LaunchVelocity / 2 + Random.Range(-RandomnessOffset, RandomnessOffset),
                LaunchVelocity / 2 + Random.Range(-RandomnessOffset, RandomnessOffset) - Random.Range(0, _forceYReductionMax),
                0));
        }
    }

    private void CheckCreatedProjectilesForRemoval()
    {
        if (_createdProjectiles.Count >= MaxProjectileCountInTheScene)
        {
            for (int i = 0; i < ProjectileCountToBeRemovedWhenExceeds; i++)
            {
                var objectToDestroy = _createdProjectiles[i];
                _createdProjectiles.RemoveAt(i);
                Destroy(objectToDestroy);
            }
        }
    }

    public void ProjectileHitTheAgent(GameObject projectile)
    {
        if (_createdProjectiles.Contains(projectile))
        {
            _createdProjectiles.Remove(projectile);
        }

        Destroy(projectile);
    }
}
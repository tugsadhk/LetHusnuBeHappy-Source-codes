using UnityEngine;

public class EffectsManager : MonoBehaviour
{
    public static EffectsManager Instance;

    public Transform EffectsParent;
    public GameObject[] ExplosionDefaultEffects;

    public GameObject PlayerDestructionExplosionEffect;


    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// This is the default one, just creating default explosion effect at given position
    /// </summary>
    /// <param name="triggerPosition"></param>
    public void CreateExplosionAtPoint(Vector3 triggerPosition)
    {
        var createdEffect = Instantiate(
                  ExplosionDefaultEffects[Random.Range(0, ExplosionDefaultEffects.Length - 1)],
                  triggerPosition,
                  Quaternion.identity,
                  EffectsParent);

        Destroy(createdEffect, GenericDataManager.DestroyingTime);
    }

    public void OnPlayerDestroyed(Vector3 position)
    {
        var createdEffect = Instantiate(
          PlayerDestructionExplosionEffect,
          position,
          Quaternion.identity,
          EffectsParent);

        Destroy(createdEffect, GenericDataManager.DestroyingTime);
    }
}
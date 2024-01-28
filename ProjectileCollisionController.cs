using UnityEngine;

public class ProjectileCollisionController : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        ProjectileEffectManager.ProjectileHitSomething(collision.transform.position);
        EffectsManager.Instance.CreateExplosionAtPoint(transform.position);
        Destroy(gameObject);
    }
}
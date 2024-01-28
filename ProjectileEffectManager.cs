using System.Linq;

using UnityEngine;

public static class ProjectileEffectManager
{
    public static void ProjectileHitSomething(Vector3 hitPosition, GameObject excludeObject = null)
    {
        var touchedObjects =
            Physics.OverlapSphere(hitPosition, GenericDataManager.ExplosionRadius)
            .Where(x => x.tag == "Agent")
            .ToList();

        foreach (var collidedObject in touchedObjects)
        {
            if (excludeObject == null || (excludeObject != collidedObject))
            {
                var animController = collidedObject.gameObject.GetComponent<AgentAnimationController>();
                if (animController != null)
                {
                    var forceDirection = hitPosition - collidedObject.transform.position;
                    forceDirection.y = Random.Range(1, GenericDataManager.MaxForce);
                    //forceDirection.Normalize();
                    var force = Random.Range(1f, GenericDataManager.MaxForce) * forceDirection;
                    animController.TriggerRagdollAtPoint(force, collidedObject.transform.position);
                }
            }
        }
    }
}
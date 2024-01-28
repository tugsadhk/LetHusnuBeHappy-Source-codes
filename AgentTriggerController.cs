using UnityEngine;

public class AgentTriggerController : MonoBehaviour
{
    private AgentAnimationController _agentAnimationController;

    private void Awake()
    {
        _agentAnimationController = GetComponent<AgentAnimationController>();
        Debug.Assert(_agentAnimationController != null, "Agent animation controller is null, check here");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Projectile")
        {
            var forceDirection = transform.position - collision.gameObject.transform.position;
            forceDirection.y = 1;
            forceDirection.Normalize();

            var force = Random.Range(1, GenericDataManager.MaxForce) * forceDirection;

            _agentAnimationController.TriggerRagdollAtPoint(force, collision.transform.position);
            ProjectileEffectManager.ProjectileHitSomething(collision.transform.position, gameObject);
            ProjectileController.Instance.ProjectileHitTheAgent(collision.gameObject);
            EffectsManager.Instance.CreateExplosionAtPoint(collision.transform.position);
        }
    }
}
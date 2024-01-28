using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class AgentAnimationController : MonoBehaviour
{
    public bool IsFemale { get; set; }

    private Animator _animator;
    private Rigidbody[] _ragdollRigidbodies;
    private CapsuleCollider _capsuleCollider;

    private GameObject _ragDollParent;

    #region Animation Ids

    private List<string> _idleAnimationIds = new List<string>()
    { "idle_f_1",
        "idle_f_2",
        "idle_m_1",
        "idle_m_2",
        "idle_phoneTalking",
        "idle_selfcheck" };

    #endregion

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        Debug.Assert(_animator != null, "Animator is null in agent, control here");
        _capsuleCollider = GetComponent<CapsuleCollider>();
        _ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();

        _ragDollParent = transform.GetChild(transform.childCount - 1).gameObject;
        _ragDollParent.SetActive(false);

        DisableRagdoll();
    }

    public void OnAgentCreationFinished()
    {
        if (IsFemale)
        {
            _idleAnimationIds.RemoveRange(2, 2);
        }
        else
        {
            _idleAnimationIds.RemoveRange(0, 2);
        }

        PlayIdleAnimations();
    }

    private void PlayIdleAnimations()
    {
        var randomIdleAnimationId = _idleAnimationIds[Random.Range(0, _idleAnimationIds.Count - 1)];
        _animator.Play(randomIdleAnimationId);
    }

    private void DisableRagdoll()
    {
        _animator.enabled = true;
        foreach (var rigidbody in _ragdollRigidbodies)
        {
            rigidbody.isKinematic = true;
        }
    }

    private void EnableRagdoll()
    {
        _ragDollParent.SetActive(true);
        _animator.enabled = false;
        Destroy(_capsuleCollider);
        foreach (var rigidbody in _ragdollRigidbodies)
        {
            rigidbody.isKinematic = false;
        }
        Destroy(gameObject, GenericDataManager.DestroyingTime);
    }

    public void TriggerRagdollAtPoint(Vector3 force, Vector3 hitPoint)
    {
        EnableRagdoll();

        var hitRigidbody = _ragdollRigidbodies.OrderBy(rigidbody => Vector3.Distance(rigidbody.position, hitPoint)).First();

        if (Random.Range(0, 1f) > GenericDataManager.ForceMultiplicationChange)
        {
            force.y *= Random.Range(0, GenericDataManager.MaxForce);
        }

        hitRigidbody.AddForceAtPosition(force, hitPoint, ForceMode.Impulse);

        GetComponent<AgentController>().MakeAgentDead();
    }


    public void AgentStartedChasingPlayer()
    {
        _animator.enabled = true;

        if (IsFemale)
        {
            _animator.Play("f_running");
        }
        else
        {
            _animator.Play("m_running");
        }
    }

    public void PlayerIsDestroyed()
    {
        _animator.Play("OzDance");
    }
}
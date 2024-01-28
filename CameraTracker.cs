using UnityEngine;

public class CameraTracker : MonoBehaviour
{
    [SerializeField]
    private Transform _targetTransform;

    public bool isDefender = false;

    public bool DisableCamera = false;

    private void FixedUpdate()
    {
        if (!DisableCamera)
        {
            transform.LookAt(_targetTransform.position);
        }
    }
}
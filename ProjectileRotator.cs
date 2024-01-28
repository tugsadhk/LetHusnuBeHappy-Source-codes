using UnityEngine;

public class ProjectileRotator : MonoBehaviour
{
    private readonly float _rotateSpeed = 30f;

    private Vector3 rotateVector = Vector3.zero;

    void Update()
    {
        rotateVector.x += _rotateSpeed * Time.deltaTime;
        rotateVector.y -= _rotateSpeed * Time.deltaTime;
        rotateVector.z += _rotateSpeed * Time.deltaTime;
        transform.Rotate(rotateVector);
    }
}
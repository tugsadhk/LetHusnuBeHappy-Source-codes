using UnityEngine;

public class CarMovementController : MonoBehaviour
{
    public bool PlayerIsDestroyed = false;
    public float Speed = 11f;
    public float RotateSpeed = 2f;

    private int _rotateYMin;
    private int _rotateYMax;
    private readonly int _rotateLimit = 30;
    private GameObject MainVehicleObj;

    private void Awake()
    {
        MainVehicleObj = transform.GetChild(transform.childCount - 1).gameObject;
        _rotateYMin = (int)MainVehicleObj.transform.localEulerAngles.y - _rotateLimit;
        _rotateYMax = (int)MainVehicleObj.transform.localEulerAngles.y + _rotateLimit;
    }

    void Update()
    {
        if (!PlayerIsDestroyed)
        {
            var horizontalInput = Input.GetAxis("Horizontal");
            var verticalInput = Input.GetAxis("Vertical");

            if (!GameMainManager.Instance.IsPlayerStartedToMove
                && horizontalInput != 0 || verticalInput != 0)
            {
                GameMainManager.Instance.IsPlayerStartedToMove = true;
            }

            var deltaTime = Time.deltaTime;

            Vector3 finalPos = Vector3.zero;

            if (verticalInput > 0)
            {
                finalPos.x += verticalInput * Speed * deltaTime;
            }

            if (horizontalInput != 0)
            {
                finalPos.z += -horizontalInput * Speed * deltaTime;

                //Rotation
                var currentY = MainVehicleObj.transform.localEulerAngles.y;

                if (currentY < _rotateYMax && currentY > _rotateYMin || (currentY <= _rotateYMin && horizontalInput > 0) || (currentY >= _rotateYMax))
                {
                    MainVehicleObj.transform.localEulerAngles = new Vector3(0, currentY + (horizontalInput * RotateSpeed), 0);
                }
            }

            if (finalPos != Vector3.zero)
            {
                transform.position += finalPos;
            }
        }
    }
}
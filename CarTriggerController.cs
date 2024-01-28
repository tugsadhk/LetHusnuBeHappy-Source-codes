using UnityEngine;

public class CarTriggerController : MonoBehaviour
{
    CarFoodStockController carHealthController;

    private bool CanVehicleTakeHit = true;
    private float _timeCounter = 0;

    private void Update()
    {
        if (!CanVehicleTakeHit)
        {
            _timeCounter += Time.deltaTime;

            if (_timeCounter >= GenericDataManager.VehicleHitCooldownTime)
            {
                CanVehicleTakeHit = true;
                _timeCounter = 0;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Building")
        {
            GameMainManager.Instance.PlayerDestroyed(GenericDataManager.PlayerDiedBecauseOfCrashedBuilding);
            return;
        }

        if (CanVehicleTakeHit)
        {
            CanVehicleTakeHit = false;
            _timeCounter = 0;

            if (carHealthController == null)
            {
                carHealthController = GetComponent<CarFoodStockController>();
            }

            if (other.tag == "Agent")
            {
                //Agent interacted with vehicle, apply it's effects
                carHealthController.CarTakeHit();
            }
        }
    }
}
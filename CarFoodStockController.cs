using UnityEngine;

public class CarFoodStockController : MonoBehaviour
{
    private int _foodStock = GenericDataManager.MaxFoodCount;

    private bool _isFinished = false;

    public void CarTakeHit()
    {
        if (!_isFinished)
        {
            _foodStock -= GenericDataManager.FoodDecreaseInOneTake;
            if (_foodStock <= 0)
            {
                _isFinished = true;
                GameMainManager.Instance.PlayerDestroyed(GenericDataManager.PlayerDiedBecauseOfFoodStockFinished);
            }
            else
            {
                MainUIManager.Instance.UpdateFoodStatusInGame(_foodStock);
            }
        }
    }
}
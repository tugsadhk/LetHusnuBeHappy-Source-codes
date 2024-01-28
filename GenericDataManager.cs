public static class GenericDataManager
{
    //Fire Related
    public static readonly float DefaultAttackCooldownTime = 0.5f;

    //Explosion
    public static readonly float MaxForce = 7f;
    public static readonly float DestroyingTime = 6f;
    public static readonly float ExplosionRadius = 10f;

    public static readonly float ForceMultiplicationChange = 0.5f;
    public static readonly float AgentChaseProbability = 0.75f;
    public static readonly float AgentDefaultVisionRange = 18f;

    //Agent Creation Parameters
    public static readonly float NormalAgentCreationProbability = 0.5f;
    public static readonly float SlowAgentCreationProbability = 0.2f;
    public static readonly float FastAgentCreationProbability = 0.3f;
    public static readonly float AgentScaleOffsetLimit = 0.2f;

    public static readonly float PlayerClearanceRadiusOnStart = 15f;
    public static readonly float AgentCreationAfterBuildingDistance = 10f;
    public static readonly float AgentCreationDistanceForForwardFromPlayer = 40f;
    public static readonly float AgentCreationDistanceForBackwardFromPlayer = 10f;

    public static readonly int MaximumAgentCount = 250;
    public static readonly int AgentsCountToSpawnOnStart = 150;
    public static readonly int MinimumAcceptableAgentCount = 40;
    public static readonly int MaxNumberOfAgentCreatedAtOnCreation = 15;
    public static readonly int DistanceThresholdToPlayerForDestroyItself = 25;

    //Agent Runtime Parameters
    public static readonly float AgentMaxSpeed = 12f;
    public static readonly float AgentMinSpeed = 7.5f;
    public static readonly float AgentStartSpeedOffset = 1.5f;

    //Vehicle
    public static readonly float VehicleHitCooldownTime = 0.5f;
    public static readonly int MaxFoodCount = 100;
    public static readonly int FoodDecreaseInOneTake = 10;

    //Sound
    public static readonly float OneShotSoundCooldownTime = 2.2f;

    //Strings
    public static readonly string PlayerDiedBecauseOfCrashedBuilding = "Did you get your license from the butcher? \n\nPlayer 1 is guilty";
    public static readonly string PlayerDiedBecauseOfFoodStockFinished = "You can't see in front of your nose \n\nPlayer 2 is guilty";
    public static readonly string SurvivalTimeText = "Survival Time:";
    public static readonly string TotalBlockedAgentCountText = "Number of Blocked People:";
}
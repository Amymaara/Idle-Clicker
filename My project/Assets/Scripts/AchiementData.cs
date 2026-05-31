using UnityEngine;

public enum AchievementRewardType
{
    Profit,
    Speed,
    ApprenticeSpeed
}

[System.Serializable]
public class AchievementData
{
    public string id;
    public string title;
    public string description;

    public AchievementRewardType rewardType;
    public float rewardValue;

    public double targetValue;
    public bool unlocked;
}

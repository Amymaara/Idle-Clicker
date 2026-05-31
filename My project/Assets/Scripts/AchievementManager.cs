using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager Instance { get; private set; }

    [Header("Achievements")]
    [SerializeField] private List<AchievementData> achievements = new();

    [Header("Goal Box UI")]
    [SerializeField] private TMP_Text goalTitleText;
    [SerializeField] private TMP_Text goalDescriptionText;
    [SerializeField] private TMP_Text goalRewardText;
    [SerializeField] private TMP_Text goalProgressText;

    public float ProfitBonus { get; private set; } = 1f;
    public float SpeedBonus { get; private set; } = 1f;
    public float ApprenticeSpeedBonus { get; private set; } = 1f;

    private AchievementData currentGoal;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SelectNextGoal();
        UpdateGoalUI();
    }

    public void AddProgress(string id, double amount)
    {
        AchievementData achievement = achievements.Find(a => a.id == id);

        if (achievement == null) return;
        if (achievement.unlocked) return;

        achievement.targetValue -= amount;

        if (achievement.targetValue <= 0)
        {
            UnlockAchievement(achievement);
        }

        SelectNextGoal();
        UpdateGoalUI();
    }

    public void CheckAchievement(string id, double currentValue)
    {
        AchievementData achievement = achievements.Find(a => a.id == id);

        if (achievement == null) return;
        if (achievement.unlocked) return;

        if (currentValue >= achievement.targetValue)
        {
            UnlockAchievement(achievement);
        }

        SelectNextGoal();
        UpdateGoalUI();
    }

    private void UnlockAchievement(AchievementData achievement)
    {
        achievement.unlocked = true;

        switch (achievement.rewardType)
        {
            case AchievementRewardType.Profit:
                ProfitBonus += achievement.rewardValue;
                break;

            case AchievementRewardType.Speed:
                SpeedBonus += achievement.rewardValue;
                break;

            case AchievementRewardType.ApprenticeSpeed:
                ApprenticeSpeedBonus += achievement.rewardValue;
                break;
        }

        Debug.Log("Achievement Unlocked: " + achievement.title);
    }

    private void SelectNextGoal()
    {
        currentGoal = null;

        foreach (AchievementData achievement in achievements)
        {
            if (achievement.unlocked) continue;

            if (currentGoal == null)
            {
                currentGoal = achievement;
                continue;
            }

            if (achievement.targetValue < currentGoal.targetValue)
            {
                currentGoal = achievement;
            }
        }
    }

    private void UpdateGoalUI()
    {
        if (currentGoal == null)
        {
            goalTitleText.text = "All Goals Complete!";
            goalDescriptionText.text = "You have unlocked every achievement.";
            goalRewardText.text = "";
            goalProgressText.text = "";
            return;
        }

        goalTitleText.text = currentGoal.title;
        goalDescriptionText.text = currentGoal.description;

        goalRewardText.text =
            "Reward: +" + (currentGoal.rewardValue * 100f).ToString("F0") +
            "% " + currentGoal.rewardType;

        goalProgressText.text =
            "Target Remaining: " + currentGoal.targetValue.ToString("F0");
    }
}
using System.Collections.Generic;
using UnityEngine;

public class CharmManager : MonoBehaviour
{
    public static CharmManager Instance { get; private set; }

    [Header("Charm Slot Settings")]
    [SerializeField] private double slotUnlockCost = 5000;
    [SerializeField] private int maxSlots = 3;

    private int unlockedSlots = 0;
    private readonly List<CharmCard> activeCharms = new();

    public int UnlockedSlots => unlockedSlots;
    public int MaxSlots => maxSlots;
    public double SlotUnlockCost => slotUnlockCost;
    public IReadOnlyList<CharmCard> ActiveCharms => activeCharms;

    public double ProfitMultiplier { get; private set; } = 1;
    public float SpeedMultiplier { get; private set; } = 1;
    public int GlobalPotionBonus { get; private set; } = 0;
    public float CostReductionMultiplier { get; private set; } = 1;
    public float ApprenticeSpeedMultiplier { get; private set; } = 1;
    public bool ChaosActive { get; private set; } = false;

    private void Awake()
    {
        Instance = this;
    }

    public bool CanAssignCharm()
    {
        return unlockedSlots > activeCharms.Count;
    }

    public bool IsCharmActive(CharmCard charm)
    {
        return activeCharms.Contains(charm);
    }

    public void UnlockCharmSlot()
    {
        if (unlockedSlots >= maxSlots) return;

        if (CurrencyManager.Instance.CanAfford(slotUnlockCost))
        {
            CurrencyManager.Instance.SpendCoins(slotUnlockCost);
            unlockedSlots++;
            RecalculateEffects();
        }
    }

    public void AssignCharm(CharmCard charm)
    {
        if (!charm.IsBought) return;
        if (activeCharms.Contains(charm)) return;
        if (!CanAssignCharm()) return;

        activeCharms.Add(charm);
        RecalculateEffects();
    }

    public void RemoveCharm(CharmCard charm)
    {
        if (!activeCharms.Contains(charm)) return;

        activeCharms.Remove(charm);
        RecalculateEffects();
    }

    private void RecalculateEffects()
    {
        ProfitMultiplier = 1;
        SpeedMultiplier = 1;
        GlobalPotionBonus = 0;
        CostReductionMultiplier = 1;
        ApprenticeSpeedMultiplier = 1;
        ChaosActive = false;

        foreach (CharmCard charm in activeCharms)
        {
            switch (charm.CharmType)
            {
                case CharmType.Profit:
                    ProfitMultiplier *= charm.EffectValue;
                    break;

                case CharmType.Speed:
                    SpeedMultiplier *= charm.EffectValue;
                    break;

                case CharmType.Growth:
                    GlobalPotionBonus += Mathf.RoundToInt(charm.EffectValue);
                    break;

                case CharmType.CostReduction:
                    CostReductionMultiplier *= 1f - charm.EffectValue;
                    break;

                case CharmType.ApprenticeSpeed:
                    ApprenticeSpeedMultiplier *= charm.EffectValue;
                    break;

                case CharmType.Chaos:
                    ChaosActive = true;
                    break;
            }
        }
    }

    public void ResetForPrestige()
    {
        unlockedSlots = 0;
        activeCharms.Clear();

        RecalculateEffects();
    }

    public double ApplyChaosBonus(double amount)
    {
        if (!ChaosActive) return amount;

        bool triggered = Random.value <= 0.15f;

        if (!triggered) return amount;

        float multiplier = Random.Range(2f, 5f);
        return amount * multiplier;
    }
}

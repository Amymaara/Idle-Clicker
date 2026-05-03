using System.Collections.Generic;
using UnityEngine;

public class CharmManager : MonoBehaviour
{
    public static CharmManager Instance { get; private set; }

    public double ProfitMultiplier { get; private set; } = 1;
    public float SpeedMultiplier { get; private set; } = 1;

    public IReadOnlyList<CharmCard> ActiveCharms => activeCharms;

    [Header("Charm Slot Settings")]
    [SerializeField] private double slotUnlockCost = 1000;
    [SerializeField] private int maxSlots = 3;

    private int unlockedSlots = 0;
    private readonly List<CharmCard> activeCharms = new();

    public int UnlockedSlots => unlockedSlots;
    public int MaxSlots => maxSlots;
    public double SlotUnlockCost => slotUnlockCost;

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
            RecalculateMultipliers();
        }
    }

    public void AssignCharm(CharmCard charm)
    {
        if (!charm.IsBought) return;
        if (activeCharms.Contains(charm)) return;
        if (!CanAssignCharm()) return;

        activeCharms.Add(charm);
        RecalculateMultipliers();
    }

    public void RemoveCharm(CharmCard charm)
    {
        if (!activeCharms.Contains(charm)) return;

        activeCharms.Remove(charm);
        RecalculateMultipliers();
    }

    private void RecalculateMultipliers()
    {
        ProfitMultiplier = 1;
        SpeedMultiplier = 1;

        foreach (CharmCard charm in activeCharms)
        {
            ProfitMultiplier *= charm.ProfitMultiplier;
            SpeedMultiplier *= charm.SpeedMultiplier;
        }
    }

}

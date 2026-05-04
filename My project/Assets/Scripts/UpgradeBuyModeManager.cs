using System;
using UnityEngine;

public enum UpgradeBuyMode
{
    One,
    Ten,
    Max
}

public class UpgradeBuyModeManager : MonoBehaviour
{
    public static UpgradeBuyModeManager Instance { get; private set; }

    public event Action OnBuyModeChanged;

    public UpgradeBuyMode CurrentMode { get; private set; } = UpgradeBuyMode.One;

    private void Awake()
    {
        Instance = this;
    }

    public void SetBuyOne()
    {
        CurrentMode = UpgradeBuyMode.One;
        OnBuyModeChanged?.Invoke();
    }

    public void SetBuyTen()
    {
        CurrentMode = UpgradeBuyMode.Ten;
        OnBuyModeChanged?.Invoke();
    }

    public void SetBuyMax()
    {
        CurrentMode = UpgradeBuyMode.Max;
        OnBuyModeChanged?.Invoke();
    }
}
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum ApprenticeBuyMode
{
    One,
    Ten,
    Max
}

public class ApprenticeBuyModeManager : MonoBehaviour
{
    public static ApprenticeBuyModeManager Instance { get; private set; }

    public event Action OnBuyModeChanged;

    [SerializeField] private Button buyModeButton;
    [SerializeField] private TMP_Text buyModeButtonText;

    public ApprenticeBuyMode CurrentMode { get; private set; } = ApprenticeBuyMode.One;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        buyModeButton.onClick.AddListener(CycleBuyMode);
        UpdateUI();
    }

    public void CycleBuyMode()
    {
        if (CurrentMode == ApprenticeBuyMode.One)
            CurrentMode = ApprenticeBuyMode.Ten;
        else if (CurrentMode == ApprenticeBuyMode.Ten)
            CurrentMode = ApprenticeBuyMode.Max;
        else
            CurrentMode = ApprenticeBuyMode.One;

        UpdateUI();
        OnBuyModeChanged?.Invoke();
    }

    private void UpdateUI()
    {
        buyModeButtonText.text = CurrentMode switch
        {
            ApprenticeBuyMode.One => "Buy Mode: x1",
            ApprenticeBuyMode.Ten => "Buy Mode: x10",
            ApprenticeBuyMode.Max => "Buy Mode: Max",
            _ => "Buy Mode: x1"
        };
    }
}
using UnityEngine;

public class PanelManager : MonoBehaviour
{

    [SerializeField] private CanvasGroup mainPanel;
    [SerializeField] private CanvasGroup upgradePanel;
    [SerializeField] private CanvasGroup apprenticePanel;
    [SerializeField] private CanvasGroup charmPanel;

    private void Start()
    {
        ShowMain();
    }

    public void ShowMain()
    {
        ShowPanel(mainPanel);
        HidePanel(upgradePanel);
        HidePanel(apprenticePanel);
        HidePanel(charmPanel);
    }

    public void ShowUpgrade()
    {
        HidePanel(mainPanel);
        ShowPanel(upgradePanel);
        HidePanel(apprenticePanel);
        HidePanel(charmPanel);
    }

    public void ShowApprentice()
    {
        HidePanel(mainPanel);
        HidePanel(upgradePanel);
        ShowPanel(apprenticePanel);
        HidePanel(charmPanel);
    }

    public void ShowCharms()
    {
        HidePanel(mainPanel);
        HidePanel(upgradePanel);
        HidePanel(apprenticePanel);
        ShowPanel(charmPanel);
    }

    private void ShowPanel(CanvasGroup panel)
    {
        panel.alpha = 1;
        panel.interactable = true;
        panel.blocksRaycasts = true;
    }

    private void HidePanel(CanvasGroup panel)
    {
        panel.alpha = 0;
        panel.interactable = false;
        panel.blocksRaycasts = false;
    }
}

using UnityEngine;

public class BuyModeTutorialController : MonoBehaviour
{
    [Header("Tutorial Targets")]
    [SerializeField] private RectTransform buyModeButtonTarget;

    [Header("Potion Rows")]
    [SerializeField] private PotionRowUI[] potionRows;

    private bool hasShownBuyModeTutorial = false;
    private bool hasShownBulkBuyTutorial = false;

    public void TryShowBuyModeTutorial()
    {
        if (hasShownBuyModeTutorial) return;
        if (TutorialManager.Instance == null) return;

        if (!AnyPotionReached25()) return;

        hasShownBuyModeTutorial = true;

        TutorialManager.Instance.ShowTutorial(
            "You unlocked Buy Modes! Switch between x1, x10, or Max purchases.",
            buyModeButtonTarget,
            TutorialAction.OpenBuyMode
        );
    }

    public void OnBuyModeChanged()
    {
        if (TutorialManager.Instance == null) return;

        TutorialManager.Instance.TryCompleteTutorial(TutorialAction.OpenBuyMode);

        if (hasShownBulkBuyTutorial) return;

        hasShownBulkBuyTutorial = true;

        TutorialManager.Instance.ShowInfoTutorial(
            "Buy Mode lets you switch between x1, x10, or Max purchases. Use it to level potions faster without clicking one upgrade at a time.",
            buyModeButtonTarget,
            3f
        );
    }
    
    private bool AnyPotionReached25()
    {
        foreach (PotionRowUI potion in potionRows)
        {
            if (potion != null && potion.PotionLevel >= 25)
            {
                return true;
            }
        }

        return false;
    }
}
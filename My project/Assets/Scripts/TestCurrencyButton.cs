using UnityEngine;

public class TestCurrencyButton : MonoBehaviour
{
  public void AddTestCoins()
    {
        CurrencyManager.Instance.AddCoins(10);
    }
}

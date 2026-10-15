using UnityEngine;

public class PaymentKeyboard : MonoBehaviour
{
    [Header("Hotkeys")]
    public KeyCode confirmKey = KeyCode.Return;
    public KeyCode addMoneyKey = KeyCode.M;
    public KeyCode customerLeavesKey = KeyCode.X;
    public KeyCode resetChangeKey = KeyCode.Backspace;
    public KeyCode giveFullChangeKey = KeyCode.Q;

    void Update()
    {
        PaymentManager pm = PaymentManager.Instance;

        if (pm == null)
            return;

        if (Input.GetKeyDown(confirmKey))
        {
            Debug.Log("[Payment] Нажата клавиша подтверждения (Confirm).");
            pm.Confirm();
        }

        if (Input.GetKeyDown(addMoneyKey))
            pm.AddMoney();

        if (Input.GetKeyDown(customerLeavesKey))
            pm.CustomerLeaves();

        if (Input.GetKeyDown(resetChangeKey))
            pm.ResetChange();

        if (Input.GetKeyDown(giveFullChangeKey))
            pm.GiveFullChange();
    }
}

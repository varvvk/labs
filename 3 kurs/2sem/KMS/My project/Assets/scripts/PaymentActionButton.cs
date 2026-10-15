using UnityEngine;

public class PaymentActionButton : MonoBehaviour
{
    public enum Action
    {
        StartPayment,
        TapCard,
        AcceptCash,
        AddMoney,
        CustomerLeaves,
        ConfirmChange,
        ResetChange
    }

    public Action action;

    void OnMouseDown()
    {
        Debug.Log($"[Payment] OnMouseDown на {name}, action={action}");
        Trigger();
    }

    public void Trigger()
    {
        PaymentManager pm = PaymentManager.Instance;

        if (pm == null)
            return;

        switch (action)
        {
            case Action.StartPayment:
                pm.StartPayment();
                break;
            case Action.TapCard:
                pm.TapCard();
                break;
            case Action.AcceptCash:
                pm.AcceptCash();
                break;
            case Action.AddMoney:
                pm.AddMoney();
                break;
            case Action.CustomerLeaves:
                pm.CustomerLeaves();
                break;
            case Action.ConfirmChange:
                pm.ConfirmChange();
                break;
            case Action.ResetChange:
                pm.ResetChange();
                break;
        }
    }
}

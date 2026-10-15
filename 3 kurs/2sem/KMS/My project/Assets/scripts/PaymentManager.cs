using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PaymentManager : MonoBehaviour
{
    public static PaymentManager Instance;

    public enum Method { Card, Cash }
    public enum Phase { Idle, AwaitingPay, Card, Cash, Change }

    // Сценарии заданий ПРАКТИКИ.
    public enum Scenario
    {
        CardCorrect,        // Задание 1: оплата картой, сумма верная
        CashExact,          // Задание 2: наличные, ровная сумма
        CashChange,         // Задание 3: наличные, нужна сдача
        CashShortLeave,     // Задание 4: наличные, недостаточно, клиент уходит без покупок
        CashShortAddMoney   // Задание 5: наличные, недостаточно, клиент даёт ещё денег
    }

    [Header("Customer Cash Bills")]
    public float[] customerBills;

    [Header("Button")]
    public GameObject payButtonObject;

    [Header("Top HUD")]
    public TMP_Text receivedText;          // Получено (от клиента)
    public TMP_Text hudTotalText;          // Итого на Canvas
    public TMP_Text changeText;

    [Header("Practice Popup")]
    public GameObject fillTablePopup;       // всплывающая панель "Заполните таблицу"

    [Header("Receipt Printer")]
    public checkprint receiptPrinter;       // принтер чека (анимация печати)

    [Header("Cash Buttons")]
    public Button acceptCashButton;
    public Button addMoneyButton;
    public Button customerLeaveButton;
    public Button confirmChangeButton;

    public string currencySymbol;

    public Phase CurrentPhase { get; private set; } = Phase.Idle;

    public Scenario CurrentScenario { get; private set; }

    private Customer currentCustomer;
    private Method method;
    private int amountDue;
    private int offered;
    private int changeDue;
    private int changeGiven;
    private Coroutine hidePopupCoroutine;

    public static int ToKopecks(float rubles)
    {
        return Mathf.RoundToInt(rubles * 100f);
    }

    string Money(int kopecks)
    {
        int rub = kopecks / 100;
        int kop = kopecks % 100;
        return $"{rub},{kop:00} {currencySymbol}";
    }

    void Awake()
    {
        Instance = this;
    }

    public void BeginCustomer(Customer customer, Scenario scenario)
    {
        currentCustomer = customer;
        CurrentScenario = scenario;
        CurrentPhase = Phase.AwaitingPay;
        ResetUI();
    }

    public void EndCustomer(Customer customer)
    {
        if (currentCustomer != customer)
            return;

        currentCustomer = null;
        CurrentPhase = Phase.Idle;
        ResetUI();
    }

    public void StartPayment()
    {
        Debug.Log($"[Payment] StartPayment: customer={(currentCustomer != null)}, phase={CurrentPhase}, hasItems={(CheckoutManager.Instance != null && CheckoutManager.Instance.HasItems)}");

        if (currentCustomer == null || CurrentPhase != Phase.AwaitingPay)
        {
            Debug.LogWarning("[Payment] Отмена: нет покупателя у кассы или этап не AwaitingPay.");
            return;
        }

        if (CheckoutManager.Instance == null || !CheckoutManager.Instance.HasItems)
        {
            Debug.LogWarning("[Payment] Отмена: нет отсканированных товаров.");
            return;
        }

        method = (CurrentScenario == Scenario.CardCorrect) ? Method.Card : Method.Cash;
        amountDue = ToKopecks(CheckoutManager.Instance.Total);

        if (method == Method.Card)
            StartCard();
        else
            StartCash();
    }

    void StartCard()
    {
        CurrentPhase = Phase.Card;
        offered = amountDue;

        // Запускаем анимацию оплаты картой у текущего покупателя.
        if (currentCustomer != null)
            currentCustomer.PlayCardPay();

        if (payButtonObject != null) payButtonObject.SetActive(false);
        if (hudTotalText != null) hudTotalText.text = $"Итого: {Money(amountDue)}";
        if (receivedText != null) receivedText.text = $"Получено: {Money(offered)}";
    }

    public void TapCard()
    {
        if (CurrentPhase != Phase.Card)
            return;

        CompleteSale();
    }

    void StartCash()
    {
        CurrentPhase = Phase.Cash;

        SetOfferForScenario();

        if (payButtonObject != null) payButtonObject.SetActive(false);
        
        RefreshCash();
    }

    void SetOfferForScenario()
    {
        switch (CurrentScenario)
        {
            case Scenario.CashExact:
                offered = amountDue;
                break;
            case Scenario.CashChange:
                offered = PickOverpay();
                break;
            case Scenario.CashShortLeave:
            case Scenario.CashShortAddMoney:
                offered = Mathf.Max(1, amountDue - Mathf.Max(1, amountDue / 3));
                break;
            default:
                offered = amountDue;
                break;
        }
    }

    int PickOverpay()
    {
        for (int i = 0; i < customerBills.Length; i++)
        {
            int bill = ToKopecks(customerBills[i]);
            if (bill >= amountDue)
                return bill;
        }

        int biggest = customerBills.Length > 0
            ? ToKopecks(customerBills[customerBills.Length - 1])
            : 10000;

        int count = Mathf.CeilToInt((float)amountDue / biggest);
        return count * biggest;
    }

    public void AcceptCash()
    {
        Debug.Log($"[Payment] AcceptCash: phase={CurrentPhase}, offered={offered}, due={amountDue}");

        if (CurrentPhase != Phase.Cash)
        {
            Debug.LogWarning("[Payment] AcceptCash проигнорирован: этап не Cash.");
            return;
        }

        if (offered < amountDue)
        {
            Debug.LogWarning("[Payment] Недостаточно средств.");
            return;
        }

        changeDue = offered - amountDue;

        if (changeDue == 0)
        {
            Debug.Log("[Payment] Ровная сумма, продажа завершена.");
            CompleteSale();
            return;
        }

        Debug.Log($"[Payment] Нужна сдача: {changeDue} коп. Кликай по ячейкам ящика.");
        StartChange();
    }

    public void AddMoney()
    {
        if (CurrentPhase != Phase.Cash)
            return;

        offered = PickOverpay();
        RefreshCash();
    }

    public void CustomerLeaves()
    {
        if (currentCustomer == null)
            return;

        Finish(false);
    }

    void StartChange()
    {
        CurrentPhase = Phase.Change;
        changeGiven = 0;

        RefreshChange();
    }

    public void GiveChange(int value)
    {
        Debug.Log($"[Payment] GiveChange: phase={CurrentPhase}, value={value}, given={changeGiven}, due={changeDue}");

        if (CurrentPhase != Phase.Change)
        {
            Debug.LogWarning("[Payment] GiveChange проигнорирован: этап не Change (сначала прими деньги — Enter).");
            return;
        }

        if (changeGiven + value > changeDue)
        {
            return;
        }

        changeGiven += value;
        RefreshChange();
    }

    public void ResetChange()
    {
        if (CurrentPhase != Phase.Change)
            return;

        changeGiven = 0;
        RefreshChange();
    }

    // Выдать всю сдачу из ящика одной операцией (клавиша Q).
    public void GiveFullChange()
    {
        if (CurrentPhase != Phase.Change)
            return;

        changeGiven = changeDue;
        RefreshChange();
    }

    public void ConfirmChange()
    {
        if (CurrentPhase != Phase.Change)
            return;

        if (changeGiven != changeDue)
           return;

        CompleteSale();
    }

    public void Confirm()
    {
        switch (CurrentPhase)
        {
            case Phase.AwaitingPay:
                StartPayment();
                break;
            case Phase.Card:
                TapCard();
                break;
            case Phase.Cash:
                AcceptCash();
                break;
            case Phase.Change:
                ConfirmChange();
                break;
        }
    }

    void CompleteSale()
    {
        Finish(true);
    }

    void Finish(bool paid)
    {
        // Товары пропадают, клиент уходит.
        if (CashDesk.Instance != null)
            CashDesk.Instance.ClearProducts();

        Customer customer = currentCustomer;
        currentCustomer = null;
        CurrentPhase = Phase.Idle;

        HidePaymentPanels();

        // Печать чека при успешном завершении задания (продажа состоялась).
        if (paid && receiptPrinter != null)
            receiptPrinter.Print();

        if (customer != null)
            customer.FinishCheckout(paid);

        // Показываем popup на 2 секунды после ухода клиента
        if (hidePopupCoroutine != null)
            StopCoroutine(hidePopupCoroutine);
        hidePopupCoroutine = StartCoroutine(ShowFillTablePopup());
    }

    IEnumerator ShowFillTablePopup()
    {
        // Появляется через 2 секунды после ухода клиента.
        yield return new WaitForSeconds(2f);

        if (fillTablePopup != null)
            fillTablePopup.SetActive(true);

        yield return new WaitForSeconds(3f);

        if (fillTablePopup != null)
            fillTablePopup.SetActive(false);
    }


    void HidePaymentPanels()
    {
        if (payButtonObject != null) payButtonObject.SetActive(false);
    }

    void RefreshCash()
    {
        if (hudTotalText != null)
            hudTotalText.text = $"Итого: {Money(amountDue)}";

        if (receivedText != null)
            receivedText.text = $"Получено: {Money(offered)}";

        bool enough = offered >= amountDue;

        if (acceptCashButton != null)
            acceptCashButton.interactable = enough;

        if (addMoneyButton != null)
            addMoneyButton.gameObject.SetActive(!enough);

        if (customerLeaveButton != null)
            customerLeaveButton.gameObject.SetActive(!enough);
    }

    void RefreshChange()
    {
        if (changeText != null)
            changeText.text = $"Сдача: {Money(changeGiven)} / {Money(changeDue)}";

        if (confirmChangeButton != null)
            confirmChangeButton.interactable = changeGiven == changeDue;
    }

    void ResetUI()
    {
        if (payButtonObject != null)
            payButtonObject.SetActive(CurrentPhase == Phase.AwaitingPay);

        if (hudTotalText != null) hudTotalText.text = "";
        if (receivedText != null) receivedText.text = "";
        if (changeText != null) changeText.text = "";
    }
}

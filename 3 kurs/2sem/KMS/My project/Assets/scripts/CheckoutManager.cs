using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class CheckoutManager : MonoBehaviour
{
    public static CheckoutManager Instance;

    public TMP_Text itemsText;
    public TMP_Text totalText;
    public TMP_Text timeText;
    public string currencySymbol = "\u20bd";

    private readonly List<Product> scannedItems = new List<Product>();
    private float total;

    public float Total => total;
    public int ItemCount => scannedItems.Count;
    public bool HasItems => scannedItems.Count > 0;

    void Awake()
    {
        Instance = this;
        ResetReceipt();
    }

    void Update()
    {
        if (timeText != null)
            timeText.text = DateTime.Now.ToString("HH:mm");
    }

    public void AddProduct(Product product)
    {
        scannedItems.Add(product);
        total += product.price * product.quantity;
        UpdateReceipt();
    }

    public void ResetReceipt()
    {
        scannedItems.Clear();
        total = 0f;
        UpdateReceipt();
    }

    void UpdateReceipt()
    {
        if (itemsText != null)
        {
            StringBuilder sb = new StringBuilder();

            foreach (Product item in scannedItems)
            {
                float lineSum = item.price * item.quantity;
                sb.AppendLine(
                    $"{item.productName}\t{item.quantity}\t{lineSum.ToString("0.00").Replace('.', ',')} {currencySymbol}");
            }

            itemsText.text = sb.ToString();
        }

        if (totalText != null)
            totalText.text = $"{total.ToString("0.00").Replace('.', ',')} {currencySymbol}";
    }
}

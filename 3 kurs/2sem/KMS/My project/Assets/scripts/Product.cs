using UnityEngine;

public class Product : MonoBehaviour
{
    public string productName = "Product";
    public float price = 1f;
    public int quantity = 1;

    [HideInInspector] public bool scanned = false;
}

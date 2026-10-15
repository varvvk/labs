using System.Collections.Generic;
using UnityEngine;

public class CashDesk : MonoBehaviour
{
    public static CashDesk Instance { get; private set; }

    public GameObject[] productPrefabs;
    public Transform[] productSpawnPoints;

    public int minProducts = 1;
    public int maxProducts = 3;

    public Vector3[] productScales;

    private readonly List<GameObject> spawnedProducts = new List<GameObject>();

    void Awake()
    {
        Instance = this;
    }

    public void ShowProducts()
    {

        if (CheckoutManager.Instance != null)
            CheckoutManager.Instance.ResetReceipt();

        if (productPrefabs.Length == 0 || productSpawnPoints.Length == 0)
            return;

        int count = Random.Range(minProducts, maxProducts + 1);
        count = Mathf.Min(count, productSpawnPoints.Length);

        List<int> freeSlots = new List<int>();
        for (int i = 0; i < productSpawnPoints.Length; i++)
            freeSlots.Add(i);

        for (int i = 0; i < count; i++)
        {
            int slotIndex = Random.Range(0, freeSlots.Count);
            Transform slot = productSpawnPoints[freeSlots[slotIndex]];
            freeSlots.RemoveAt(slotIndex);

            int prefabIndex = Random.Range(0, productPrefabs.Length);
            GameObject prefabToSpawn = productPrefabs[prefabIndex];

            GameObject product =
                Instantiate(prefabToSpawn, slot.position, slot.rotation);

            if (productScales != null && prefabIndex < productScales.Length
                && productScales[prefabIndex] != Vector3.zero)
                product.transform.localScale = productScales[prefabIndex];

            spawnedProducts.Add(product);
        }
    }

    public void ClearProducts()
    {
        foreach (GameObject product in spawnedProducts)
        {
            if (product != null)
                Destroy(product);
        }

        spawnedProducts.Clear();
    }
}

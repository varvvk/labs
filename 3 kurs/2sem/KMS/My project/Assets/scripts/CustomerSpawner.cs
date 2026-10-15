using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    public GameObject customerPrefab;
    public Transform spawnPoint;

    public Transform[] browsePoints;
    public Transform cashDeskPoint;
    public Transform exitPoint;

    public static CustomerSpawner Instance { get; private set; }

    private Customer current;

    void Awake()
    {
        Instance = this;
    }

    // Спавн клиента по нажатию на задание ПРАКТИКИ с заданным сценарием оплаты.
    public bool Spawn(PaymentManager.Scenario scenario)
    {
        if (current != null)
        {
            Debug.LogWarning("[CustomerSpawner] Клиент уже обслуживается — дождитесь окончания задания.");
            return false;
        }

        GameObject customer =
            Instantiate(customerPrefab,
            spawnPoint.position,
            spawnPoint.rotation);

        Customer customerScript =
            customer.GetComponent<Customer>();

        customerScript.browsePoints = browsePoints;
        customerScript.cashDeskPoint = cashDeskPoint;
        customerScript.exitPoint = exitPoint;
        customerScript.scenario = scenario;

        current = customerScript;
        return true;
    }

    // Вызывается Customer при завершении обслуживания, чтобы освободить слот.
    public void OnCustomerDone(Customer customer)
    {
        if (current == customer)
            current = null;
    }
}

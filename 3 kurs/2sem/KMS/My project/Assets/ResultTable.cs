using TMPro;
using UnityEngine;

public class ResultTable : MonoBehaviour
{
    public GameObject tablePanel;

    [Header("Поля ввода")]
    public TMP_InputField inputSum;
    public TMP_InputField inputReceived;

    [Header("Столбец 'Сумма покупки'")]
    public TMP_Text[] sumCells;

    [Header("Столбец 'Получено'")]
    public TMP_Text[] receivedCells;

    [Header("Столбец 'Результат'")]
    public TMP_Text[] resultCells;

    private int currentRow = 0;

    void Start()
    {
        if (tablePanel != null)
            tablePanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            ToggleTable();
        }
    }

    public void ToggleTable()
    {
        if (tablePanel == null)
            return;

        bool open = !tablePanel.activeSelf;

        tablePanel.SetActive(open);

        if (GameUIManager.Instance != null)
        {
            if (open)
                GameUIManager.Instance.OpenWindow();
            else
                GameUIManager.Instance.CloseWindow();
        }
    }

    public void AddRecord()
    {
        if (currentRow >= sumCells.Length)
        {
            Debug.Log("Таблица заполнена");
            return;
        }

        float sum;
        float received;

        if (!float.TryParse(inputSum.text, out sum))
        {
            Debug.Log("Некорректная сумма покупки");
            return;
        }

        if (!float.TryParse(inputReceived.text, out received))
        {
            Debug.Log("Некорректная сумма полученных денег");
            return;
        }

        string result;

        if (received < sum)
        {
            result = "Отказано";
        }
        else if (Mathf.Approximately(received, sum))
        {
            result = "Успешно";
        }
        else
        {
            float change = received - sum;
            result = $"Успешно (сдача = {change:F2})";
        }

        sumCells[currentRow].text = sum.ToString("F2");
        receivedCells[currentRow].text = received.ToString("F2");
        resultCells[currentRow].text = result;

        currentRow++;

        inputSum.text = "";
        inputReceived.text = "";
    }

    public void ClearTable()
    {
        for (int i = 0; i < sumCells.Length; i++)
        {
            sumCells[i].text = "";
            receivedCells[i].text = "";
            resultCells[i].text = "";
        }

        currentRow = 0;

        inputSum.text = "";
        inputReceived.text = "";
    }
}
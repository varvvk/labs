using System;
using TMPro;
using UnityEngine;

public class PracticeMode : MonoBehaviour
{
    public static PracticeMode Instance;

    [Serializable]
    public class PracticeTask
    {
        public string title;

        [TextArea(2, 6)]
        public string hint;

        [Tooltip("Сценарий оплаты, который получит клиент этого задания.")]
        public PaymentManager.Scenario scenario = PaymentManager.Scenario.CardCorrect;
    }

    [Tooltip("Нижняя информационная панель StatusPanel (всегда на экране, не влияет на управление).")]
    public GameObject statusPanel;

    [Tooltip("Спавнер клиентов. При выборе задания создаёт клиента с нужным сценарием.")]
    public CustomerSpawner spawner;

    [Header("Элементы StatusPanel")]
    [Tooltip("Текстовое окно для заголовка задания (StatusTitle).")]
    public TMP_Text statusTitle;
    [Tooltip("Текстовое окно слева для вывода подсказок (StatusText).")]
    public TMP_Text statusText;

    [Header("Задания раздела ПРАКТИКА")]
    [TextArea(2, 5)]
    public string introHint =
        "Выполните последовательно все задания, вызывая подсказки\nщелчком мыши по соответствующим кнопкам ПРАКТИКИ.";

    public PracticeTask[] tasks;

    public int CurrentTaskIndex { get; private set; } = -1;

    public PracticeTask CurrentTask =>
        (tasks != null && CurrentTaskIndex >= 0 && CurrentTaskIndex < tasks.Length)
            ? tasks[CurrentTaskIndex]
            : null;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // StatusPanel всегда на экране и не влияет на управление.
        if (statusPanel != null)
            statusPanel.SetActive(true);

        if (statusTitle != null)
            statusTitle.text = "";

        if (statusText != null)
            statusText.text = introHint;
    }

    // Кнопки "Задание N" (передавать индекс 0..N-1)
    public void ShowTask(int index)
    {
        if (tasks == null || index < 0 || index >= tasks.Length)
        {
            Debug.LogWarning($"[PracticeMode] Нет задания с индексом {index}.");
            return;
        }

        CurrentTaskIndex = index;
        PracticeTask task = tasks[index];

        if (statusTitle != null)
            statusTitle.text = task.title;

        if (statusText != null)
            statusText.text = task.hint;

        // Очищаем текстовые поля PaymentManager сразу при выборе задания
        if (PaymentManager.Instance != null)
        {
            if (PaymentManager.Instance.hudTotalText != null)
                PaymentManager.Instance.hudTotalText.text = "";
            if (PaymentManager.Instance.receivedText != null)
                PaymentManager.Instance.receivedText.text = "";
            if (PaymentManager.Instance.changeText != null)
                PaymentManager.Instance.changeText.text = "";
        }

        // Появляется клиент с заданным способом оплаты.
        if (spawner != null)
            spawner.Spawn(task.scenario);
    }
}

using System.Collections;
using UnityEngine;

public class Customer : MonoBehaviour
{
    public Transform[] browsePoints;
    public Transform cashDeskPoint;
    public Transform exitPoint;

    public float speed = 2f;
    public float patience = 120f;

    [Tooltip("Сценарий оплаты, задаваемый заданием ПРАКТИКИ.")]
    public PaymentManager.Scenario scenario = PaymentManager.Scenario.CardCorrect;

    private Animator animator;
    private bool waitingForCheckout;

    public bool IsServed { get; private set; }

    void Start()
    {
        animator = GetComponent<Animator>();

        StartCoroutine(CustomerRoutine());
    }

    IEnumerator CustomerRoutine()
    {
        animator.SetBool("Walk", true);

        // Visit 2 random browse points
        for (int i = 0; i < 2; i++)
        {
            Transform point =
                browsePoints[Random.Range(0, browsePoints.Length)];

            yield return MoveTo(point);

            animator.SetBool("Walk", false);

            yield return new WaitForSeconds(
                Random.Range(1f, 3f));

            animator.SetBool("Walk", true);
        }

        // Go to the cash desk
        yield return MoveTo(cashDeskPoint);

        animator.SetBool("Walk", false);

        // Точное выравнивание у кассы, чтобы анимация оплаты
        // всегда проигрывалась из одинаковой позы независимо от пути.
        yield return AlignTo(cashDeskPoint);

        // Show 1-3 products on the cash desk
        if (CashDesk.Instance != null)
            CashDesk.Instance.ShowProducts();

        // Wait at the desk (serving the customer)
        waitingForCheckout = true;

        if (PaymentManager.Instance != null)
            PaymentManager.Instance.BeginCustomer(this, scenario);

        float waited = 0f;
        while (waitingForCheckout && waited < patience)
        {
            waited += Time.deltaTime;
            yield return null;
        }

        // После оплаты покупатель стоит ещё пару секунд, как будто ждёт чек.
        if (IsServed)
            yield return new WaitForSeconds(2f);

        if (PaymentManager.Instance != null)
            PaymentManager.Instance.EndCustomer(this);

        // Clear products before leaving
        if (CashDesk.Instance != null)
            CashDesk.Instance.ClearProducts();

        animator.SetBool("Walk", true);

        // Walk to the exit
        yield return MoveTo(exitPoint);

        Destroy(gameObject);
    }

    // Запуск анимации оплаты картой. Вызывается из PaymentManager при оплате картой.
    public void PlayCardPay()
    {
        if (animator != null)
            animator.SetTrigger("CardPay");
    }

    public void FinishCheckout(bool paid)
    {
        IsServed = paid;
        waitingForCheckout = false;

        // Оповещаем спавнер, что клиент освободился.
        if (CustomerSpawner.Instance != null)
            CustomerSpawner.Instance.OnCustomerDone(this);
    }

    IEnumerator MoveTo(Transform target)
    {
        while (Vector3.Distance(transform.position,
                                target.position) > 0.1f)
        {
            transform.position =
                Vector3.MoveTowards(
                    transform.position,
                    target.position,
                    speed * Time.deltaTime);

            transform.LookAt(
                new Vector3(
                    target.position.x,
                    transform.position.y,
                    target.position.z));

            yield return null;
        }
    }

    // Плавно доводит персонажа в точную позицию и поворот точки кассы,
    // чтобы стартовая поза для анимации всегда была одинаковой.
    IEnumerator AlignTo(Transform target)
    {
        float rotationSpeed = 360f;

        while (Vector3.Distance(transform.position, target.position) > 0.001f ||
               Quaternion.Angle(transform.rotation, target.rotation) > 0.5f)
        {
            transform.position =
                Vector3.MoveTowards(
                    transform.position,
                    target.position,
                    speed * Time.deltaTime);

            transform.rotation =
                Quaternion.RotateTowards(
                    transform.rotation,
                    target.rotation,
                    rotationSpeed * Time.deltaTime);

            yield return null;
        }

        transform.position = target.position;
        transform.rotation = target.rotation;
    }
}
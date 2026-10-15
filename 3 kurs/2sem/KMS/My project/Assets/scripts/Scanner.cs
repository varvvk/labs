using UnityEngine;

public class Scanner : MonoBehaviour
{
    public Transform scanPoint;
    public Transform aimSource;
    public float scanRadius = 0.25f;
    public float scanRange = 3f;
    public LayerMask productLayers = ~0;

    [Header("Sound")]
    public AudioClip scanSuccessSound;
    public AudioClip scanFailSound;
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void TryScan()
    {
        Transform t = scanPoint != null ? scanPoint : transform;
        Vector3 origin = t.position;

        Transform aim = aimSource != null ? aimSource : Camera.main?.transform;
        Vector3 direction = aim != null ? aim.forward : t.forward;

        RaycastHit[] hits = Physics.SphereCastAll(
            origin, scanRadius, direction, scanRange, productLayers,
            QueryTriggerInteraction.Collide);

        foreach (RaycastHit hit in hits)
        {
            if (TryRegister(hit.collider))
            {
                PlaySound(scanSuccessSound);
                return;
            }
        }

        Collider[] near =
            Physics.OverlapSphere(origin, scanRadius, productLayers,
                QueryTriggerInteraction.Collide);

        foreach (Collider col in near)
        {
            if (TryRegister(col))
            {
                PlaySound(scanSuccessSound);
                return;
            }
        }

        PlaySound(scanFailSound);
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
            audioSource.PlayOneShot(clip);
    }

    private bool TryRegister(Collider hit)
    {
        Product product = hit.GetComponentInParent<Product>();

        if (product != null && !product.scanned)
        {
            product.scanned = true;

            if (CheckoutManager.Instance != null)
                CheckoutManager.Instance.AddProduct(product);

            Debug.Log("Scanned: " + product.productName);
            return true;
        }

        return false;
    }
}

using UnityEngine;

public class ObjectGrabber : MonoBehaviour
{
    public Camera playerCamera;

    public float grabRange = 3f;
    public float holdDistance = 2f;
    public float moveForce = 15f;

    public LayerMask grabbableLayers = ~0;

    [Header("Hand Hold (Scanner)")]
    public Vector3 handOffset = new Vector3(0.3f, -0.3f, 0.6f);
    public Vector3 handRotation = new Vector3(0f, 0f, 0f);

    private Rigidbody heldObject;
    private bool isHeldInHand;

    private Transform scannerHomeParent;
    private Vector3 scannerHomePosition;
    private Quaternion scannerHomeRotation;
    private bool hasScannerHome;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (heldObject == null)
                TryGrab();
            else
                Release();
        }

        if (Input.GetMouseButtonDown(1) && heldObject != null)
        {
            Scanner scanner = heldObject.GetComponent<Scanner>();

            if (scanner != null)
                scanner.TryScan();
        }
    }

    void FixedUpdate()
    {
        if (heldObject == null || isHeldInHand)
            return;

        Vector3 targetPoint =
            playerCamera.transform.position +
            playerCamera.transform.forward * holdDistance;

        Vector3 direction = targetPoint - heldObject.position;
        heldObject.velocity = direction * moveForce;
    }

    void TryGrab()
    {
        Ray ray = new Ray(
            playerCamera.transform.position,
            playerCamera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, grabRange, grabbableLayers))
        {
            Rigidbody rb = hit.rigidbody;

            if (rb == null)
                return;

            bool isScanner = rb.GetComponent<Scanner>() != null;

            if (rb.isKinematic && !isScanner)
                return;

            {
                heldObject = rb;
                heldObject.useGravity = false;
                heldObject.drag = 10f;

                if (isScanner)
                {
                    scannerHomeParent = heldObject.transform.parent;
                    scannerHomePosition = heldObject.transform.position;
                    scannerHomeRotation = heldObject.transform.rotation;
                    hasScannerHome = true;

                    isHeldInHand = true;
                    heldObject.isKinematic = true;
                    heldObject.transform.SetParent(playerCamera.transform);
                    heldObject.transform.localPosition = handOffset;
                    heldObject.transform.localEulerAngles = handRotation;
                }
                else
                {
                    isHeldInHand = false;
                }
            }
        }
    }

    void Release()
    {
        if (isHeldInHand)
        {
            heldObject.transform.SetParent(scannerHomeParent);

            if (hasScannerHome)
            {
                heldObject.transform.position = scannerHomePosition;
                heldObject.transform.rotation = scannerHomeRotation;
            }

            heldObject.velocity = Vector3.zero;
            heldObject.angularVelocity = Vector3.zero;

            heldObject.drag = 0f;
            heldObject = null;
            isHeldInHand = false;
            hasScannerHome = false;
            return;
        }

        heldObject.useGravity = true;
        heldObject.drag = 0f;
        heldObject = null;
        isHeldInHand = false;
    }
}

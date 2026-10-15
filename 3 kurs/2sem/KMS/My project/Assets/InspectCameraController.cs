using UnityEngine;

public class InspectCameraController : MonoBehaviour
{
    public static InspectCameraController Instance { get; private set; }

    public bool move = false;

    private Vector3 startPosition;
    private Quaternion startRotation;

    private Vector3 needPosition;
    private Quaternion needRotation;

    public float speed = 3f;

    public Camera mainCamera;
    public Camera inspectCamera;

    private AudioListener mainListener;
    private AudioListener inspectListener;
    private bool isInspectMode;

    void Awake()
    {
        Instance = this;
        mainListener = mainCamera.GetComponent<AudioListener>();
        inspectListener = inspectCamera.GetComponent<AudioListener>();
        SetActiveCamera(useInspect: false);
    }

    public bool IsInspectMode => isInspectMode;

    public void EnterInspectMode()
    {
        if (isInspectMode) return;

        isInspectMode = true;
        SetActiveCamera(useInspect: true);
        GameUIManager.Instance.OpenWindow();
    }

    public void ExitInspectMode()
    {
        if (!isInspectMode) return;

        isInspectMode = false;
        move = false;
        SetActiveCamera(useInspect: false);
        GameUIManager.Instance.CloseWindow();
    }

    void SetActiveCamera(bool useInspect)
    {
        mainCamera.enabled = !useInspect;
        inspectCamera.enabled = useInspect;

        if (mainListener != null)
            mainListener.enabled = !useInspect;

        if (inspectListener != null)
            inspectListener.enabled = useInspect;
    }

    void BeginMove(Vector3 position, Quaternion rotation)
    {
        EnterInspectMode();

        move = true;
        startPosition = transform.position;
        startRotation = transform.rotation;
        needPosition = position;
        needRotation = rotation;
    }

    void Update()
    {
        if (!isInspectMode || !move) return;

        transform.position = Vector3.Lerp(
            transform.position,
            needPosition,
            Time.deltaTime * speed
        );

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            needRotation,
            Time.deltaTime * speed
        );

        if (Vector3.Distance(transform.position, needPosition) < 0.01f)
        {
            move = false;
        }
    }

    public void MoveToMonitor()
    {
        BeginMove(
            new Vector3(-1.517f, 3.465f, -85.864f),
            Quaternion.Euler(17.511f, 90.24f, -0.0031f)
        );
    }

    public void MoveToPrinter()
    {
        BeginMove(
            new Vector3(-0.873f, 2.982f, -85.258f),
            Quaternion.Euler(44.134f, 90.319f, 0.146f)
        );
    }

    public void MoveToScaner()
    {
        BeginMove(
            new Vector3(-1.243f, 2.913f, -86.379f),
            Quaternion.Euler(44.134f, 125.639f, 0.146f)
        );
    }

    public void MoveToTerminal()
    {
        BeginMove(
            new Vector3(-0.284f, 2.946f, -86.475f),
            Quaternion.Euler(82.746f, -90f, 0.859f)
        );
    }

    public void MoveToDrawer()
    {
        BeginMove(
            new Vector3(-1.3f, 2.896f, -85.909f),
            Quaternion.Euler(32.144f, 90f, 0.068f)
        );
    }
}

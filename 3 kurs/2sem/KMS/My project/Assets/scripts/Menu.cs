using UnityEngine;

public class Menu : MonoBehaviour
{
    public GameObject menuPanel;

    private bool isOpen;

    void Start()
    {
        menuPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleMenu();
        }
    }

    public void ToggleMenu()
    {
        if (isOpen)
            CloseMenu();
        else
            OpenMenu();
    }

    public void OpenMenu()
    {
        menuPanel.SetActive(true);
        GameUIManager.Instance.OpenWindow();

        isOpen = true;
    }

    public void CloseMenu()
    {
        menuPanel.SetActive(false);

        if (InspectCameraController.Instance != null)
            InspectCameraController.Instance.ExitInspectMode();

        GameUIManager.Instance.CloseWindow();

        isOpen = false;
    }
}

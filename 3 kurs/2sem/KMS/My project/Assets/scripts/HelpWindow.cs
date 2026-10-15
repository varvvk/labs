using UnityEngine;

public class HelpWindow : MonoBehaviour
{
    public GameObject helpPanel;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (helpPanel.activeSelf)
                Close();
            else
                Open();
        }
    }

    public void Open()
    {
        if (helpPanel.activeSelf) return;

        helpPanel.SetActive(true);
        GameUIManager.Instance.OpenWindow();
    }

    public void Close()
    {
        if (!helpPanel.activeSelf) return;

        helpPanel.SetActive(false);
        GameUIManager.Instance.CloseWindow();
    }
}
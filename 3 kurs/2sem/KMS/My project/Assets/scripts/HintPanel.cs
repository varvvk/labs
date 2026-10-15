using UnityEngine;

public class HintPanel : MonoBehaviour
{
    public GameObject panel;

    void Start()
    {
        panel.SetActive(true);

        GameUIManager.Instance.OpenWindow();
    }

    public void CloseTutorial()
    {
        panel.SetActive(false);

        GameUIManager.Instance.CloseWindow();
    }
}
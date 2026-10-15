using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIInfo : MonoBehaviour
{
    public GameObject infoPanel;
    public TMP_Text infoText;

    public void ShowMonitor()
    {
        infoPanel.SetActive(true);
        infoText.text =
            "Монитор";
    }

    public void ShowPrinter()
    {
        infoPanel.SetActive(true);
        infoText.text =
            "Принтер";
    }

    public void ShowScaner()
    {
        infoPanel.SetActive(true);
        infoText.text =
            "Сканер";
    }

    public void ShowTerminal()
    {
        infoPanel.SetActive(true);
        infoText.text =
            "Терминал";
    }

    public void ShowDrawer()
    {
        infoPanel.SetActive(true);
        infoText.text =
            "ящик";
    }

    public void HideInfo()
    {
        infoPanel.SetActive(false);
    }
}
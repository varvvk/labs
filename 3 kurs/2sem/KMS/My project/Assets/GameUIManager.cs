using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager Instance;

    public FPSController player;

    private int openWindows = 0;

    void Awake()
    {
        Instance = this;
    }

    void SetGameplayMode()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void SetUIMode()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void OpenWindow()
    {
        openWindows++;
        player.canMove = false;
        SetUIMode();
    }

    public void CloseWindow()
    {
        openWindows--;

        if (openWindows < 0)
            openWindows = 0;

        if (openWindows == 0)
        {
            player.canMove = true;

            SetGameplayMode();
        }
    }
}
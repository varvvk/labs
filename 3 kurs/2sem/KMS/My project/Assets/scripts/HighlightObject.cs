using UnityEngine;

public class HighlightObject : MonoBehaviour
{
    private readonly Color highlightColor = new Color(1f, 0.98f, 0.94f) * 0.8f;

    public void Highlight(Renderer target)
    {
        target.material.EnableKeyword("_EMISSION");
        target.material.SetColor("_EmissionColor", highlightColor);
    }

    public void UnHighlight(Renderer target)
    {
        target.material.SetColor("_EmissionColor", Color.black);
    }
}
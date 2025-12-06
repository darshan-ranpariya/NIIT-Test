using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public abstract class BaseScreen : MonoBehaviour
{
    protected UIManager _uiManager;
    protected CanvasGroup _canvasGroup;

    protected virtual void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _uiManager = UIManager.inst; // Simple service locator
    }

    public virtual void Show()
    {
        gameObject.SetActive(true);
        _canvasGroup.alpha = 1;
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
    }

    public virtual void Hide()
    {
        _canvasGroup.alpha = 0;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
        gameObject.SetActive(false);
    }
}

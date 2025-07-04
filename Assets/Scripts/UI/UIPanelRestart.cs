using UnityEngine;
using UnityEngine.UI;

public class UIPanelRestart : MonoBehaviour, IMenu
{
    [SerializeField] private Button btnRestart;

    private UIMainManager m_mngr;

    private void Awake()
    {
        btnRestart.onClick.AddListener(OnClick);
    }

    private void OnDestroy()
    {
        if (btnRestart) btnRestart.onClick.RemoveAllListeners();
    }

    public void Setup(UIMainManager mngr)
    {
        m_mngr = mngr;
    }

    private void OnClick()
    {
        m_mngr.RestartCurrentLevel();
    }
    
    public void Show()
    {
    }

    public void Hide()
    {
    }
}


using UnityEngine;
using UnityEngine.UI;

public class MenuBuilder : MonoBehaviour
{
    [SerializeField] private Button optionButton;
    [SerializeField] private Button helpButton;
    [SerializeField] private Button backButtonOption;
    [SerializeField] private Button backButtonHelp;

    // 外部へ通知するイベント
    public System.Action<string> OnCommand;

    void Start()
    {
        if (optionButton)
            optionButton.onClick.AddListener(() => OnCommand?.Invoke("Option"));
        if (helpButton)
            helpButton.onClick.AddListener(() => OnCommand?.Invoke("Help"));
        if (backButtonOption)
            backButtonOption.onClick.AddListener(() => OnCommand?.Invoke("Back"));
        if (backButtonHelp)
            backButtonHelp.onClick.AddListener(() => OnCommand?.Invoke("Back"));
    }
}

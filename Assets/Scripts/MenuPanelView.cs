using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuPanelView : MonoBehaviour
{
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject optionPanel;
    [SerializeField] private GameObject helpPanel;

    public Button nextButton;   // 試薬を切り替えるボタン
    public TextMeshProUGUI currentLabel;          // ボタン上のテキスト（Current）
    public TextMeshProUGUI nextLabel;             // 次に切り替わる試薬名（Next）
    public void ShowMenu()
    {
        menuPanel.SetActive(true);
        optionPanel.SetActive(false);
        helpPanel.SetActive(false);
    }

    public void ShowOption()
    {
        menuPanel.SetActive(false);
        optionPanel.SetActive(true);
        helpPanel.SetActive(false);
    }

    public void ShowHelp()
    {
        menuPanel.SetActive(false);
        optionPanel.SetActive(false);
        helpPanel.SetActive(true);
    }
}

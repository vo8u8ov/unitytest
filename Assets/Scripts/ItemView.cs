using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemView : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text subText;
    [SerializeField] private Button button;

    public KitItem Bound { get; private set; }
    public System.Action<ItemView> OnClick;

    public void Setup(KitItem kit)
    {
        Bound = kit;
        if (nameText) nameText.text = kit.display;
        if (subText)  subText.text  = kit.subtext.ToString(); // %は付けない運用に
        if (button)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => OnClick?.Invoke(this));
        }
    }

    // public void SetSelectedColor(Color c)  { if (button) button.image.color = c; }
    // public void SetNormalColor(Color c)    { if (button) button.image.color = c; }
}

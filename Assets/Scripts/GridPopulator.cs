using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GridPopulator : MonoBehaviour
{
    [SerializeField] private RectTransform content;
    [SerializeField] private GameObject gridItemPrefab;
    [SerializeField] private Color selectedColor = new(0.7f, 0.9f, 1f, 1f);

    private readonly Dictionary<Button, Color> originalColors = new();
    private Button currentSelectedButton;

    public System.Action<KitItem> OnItemClicked; // ← Presenter/Viewへ通知

    public void SetData(KitItem[] kits)
    {
        // クリア
        for (int i = content.childCount - 1; i >= 0; i--) Destroy(content.GetChild(i).gameObject);
        currentSelectedButton = null;
        originalColors.Clear();

        if (kits == null || kits.Length == 0) return;

        foreach (var kit in kits)
        {
            if (!kit.available) continue;

            var go   = Instantiate(gridItemPrefab, content);
            var item = go.GetComponent<ItemView>();
            if (!item) item = go.AddComponent<ItemView>(); // 念のため

            // ItemView に任せる
            item.Setup(kit);
            var button = go.GetComponent<Button>();
            if (button && !originalColors.ContainsKey(button))
                originalColors.Add(button, button.image.color);

            item.OnClick = _ => OnItemPressed(button, kit);
        }
    }

    private void OnItemPressed(Button button, KitItem kit)
    {
        if (currentSelectedButton && originalColors.TryGetValue(currentSelectedButton, out var normal))
            currentSelectedButton.image.color = normal;

        var hi = selectedColor; // ← コピーしてから
        hi.a = 1f;
        button.image.color = hi;

        currentSelectedButton = button;
        OnItemClicked?.Invoke(kit);
    }
}

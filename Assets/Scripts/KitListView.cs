using UnityEngine;

public class KitListView : MonoBehaviour
{
    [SerializeField] private GridPopulator grid;
    [SerializeField] private GameObject loading;
    [SerializeField] private GameObject errorPanel;
    [SerializeField] private GameObject empty;

    public System.Action<KitItem> OnItemClicked
    {
        set => grid.OnItemClicked = value;
    }

    public void ShowLoading(bool v) { if (loading)    loading.SetActive(v); }
    public void ShowError(bool v)   { if (errorPanel) errorPanel.SetActive(v); }
    public void ShowEmpty(bool v)   { if (empty)      empty.SetActive(v); }

    public void BindItems(KitItem[] items) { grid.SetData(items); }
}

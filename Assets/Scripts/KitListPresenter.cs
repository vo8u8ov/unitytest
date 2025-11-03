using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public sealed class KitListPresenter
{
    private readonly KitListView view;
    private readonly IKitRepository repo;
    private readonly string url;

    private List<KitItem> kits = new List<KitItem>();
    public int KitCount => kits.Count;
    
    public KitListPresenter(KitListView view, IKitRepository repo, string url)
    {
        this.view = view; this.repo = repo; this.url = url;
        this.view.OnItemClicked = OnItemClicked;
    }

    public async Task InitAsync(CancellationToken ct = default)
    {
        view.ShowError(false);
        view.ShowEmpty(false);
        view.ShowLoading(true);
        try
        {
            var loaded = await repo.FetchAsync(url, ct);
            view.ShowLoading(false);
            if (loaded == null || loaded.Length == 0) { view.ShowEmpty(true); return; }

            kits = new List<KitItem>(loaded);
            kits.Sort((a, b) => a.order.CompareTo(b.order));
            view.BindItems(loaded);
        }
        catch
        {
            view.ShowLoading(false);
            view.ShowError(true);
        }
    }

    public KitItem GetKit(int index)
    {
        if (kits == null || kits.Count == 0) return null;
        return kits[index];
    }

    private void OnItemClicked(KitItem kit)
    {
        // 必要ならここで他UIへ反映/サーバーPOSTなど
        UnityEngine.Debug.Log($"[Presenter] Click: {kit.id}");
    }

}

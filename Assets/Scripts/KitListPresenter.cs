using System.Threading;
using System.Threading.Tasks;

public sealed class KitListPresenter
{
    private readonly KitListView view;
    private readonly IKitRepository repo;
    private readonly string url;

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
            KitItem[] kits = await repo.FetchAsync(url, ct);
            view.ShowLoading(false);
            if (kits == null || kits.Length == 0) { view.ShowEmpty(true); return; }
            view.BindItems(kits);
        }
        catch
        {
            view.ShowLoading(false);
            view.ShowError(true);
        }
    }

    private void OnItemClicked(KitItem kit)
    {
        // 必要ならここで他UIへ反映/サーバーPOSTなど
        UnityEngine.Debug.Log($"[Presenter] Click: {kit.id}");
    }
}

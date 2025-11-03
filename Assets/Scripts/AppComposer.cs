using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppComposer : MonoBehaviour
{
    [SerializeField] private string jsonUrl = "";
    [SerializeField] private KitListView view;
    private KitListPresenter presenter;

    async void Start()
    {
        var repo = new HttpKitRepository();               // モデル層（データ取得）
        string urlWithQuery = $"{jsonUrl}?v={System.DateTime.UtcNow:yyyyMMddHHmmss}";
        presenter = new KitListPresenter(view, repo, urlWithQuery);  // プレゼンター生成
        await presenter.InitAsync();                      // 初期化・画面構築開始
    }
}

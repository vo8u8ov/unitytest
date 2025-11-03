using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppComposer : MonoBehaviour
{
    [SerializeField] private string jsonUrl = "";
    [SerializeField] private KitListView view;
    [SerializeField] private MenuPanelView menuView;
    [SerializeField] private MenuBuilder menuBuilder;
    private KitListPresenter presenter;
    private StateMachine fsm;
    private MenuScreen menuScreen;

    async void Start()
    {
        var repo = new HttpKitRepository();               // モデル層（データ取得）
        string urlWithQuery = $"{jsonUrl}?v={System.DateTime.UtcNow:yyyyMMddHHmmss}";
        presenter = new KitListPresenter(view, repo, urlWithQuery);  // プレゼンター生成
        await presenter.InitAsync();                      // 初期化・画面構築開始

        // FSM初期化
        fsm = new StateMachine();
        menuScreen = new MenuScreen(fsm, menuView, presenter);
        fsm.Change(menuScreen); // 初期画面はMenu

        // メニューのボタンイベント
        menuBuilder.OnCommand = id =>
        {
            switch (id)
            {
                case "Option":
                    (fsm.Current as MenuScreen)?.GoOption();
                    break;
                case "Help":
                    (fsm.Current as MenuScreen)?.GoHelp();
                    (fsm.Current as OptionScreen)?.Help();
                    break;
                case "Back":
                    (fsm.Current as OptionScreen)?.Back();
                    (fsm.Current as HelpScreen)?.Back();
                    break;
            }
        };
    }
}

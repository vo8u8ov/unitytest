using UnityEngine;

public sealed class MenuScreen : IState
{
    private readonly StateMachine fsm;
    private readonly MenuPanelView view;
    private readonly KitListPresenter presenter;
    private int currentIndex = 0;

    public MenuScreen(StateMachine fsm, MenuPanelView view, KitListPresenter presenter)
    {
        this.fsm = fsm;
        this.view = view;
        this.presenter = presenter;

        // ボタンクリック時のイベント登録
        view.nextButton.onClick.AddListener(OnNextClicked);
    }

    public void Enter()
    {
        view.ShowMenu();
        UpdateLabels();
    }
    public void Exit()
    {
        view.nextButton.onClick.RemoveListener(OnNextClicked);
    }
    public void Tick() { }

    public void GoOption() => fsm.Change(new OptionScreen(fsm, view, presenter));
    public void GoHelp() => fsm.Change(new HelpScreen(fsm, view, presenter));

    private void OnNextClicked()
    {
        if (presenter.KitCount == 0) return;

        currentIndex = (currentIndex + 1) % presenter.KitCount;
        UpdateLabels();
    }

    private void UpdateLabels()
    {
        if (presenter.KitCount == 0)
        {
            view.currentLabel.text = "No Data";
            view.nextLabel.text = "-";
            return;
        }

        // 現在と次を取得
        var currentKit = presenter.GetKit(currentIndex);
        var nextIndex = (currentIndex + 1) % presenter.KitCount;
        var nextKit = presenter.GetKit(nextIndex);

        // ボタンに現在の試薬名を表示
        view.currentLabel.text = currentKit.display;

        // ラベルに「次の試薬名」を表示
        view.nextLabel.text = $"Next: {nextKit.display}";
    }
}

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

    public void Enter() { view.ShowMenu(); }
    public void Exit() { }
    public void Tick() { }

    public void GoOption() => fsm.Change(new OptionScreen(fsm, view, presenter));
    public void GoHelp() => fsm.Change(new HelpScreen(fsm, view, presenter));

    private void OnNextClicked()
    {
        if (presenter.KitCount == 0) return;

        currentIndex = (currentIndex + 1) % presenter.KitCount;
        UpdateLabel();
    }

    private void UpdateLabel()
    {
        var kit = presenter.GetKit(currentIndex);
        if (kit == null)
        {
            view.label.text = "No data";
            return;
        }

        view.label.text = $"{kit.display}";
    }
}

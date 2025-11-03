public sealed class HelpScreen : IState
{
    private readonly StateMachine fsm;
    private readonly MenuPanelView view;
    private readonly KitListPresenter rightPresenter;

    public HelpScreen(StateMachine fsm, MenuPanelView view, KitListPresenter rightPresenter)
    {
        this.fsm = fsm; this.view = view; this.rightPresenter = rightPresenter;
    }

    public void Enter() { view.ShowHelp(); }
    public void Exit() { }
    public void Tick() { }

    public void Back() => fsm.Change(new MenuScreen(fsm, view, rightPresenter));
}

public sealed class OptionScreen : IState
{
    private readonly StateMachine fsm;
    private readonly MenuPanelView view;
    private readonly KitListPresenter rightPresenter;

    public OptionScreen(StateMachine fsm, MenuPanelView view, KitListPresenter rightPresenter)
    {
        this.fsm = fsm; this.view = view; this.rightPresenter = rightPresenter;
    }

    public void Enter() { view.ShowOption(); }
    public void Exit() { }
    public void Tick() { }

    public void Back() => fsm.Change(new MenuScreen(fsm, view, rightPresenter));
    public void Help() => fsm.Change(new HelpScreen(fsm, view, rightPresenter));
}

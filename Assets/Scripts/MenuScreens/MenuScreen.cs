public sealed class MenuScreen : IState
{
    private readonly StateMachine fsm;
    private readonly MenuPanelView view;
    private readonly KitListPresenter rightPresenter;

    public MenuScreen(StateMachine fsm, MenuPanelView view, KitListPresenter rightPresenter)
    {
        this.fsm = fsm; 
        this.view = view; 
        this.rightPresenter = rightPresenter;
    }

    public void Enter() { view.ShowMenu(); }
    public void Exit() { }
    public void Tick() { }

    public void GoOption() => fsm.Change(new OptionScreen(fsm, view, rightPresenter));
    public void GoHelp()   => fsm.Change(new HelpScreen(fsm, view, rightPresenter));
}

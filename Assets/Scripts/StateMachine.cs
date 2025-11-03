public sealed class StateMachine
{
    public IState Current { get; private set; }

    public void Change(IState next)
    {
        if (Current == next) return;
        Current?.Exit();
        Current = next;
        Current?.Enter();
    }

    public void Update() => Current?.Tick();
}
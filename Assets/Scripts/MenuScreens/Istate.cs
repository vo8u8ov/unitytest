public interface IState
{
    void Enter();  // その画面に入った瞬間に実行
    void Exit();   // その画面から出る瞬間に実行
    void Tick();   // 毎フレーム必要なら（不要なら空でOK）
}

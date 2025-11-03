// Assets/Tests/PlayMode/Test_PlayModeSuite.cs
using NUnit.Framework;
using System.Collections;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.TestTools;

public class Test_PlayModeSuite
{
    // ユーティリティ：private SerializeField をリフレクションで差し込む
    static void SetPrivateField(object obj, string fieldName, object value)
    {
        var f = obj.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
        Assert.IsNotNull(f, $"Field '{fieldName}' not found on {obj.GetType().Name}");
        f.SetValue(obj, value);
    }

    // 1) MenuPanelView の表示切替テスト
    // テスト内容: ShowMenu(), ShowOption(), ShowHelp() を順に呼び出して、各パネルの activeSelf 状態を確認
    // 検証していること: 1. 各メソッド呼び出しで対象パネルだけが表示される（他は非表示） 2. UIの切替ロジックが単純で確実に動く
    [UnityTest]
    public IEnumerator MenuPanelView_TogglesPanelsCorrectly()
    {
        // 構築：MenuPanelView と 3つの子パネルを作る
        var root = new GameObject("MenuPanelViewRoot");
        var view = root.AddComponent<MenuPanelView>();

        var menu = new GameObject("MenuPanel");
        var option = new GameObject("OptionPanel");
        var help = new GameObject("HelpPanel");
        menu.transform.SetParent(root.transform, false);
        option.transform.SetParent(root.transform, false);
        help.transform.SetParent(root.transform, false);

        // MenuPanelView の private フィールドへ割当
        SetPrivateField(view, "menuPanel", menu);
        SetPrivateField(view, "optionPanel", option);
        SetPrivateField(view, "helpPanel", help);

        // 初期：Menuを表示
        view.ShowMenu();
        yield return null;
        Assert.IsTrue(menu.activeSelf, "Menu should be active after ShowMenu()");
        Assert.IsFalse(option.activeSelf, "Option should be inactive after ShowMenu()");
        Assert.IsFalse(help.activeSelf, "Help should be inactive after ShowMenu()");

        // Optionへ
        view.ShowOption();
        yield return null;
        Assert.IsFalse(menu.activeSelf);
        Assert.IsTrue(option.activeSelf, "Option should be active after ShowOption()");
        Assert.IsFalse(help.activeSelf);

        // Helpへ
        view.ShowHelp();
        yield return null;
        Assert.IsFalse(menu.activeSelf);
        Assert.IsFalse(option.activeSelf);
        Assert.IsTrue(help.activeSelf, "Help should be active after ShowHelp()");

        Object.DestroyImmediate(root);
    }

    // 2) Repository のFetchが成功するか（実通信）
    // テスト内容: 指定されたURLから非同期通信でJSONを取得できるか
    // 検証していること: 1. 通信が例外を出さずに完了する 2. デシリアライズが正常に行われ、kits が null でも空でもない
    [UnityTest]
    public IEnumerator HttpKitRepository_FetchAsync_Succeeds()
    {
        var repo = new HttpKitRepository();
        var url = "https://vo8u8ov.github.io/unityportfolio/testkits.json";

        var task = repo.FetchAsync(url, CancellationToken.None);

        // Task完了まで待つ
        yield return new WaitUntil(() => task.IsCompleted);

        // 例外が出ていないか
        Assert.IsFalse(task.IsFaulted, task.Exception?.ToString());
        var kits = task.Result;

        Assert.IsNotNull(kits, "kits should not be null");
        Assert.IsTrue(kits.Length > 0, "Expected non-empty kit array");
    }

    // 3) 低頻度の不具合検出用：切替を何度も繰り返す
    // テスト内容: Menu ↔ Option ↔ Help の切替を100回繰り返す
    // 検証していること: 長時間繰り返しても activeSelf の切替が壊れない 
    [UnityTest]
    public IEnumerator MenuPanelView_RepeatToggle_Stable()
    {
        var root = new GameObject("MenuPanelViewRoot");
        var view = root.AddComponent<MenuPanelView>();

        var menu   = new GameObject("MenuPanel");
        var option = new GameObject("OptionPanel");
        var help   = new GameObject("HelpPanel");
        menu.transform.SetParent(root.transform, false);
        option.transform.SetParent(root.transform, false);
        help.transform.SetParent(root.transform, false);

        SetPrivateField(view, "menuPanel",   menu);
        SetPrivateField(view, "optionPanel", option);
        SetPrivateField(view, "helpPanel",   help);

        // 100回切替（必要なら回数を増やせます）
        for (int i = 0; i < 100; i++)
        {
            view.ShowMenu();   yield return null;
            Assert.IsTrue(menu.activeSelf && !option.activeSelf && !help.activeSelf, $"Mismatch at loop {i} (Menu)");

            view.ShowOption(); yield return null;
            Assert.IsTrue(option.activeSelf && !menu.activeSelf && !help.activeSelf, $"Mismatch at loop {i} (Option)");

            view.ShowHelp();   yield return null;
            Assert.IsTrue(help.activeSelf && !menu.activeSelf && !option.activeSelf, $"Mismatch at loop {i} (Help)");
        }

        Object.DestroyImmediate(root);
    }
}

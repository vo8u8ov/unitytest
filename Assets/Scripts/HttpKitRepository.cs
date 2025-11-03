using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public sealed class HttpKitRepository : IKitRepository
{
    public async Task<KitItem[]> FetchAsync(string url, CancellationToken ct = default)
    {
        //using スコープの終わりで自動的にリソースを解放する
        using var req = UnityWebRequest.Get(url);
        var op = req.SendWebRequest();
        while (!op.isDone)
        {
            if (ct.IsCancellationRequested) { req.Abort(); break; }
            await Task.Yield();
        }

#if UNITY_2020_2_OR_NEWER
        if (req.result != UnityWebRequest.Result.Success)
            throw new System.Exception(req.error);
#endif
        string text = req.downloadHandler.text;
        TestKitData data = JsonUtility.FromJson<TestKitData>(text);
        return data?.kits ?? System.Array.Empty<KitItem>();
    }
}

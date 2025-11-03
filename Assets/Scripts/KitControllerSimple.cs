using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class KitControllerSimple : MonoBehaviour
{
    [SerializeField] private string catalogUrl = "";
    [SerializeField] private Text label;

    [Serializable] private class KitItem { public string id; public string display; public int subtext; public bool available; public int order; }
    [Serializable] private class KitCatalog { public string version; public KitItem[] kits; }

    private KitItem[] kits;
    private int index = 0;

    private void Start()
    {
        if (!label) label = GetComponentInChildren<Text>();
        label.text = "Loading...";
        StartCoroutine(LoadKits());
    }

    public void Next()
    {
        if (kits == null || kits.Length == 0) return;
        index = (index + 1) % kits.Length;
        UpdateLabel();
    }

    private IEnumerator LoadKits()
    {
        using (var req = UnityWebRequest.Get(catalogUrl))
        {
            yield return req.SendWebRequest();

            if (req.result == UnityWebRequest.Result.Success)
            {
                var cat = JsonUtility.FromJson<KitCatalog>(req.downloadHandler.text);
                if (cat != null && cat.kits != null)
                {
                    kits = cat.kits
                        .Where(k => k.available)
                        .OrderBy(k => k.order)
                        .ToArray();
                }
            }
        }

        if (kits == null || kits.Length == 0)
        {
            kits = new[]
            {
                new KitItem{ id="none", display="none" },
                new KitItem{ id="fluA", display="fluA" },
                new KitItem{ id="fluB", display="fluB" },
                new KitItem{ id="covid", display="COVID-19" },
            };
        }

        index = 0;
        UpdateLabel();
    }

    private void UpdateLabel()
    {
        label.text = kits[index].display + $" ({kits[index].subtext})";
    }
}

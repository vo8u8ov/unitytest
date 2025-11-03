using System;

[Serializable]
public class TestKitData
{
    public string version;
    public KitItem[] kits;
}

[Serializable]
public class KitItem
{
    public string id;
    public string display;
    public int subtext;  // ← 新しく追加！
    public bool available;
}


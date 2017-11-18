public class TowerInfo
{
    public int ID { get; internal set; }
    public string PrefabName { get; internal set; }
    public string NormalIcon { get; internal set; }
    public string DisabledIcon { get; internal set; }
    public int MaxLevel { get; internal set; }
    public int BasePrice { get; internal set; }
    public float ShotRate { get; internal set; }
    public float GuardRange { get; internal set; }
    public int UseBulletID { get; internal set; }
}
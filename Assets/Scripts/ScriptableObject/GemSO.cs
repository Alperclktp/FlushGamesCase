using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName ="New Gem", menuName ="Gem Settings/New Gem Type")]
public class GemSO : ScriptableObject
{
    [SerializeField] private int id = default;
    public int ID { get { return id; } }

    [SerializeField] private string displayName;
    public string Name { get { return displayName; } }

    [SerializeField] private int salePrice;
    public int SalePrice { get {  return salePrice; } }

    [SerializeField] private Sprite icon;
    public Sprite Icon { get { return icon; } }

    [SerializeField] private GameObject prefab;
    public GameObject Prefab { get { return prefab; } }
}

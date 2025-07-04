using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [SerializeField] private PrefabSet normalItemSet;
    [SerializeField] private PrefabSet specialItemSet;

    public static ItemManager Instance { get; private set; }

    // Public getters, private setters — read-only from outside
    public GameObject[] NormalItemPrefabs => normalItemSet.itemPrefabSet;
    public GameObject[] SpecialItemPrefabs => specialItemSet.itemPrefabSet;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
}
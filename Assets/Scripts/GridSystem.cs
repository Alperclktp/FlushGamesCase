using Sirenix.OdinInspector;
using UnityEngine;

public class GridSystem : MonoBehaviour
{
    [TabGroup("Options")][SerializeField] private int rowCount = 5;
    [TabGroup("Options")][SerializeField] private int columnCount = 5;
    [TabGroup("Options")][SerializeField] private float tileSpacing = 1.5f;

    [TabGroup("References")] [SerializeField] private GameObject tilePrefab;

    private void Awake()
    {
        LoadTilePrefab();
    }

    [Button("Generate Grid")]
    private void GenerateGrid()
    {
        ClearGrid();

        float totalSpacingX = (columnCount - 1) * tileSpacing;
        float totalSpacingZ = (rowCount - 1) * tileSpacing;

        for (int i = 0; i < rowCount; i++)
        {
            for (int j = 0; j < columnCount; j++)
            {
                GameObject tile = Instantiate(tilePrefab, transform);

                float posX = j * tileSpacing - totalSpacingX / 2f;
                float posZ = i * tileSpacing - totalSpacingZ / 2f;

                tile.transform.localPosition = new Vector3(posX, 0f, posZ);
            }
        }
    }

    private void LoadTilePrefab()
    {
        tilePrefab = Resources.Load<GameObject>("Tile/Tile");

        if (tilePrefab == null)
        {
            Debug.LogError("<color=#FF0000><b> Tile prefab not loaded.</b></color>");
        }
        else
        {
            Debug.Log("<color=#FFCC00> <b> Tile prefab loaded. </b> </color>");
        }
    }

    private void ClearGrid()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}

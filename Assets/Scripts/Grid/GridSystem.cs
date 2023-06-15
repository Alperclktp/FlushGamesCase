using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem : MonoBehaviour
{
    [TabGroup("Options")][SerializeField] private int rowCount = 5;
    [TabGroup("Options")][SerializeField] private int columnCount = 5;
    [TabGroup("Options")][SerializeField] private float tileSpacing = 1.5f;

    public List<GameObject> gems = new List<GameObject>();

    private GameObject tilePrefab;

    private void Awake()
    {
        LoadTilePrefab();

        GenerateGrid();
    }

    [Button("Generate Grid")]
    private void GenerateGrid()
    {
        ClearGrid();

        float totalSpacingX = (columnCount - 1) * tileSpacing;
        float totalSpacingZ = (rowCount - 1) * tileSpacing;

        GemSO[] gemSOs = Resources.LoadAll<GemSO>("Gem");

        for (int i = 0; i < rowCount; i++)
        {
            for (int j = 0; j < columnCount; j++)
            {
                int randomIndex = Random.Range(0, gemSOs.Length);
                GemSO selectedGemSO = gemSOs[randomIndex];

                GameObject tile = Instantiate(tilePrefab, transform);

                float posX = j * tileSpacing - totalSpacingX / 2f;
                float posZ = i * tileSpacing - totalSpacingZ / 2f;

                tile.transform.localPosition = new Vector3(posX, 0f, posZ);

                Tile tileComponent = tile.GetComponent<Tile>();

                if (tileComponent != null)
                {
                    tileComponent.gemSO = selectedGemSO;

                    var gem = Instantiate(selectedGemSO.Prefab);
                    gem.transform.position = tileComponent.transform.position + Vector3.up * 0.03f;
                    gem.transform.parent = tileComponent.transform;

                    gems.Add(gem);
                }
            }
        }
    }

    public IEnumerator IEGenerateRandomGemOnTile(Tile tile)
    {
        yield return new WaitForSeconds(tile.gem.spawnInterval);

        GemSO[] gemSOs = Resources.LoadAll<GemSO>("Gem");

        int randomIndex = Random.Range(0, gemSOs.Length);
        GemSO selectedGemSO = gemSOs[randomIndex];

        tile.gemSO = selectedGemSO; 

        var gem = Instantiate(selectedGemSO.Prefab);
        gem.transform.position = tile.transform.position + Vector3.up * 0.03f;
        gem.transform.parent = tile.transform;

        gems.Add(gem);
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

        gems.Clear();
    }
}

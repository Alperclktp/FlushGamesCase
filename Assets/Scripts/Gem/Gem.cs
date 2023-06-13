using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Gem : MonoBehaviour
{
    [TabGroup("Options")][SerializeField] private float duration;
    [TabGroup("Options")] public float collectScale;
    [TabGroup("Options")][SerializeField] private float maxScale;

    [Space(5)]

    [TabGroup("Options")] public float spawnInterval;

    [TabGroup("Options")][SerializeField] private int currentSalePrice;

    [FoldoutGroup("Debug")][SerializeField] private bool isCollectable;

    private GemSO gemSO;

    [HideInInspector] public Tile tile;
    [HideInInspector] public GridSystem gridSystem;

    private void Start()
    {
        tile = GetComponentInParent<Tile>();
        tile.gem = this;

        gemSO = tile.gemSO;
        gridSystem = GetComponentInParent<GridSystem>();

        SetScale(new Vector3(maxScale, maxScale, maxScale));

        currentSalePrice = GetComponentInParent<Tile>().gemSO.SalePrice;
    }

    private void Update()
    {
        if (transform.localScale.x >= collectScale)
        {
            isCollectable = true;
        }
    }

    public void SetScale(Vector3 scale)
    {
        StartCoroutine(IEScale(scale));
    }

    private IEnumerator IEScale(Vector3 targetScale)
    {
        Vector3 currentScale = transform.localScale;

        float initialPosY = transform.position.y;
        float targetPosY = initialPosY + (targetScale.y - currentScale.y);

        float elapsedTime = 0.0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            Vector3 newScale = Vector3.Lerp(currentScale, targetScale, t);
            float newYPos = Mathf.Lerp(initialPosY, targetPosY, t);

            transform.localScale = newScale;
            transform.position = new Vector3(transform.position.x, newYPos, transform.position.z);

            float baseSalePrice = gemSO.SalePrice;
            float scaleUnit = baseSalePrice / maxScale;
            currentSalePrice = Mathf.RoundToInt(baseSalePrice + newScale.y * scaleUnit * maxScale);

            //Debug.Log("Sale Price Value: " + currentSalePrice);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale;
        transform.position = new Vector3(transform.position.x, targetPosY, transform.position.z);

    }
}

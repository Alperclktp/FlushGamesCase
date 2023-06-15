using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class SaleArea : MonoBehaviour
{
    public Transform salePos;

    private void OnTriggerStay(Collider other)
    {
        var player = other.GetComponent<PlayerInteraction>();

        if (player != null)
        {
            if (player.saleGemCoroutine == null)
            {
                player.saleGemCoroutine = StartCoroutine(IESaleGem(other));
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        var player = other.GetComponent<PlayerInteraction>();

        if (player != null)
        {
            if (player.saleGemCoroutine != null)
            {
                StopCoroutine(player.saleGemCoroutine);
                player.saleGemCoroutine = null;
            }
        }
    }

    private IEnumerator IESaleGem(Collider other)
    {
        var player = other.GetComponent<PlayerInteraction>();

        yield return new WaitForSeconds(0.1f);

        for (int i = player.collections.Count - 1; i >= 0; i--)
        {
            Vector3 targetPosition = salePos.position + Vector3.up * ((player.collections.Count - 1 - i) * player.perYOffset);
            GameObject gem = player.collections[i];

            StartCoroutine(IESaleLastGem(other, gem, targetPosition));

            yield return new WaitForSeconds(0.2f);
        }
    }

    private IEnumerator IESaleLastGem(Collider other, GameObject gem, Vector3 target)
    {
        var player = other.GetComponent<PlayerInteraction>();

        gem.transform.parent = null;

        Vector3 startPosition = gem.transform.position;

        float duration = 0.3f;

        float startTime = Time.time;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            if (gem != null)
                gem.transform.position = Vector3.Lerp(startPosition, target, t);
            elapsedTime = Time.time - startTime;
            yield return null;
        }

        if (gem != null && player.collections.Contains(gem))
        {
            player.collections.Remove(gem);

            Gem gemComponent = gem.GetComponent<Gem>();
            if (gemComponent != null)
            {
                if (player.gemTypeCollections.ContainsKey(gemComponent.gemSO))
                {
                    player.gemTypeCollections[gemComponent.gemSO].Remove(gem);

                    UIManager.Instance.UpdateCollectedCountText();
                }
            }

            UIManager.Instance.currentMoney += gem.GetComponent<Gem>().currentSalePrice;
            UIManager.Instance.Save();
            Destroy(gem);
        }
    }
}

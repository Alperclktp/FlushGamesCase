using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public List<GameObject> collections = new List<GameObject>();

    [SerializeField] private Transform collectHolder;

    [SerializeField] private float perYOffset;

    [SerializeField] private int maxStackCount;

    private GridSystem gridSystem;

    public static PlayerInteraction Instance;

    private void Start()
    {
        Instance = this;

        gridSystem = FindObjectOfType<GridSystem>();
    }

    private void OnTriggerStay(Collider other)
    {
        var gem = other.GetComponent<Gem>();

        if (gem != null)
        {
            if (other.transform.localScale.x >= gem.collectScale)
            {
                CollectGem(other.transform);

                gem.StopAllCoroutines();

                StartCoroutine(gridSystem.IEGenerateRandomGemOnTile(gem.tile));
            }
        }
    }

    private void CollectGem(Transform t)
    {
        if (!collections.Contains(t.gameObject))
        {
            if (collections.Count < maxStackCount)
            {
                float targetY = (collections.Count - 1) * perYOffset;

                t.parent = collectHolder;

                collections.Add(t.gameObject);

                t.DOLocalJump(new Vector3(0, targetY, 0), 0.2f, 1, 0.5f).OnComplete(() =>
                {                           
                    t.transform.localPosition = new Vector3(0, targetY, 0);
                
                    t.localRotation = Quaternion.Euler(0, 0, 0);
                
                    t.GetComponent<Collider>().enabled = false;        
                });
            }
        }      
    }

    //private void FollowPrevious()
    //{
    //    for (int i = 0; i < collections.Count; i++)
    //    {
    //        if (i == 0)
    //        {
    //            collections[i].transform.position = firstFollowPaper.transform.position;
    //        }
    //        else
    //        {
    //            collections[i].transform.localPosition = Vector3.Lerp(collections[i].transform.localPosition, new Vector3(collections[i - 1].transform.localPosition.x, collections[i].transform.localPosition.y, collections[i - 1].transform.localPosition.z), followPower * Time.fixedDeltaTime);
    //        }
    //    }
    //}
}  

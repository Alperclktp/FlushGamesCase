using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GemCollectionDictionary : Dictionary<GemSO, List<GameObject>> { }

[System.Serializable]
public class GemTypeCounts : Dictionary<GemSO, int> { }

public class PlayerInteraction : MonoBehaviour
{
    public static PlayerInteraction Instance;

    public List<GameObject> collections = new List<GameObject>();

    public GemCollectionDictionary gemTypeCollections = new GemCollectionDictionary();

    public GemTypeCounts gemTypeCounts = new GemTypeCounts();

    [TabGroup("Options")] public float perYOffset;
    [TabGroup("Options")][SerializeField] private int maxStackCount;

    [TabGroup("References")][SerializeField] private Transform collectHolder;

    private GridSystem gridSystem;

    public Coroutine saleGemCoroutine;

    private void Awake()
    {
        DOTween.SetTweensCapacity(1500, 1000);
    }

    private void Start()
    {
        Instance = this;

        gridSystem = FindObjectOfType<GridSystem>();
    }

    private void FixedUpdate()
    {
        //FollowPrevious();
    }

    private void OnTriggerStay(Collider other)
    {
        var gem = other.GetComponent<Gem>();

        if (gem != null)
        {
            if (other.transform.localScale.x >= gem.collectScale)
            {
                if (CollectGem(other.transform))
                {
                    gem.StopAllCoroutines();
                    StartCoroutine(gridSystem.IEGenerateRandomGemOnTile(gem.tile));
                }
            }
        }
    }

    private bool CollectGem(Transform t)
    {
        Gem gem = t.GetComponent<Gem>();

        if (gem != null && !collections.Contains(t.gameObject))
        {
            if (collections.Count < maxStackCount)
            {
                float targetY = (collections.Count - 1) * perYOffset;

                t.parent = collectHolder;

                if (!gemTypeCollections.ContainsKey(gem.gemSO))
                {
                    gemTypeCollections[gem.gemSO] = new List<GameObject>();
                }

                collections.Add(t.gameObject);

                gemTypeCollections[gem.gemSO].Add(t.gameObject);

                t.DOLocalJump(new Vector3(0, targetY, 0), 0.2f, 1, 0.5f).OnComplete(() =>
                {
                    t.transform.localPosition = new Vector3(0, targetY, 0);

                    t.localRotation = Quaternion.Euler(0, 0, 0);

                    t.GetComponent<Collider>().enabled = false;

                    //UIManager.Instance.UpdateCollectedCountText();
                });

                if (gemTypeCounts.ContainsKey(gem.gemSO))
                {
                    gemTypeCounts[gem.gemSO]++;
                }
                else
                {
                    gemTypeCounts[gem.gemSO] = 1;
                }
                return true;
            }
        }

        return false;
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
//
//       collections[i].transform.localPosition = Vector3.Lerp(collections[i].transform.localPosition, new Vector3

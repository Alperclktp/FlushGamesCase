using DG.Tweening;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public static PlayerInteraction Instance;

    public List<GameObject> collections = new List<GameObject>();

    [TabGroup("Options")][SerializeField] private Transform collectHolder;
    [TabGroup("Options")] public float perYOffset;
    [TabGroup("Options")][SerializeField] private int maxStackCount;

    private GridSystem gridSystem;

    public int currentMoney;

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

    private void Update()
    {
        UIManager.Instance.currentMoneyText.text = "$" + currentMoney.ToString();
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

                return true;
            }
        }

        return false;
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
    //            collections[i].transform.localPosition = Vector3.Lerp(collections[i].transform.localPosition, new Vector3(collections[i - 1].transform.localPosition.x, collections[i].transform.localPosition.y, collections[i - 1].transform.localPosition.z), 20 * Time.fixedDeltaTime);
    //        }
    //    }
    //}
}




  
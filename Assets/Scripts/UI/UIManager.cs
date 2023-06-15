using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [TabGroup("Options")] public int currentMoney;

    [TabGroup("References")] public TMP_Text currentMoneyText;

    [TabGroup("References")][SerializeField] private ScrollRect showAllGemsScrollView;

    [TabGroup("References")][SerializeField] private GameObject allGemsButton;
    [TabGroup("References")] public GameObject gemInfo;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateGemTypeCounts();

        Load();
    }

    private void Update()
    {
        currentMoneyText.text = "$" + currentMoney.ToString();
    }

    public void ShowAllGemsPanel()
    {
        showAllGemsScrollView.gameObject.GetComponent<Animator>().Play("ShowScrollViewAnimation");
        //showAllGemsScrollView.gameObject.SetActive(true);
        allGemsButton.SetActive(false);

        showAllGemsScrollView.normalizedPosition = Vector3.up;
    }

    public void CloseAllGemsPanel()
    {
        showAllGemsScrollView.gameObject.GetComponent<Animator>().Play("UnShowScrollViewAnimation");
        //showAllGemsScrollView.gameObject.SetActive(false);
        allGemsButton.SetActive(true);
    }

    public void Save()
    {
        PlayerPrefs.SetInt("Money", currentMoney);

        foreach (var gemType in PlayerInteraction.Instance.gemTypeCounts.Keys)
        {
            int count = PlayerInteraction.Instance.gemTypeCounts[gemType];
            PlayerPrefs.SetInt("GemTypeCount_" + gemType.ID, count);
        }
    }

    public void Load()
    {
        currentMoney = PlayerPrefs.GetInt("Money");

        GemSO[] gemTypes = Resources.LoadAll<GemSO>("Gem");

        foreach (GemSO gemType in gemTypes)
        {
            int count = PlayerPrefs.GetInt("GemTypeCount_" + gemType.ID, 0);
            PlayerInteraction.Instance.gemTypeCounts[gemType] = count;
        }

        UpdateCollectedCountText();
    }

    [Button("Delete Data")]
    public void DeleteData()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("<color=#00FF00> <b> Data Deleted. </b> </color>");
    }

    public void UpdateGemTypeCounts()
    {
        GemSO[] gemTypes = Resources.LoadAll<GemSO>("Gem");

        int gemInfoIndex = showAllGemsScrollView.content.childCount;

        foreach (GemSO gemType in gemTypes)
        {
            int gemCount = 0;

            if (PlayerInteraction.Instance.gemTypeCollections.ContainsKey(gemType))
            {
                gemCount = PlayerInteraction.Instance.gemTypeCollections[gemType].Count;
            }

            GameObject gemInfoPrefab = Instantiate(gemInfo, showAllGemsScrollView.content);
            gemInfoPrefab.name = "GemInfo" + (gemInfoIndex + 1);

            RectTransform gemInfoTransform = gemInfoPrefab.GetComponent<RectTransform>();
            gemInfoTransform.SetParent(showAllGemsScrollView.content, false);

            Image gemIcon = gemInfoPrefab.transform.Find("GemIcon").GetComponent<Image>();
            TMP_Text gemTypeText = gemInfoPrefab.GetComponentInChildren<TMP_Text>();

            gemIcon.sprite = gemType.Icon;
            gemTypeText.text = gemType.Name;

            gemInfoIndex++;
        }
    }

    public void UpdateCollectedCountText()
    {
        foreach (var gemType in PlayerInteraction.Instance.gemTypeCounts.Keys)
        {
            int count = PlayerInteraction.Instance.gemTypeCounts[gemType];

            Transform gemInfoTransform = showAllGemsScrollView.content.Find("GemInfo" + gemType.ID);

            if (gemInfoTransform != null)
            {
                TMP_Text collectedCountText = gemInfoTransform.Find("CollectedCountText").GetComponent<TMP_Text>();
                collectedCountText.text = "Collected Count: " + count.ToString();
            }
        }
    }
}

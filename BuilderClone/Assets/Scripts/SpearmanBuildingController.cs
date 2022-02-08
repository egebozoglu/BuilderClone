using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearmanBuildingController : MonoBehaviour
{
    public static SpearmanBuildingController instance;

    public GameObject goldNeedTextObject, woodNeedTextObject, stoneNeedTextObject;

    public int goldNeedCount, stoneNeedCount, woodNeedCount;

    public bool buildCompleted = false;

    public bool spearmanStarted = false;

    public bool spearmenCompleted = false;

    public GameObject spearmanPrefab;

    public List<GameObject> instantiatedSpearmen = new List<GameObject>();

    public List<GameObject> spearmenStartingPoints = new List<GameObject>();

    public GameObject building;

    public GameObject needArea;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        goldNeedTextObject.transform.GetComponent<TextMesh>().text = goldNeedCount.ToString();
        woodNeedTextObject.transform.GetComponent<TextMesh>().text = woodNeedCount.ToString();
        stoneNeedTextObject.transform.GetComponent<TextMesh>().text = stoneNeedCount.ToString();

        if (goldNeedCount == 0 && woodNeedCount == 0 && stoneNeedCount == 0)
        {
            buildCompleted = true;
            building.SetActive(true);
        }

        if (instantiatedSpearmen.Count == 3)
        {
            spearmenCompleted = true;
        }

        SpearmanAnimation();
        SpearmanCreator();

        if (instantiatedSpearmen.Count == 3)
        {
            needArea.SetActive(false);
        }
    }

    public void InstantiateSpearman()
    {
        GameObject spearman;

        var count = instantiatedSpearmen.Count;

        spearman = Instantiate(spearmanPrefab, spearmenStartingPoints[count].transform.position, Quaternion.Euler(0f, 180f, 0f));

        instantiatedSpearmen.Add(spearman);
    }

    public void SpearmanAnimation()
    {
        if (!GameManager.instance.finished)
        {
            if (instantiatedSpearmen.Count > 1)
            {
                for (int i = 0; i < instantiatedSpearmen.Count - 1; i++)
                {
                    instantiatedSpearmen[i].GetComponent<Animation>().Play("dancing");
                }
            }
        }
    }

    public void SpearmanCreator()
    {
        if (instantiatedSpearmen.Count < 3 && woodNeedCount == 0 && goldNeedCount == 0 && stoneNeedCount == 0)
        {
            goldNeedCount = 5;
            stoneNeedCount = 5;

            if (spearmanStarted)
            {
                InstantiateSpearman();
            }

            if (instantiatedSpearmen.Count == 0)
            {
                spearmanStarted = true;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArcherBuildingController : MonoBehaviour
{
    public static ArcherBuildingController instance;

    public GameObject goldNeedTextObject, woodNeedTextObject, stoneNeedTextObject;

    public int goldNeedCount = 5, stoneNeedCount = 5, woodNeedCount = 5;

    public bool buildCompleted = false;

    public bool archersStarted = false;

    public bool archersCompleted = false;

    public GameObject archerPrefab;

    public List<GameObject> instantiatedArchers = new List<GameObject>();

    public List<GameObject> archersStartingPoints = new List<GameObject>();

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

        if (instantiatedArchers.Count==3)
        {
            archersCompleted = true;
        }

        ArchersAnimation();
        ArcherCreator();

        if (instantiatedArchers.Count == 3)
        {
            needArea.SetActive(false);
        }
    }

    public void InstantiateArcher()
    {
        GameObject archer;

        var count = instantiatedArchers.Count;

        archer = Instantiate(archerPrefab, archersStartingPoints[count].transform.position, Quaternion.Euler(0f, 180f, 0f));

        instantiatedArchers.Add(archer);
    }

    public void ArchersAnimation()
    {
        if (!GameManager.instance.finished)
        {
            if (instantiatedArchers.Count > 1)
            {
                for (int i = 0; i < instantiatedArchers.Count - 1; i++)
                {
                    instantiatedArchers[i].GetComponent<Animation>().Play("dancing");
                }
            }
        }
    }

    public void ArcherCreator()
    {
        if (instantiatedArchers.Count<3 && woodNeedCount == 0 && goldNeedCount == 0 && stoneNeedCount == 0)
        {
            goldNeedCount = 5;
            stoneNeedCount = 5;

            if (archersStarted)
            {
                InstantiateArcher();
            }

            if (instantiatedArchers.Count==0)
            {
                archersStarted = true;
            }
        }
    }
}

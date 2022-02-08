using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public List<GameObject> collectedStacks = new List<GameObject>();

    public bool finished = false;

    public GameObject goldSoundPrefab, woodAndStoneSoundPrefab, applauseSoundPrefab, throwingStackSoundPrefab;

    bool applauseInstantiated = false;

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
        if (ArcherBuildingController.instance.archersCompleted && SpearmanBuildingController.instance.spearmenCompleted)
        {
            finished = true;

            if (!applauseInstantiated)
            {
                InstantiateApplauseSound(Vector3.zero);
                applauseInstantiated = true;
            }

            foreach (GameObject archer in ArcherBuildingController.instance.instantiatedArchers)
            {
                archer.GetComponent<Animation>().Play("dancing");
            }

            foreach (GameObject spearman in SpearmanBuildingController.instance.instantiatedSpearmen)
            {
                spearman.GetComponent<Animation>().Play("dancing");
            }

            foreach (GameObject stack in collectedStacks)
            {
                Destroy(stack, 0f);
            }

            PlayerController.instance.transform.position = new Vector3(0f, 0f, 0f);
            PlayerController.instance.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            PlayerController.instance.transform.GetComponent<Animation>().Play("dancing");
        }
    }

    public void InstantiateSound(GameObject soundPrefab, Vector3 position)
    {
        GameObject sound;

        sound = Instantiate(soundPrefab, position, Quaternion.identity);

        Destroy(sound, 2f);
    }

    public void InstantiateApplauseSound(Vector3 position)
    {
        GameObject applauseSound;

        applauseSound = Instantiate(applauseSoundPrefab, position, Quaternion.identity);
    }
}

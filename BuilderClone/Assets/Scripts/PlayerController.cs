using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    public DynamicJoystick joystick;

    string animationName = "idle";

    float speed = 0.04f;

    public GameObject stackStartingPoint;

    public GameObject woodStack, goldStack, stoneStack;

    float collectingRate = 0f;
    float collectingTime = 1f;

    float throwingRate = 0f;
    float throwingTime = 0.5f;

    public GameObject archerCollecting, spearmanCollecting, recyclerCollecting;

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
        transform.GetComponent<Animation>().Play(animationName);
    }

    private void FixedUpdate()
    {
        Movement();
    }

    void Movement()
    {
        var horizontal = joystick.Horizontal;
        var vertical = joystick.Vertical;

        if (!GameManager.instance.finished)
        {
            if (horizontal != 0 || vertical != 0)
            {
                var joyAngle = Mathf.Atan2(horizontal, vertical);

                joyAngle = joyAngle * Mathf.Rad2Deg;

                transform.rotation = Quaternion.Euler(0f, joyAngle, 0f);

                transform.Translate(Vector3.forward * speed);
                animationName = "walking";
            }

            else
            {
                animationName = "idle";
            }
        }
        else
        {
            animationName = "dancing";
        }
    }

    void InstantiateStack(GameObject stackObject, GameObject soundPrefab)
    {
        if (GameManager.instance.collectedStacks.Count<15)
        {
            GameObject stack;

            stack = Instantiate(stackObject, Vector3.zero, Quaternion.identity);
            stack.transform.SetParent(stackStartingPoint.transform, false);
            if (GameManager.instance.collectedStacks.Count != 0)
            {
                var count = GameManager.instance.collectedStacks.Count;
                var newPosition = new Vector3(GameManager.instance.collectedStacks[count - 1].transform.position.x,
                    GameManager.instance.collectedStacks[count - 1].transform.position.y + 0.134f, GameManager.instance.collectedStacks[count - 1].transform.position.z);
                stack.transform.position = newPosition;
            }
            GameManager.instance.collectedStacks.Add(stack);
            GameManager.instance.InstantiateSound(soundPrefab, transform.position);
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "rockCollectingCollider")
        {
            collectingRate += Time.deltaTime;
            if (collectingRate>collectingTime)
            {
                collectingRate = 0f;
                InstantiateStack(stoneStack, GameManager.instance.woodAndStoneSoundPrefab);
            }
        }

        if (other.gameObject.tag == "woodCollectingCollider")
        {
            collectingRate += Time.deltaTime;
            if (collectingRate > collectingTime)
            {
                collectingRate = 0f;
                InstantiateStack(woodStack, GameManager.instance.woodAndStoneSoundPrefab);
            }
        }

        if (other.gameObject.tag == "goldCollectingCollider")
        {
            collectingRate += Time.deltaTime;
            if (collectingRate > collectingTime)
            {
                collectingRate = 0f;
                InstantiateStack(goldStack, GameManager.instance.goldSoundPrefab);
            }
        }

        if (other.gameObject.tag == "recyclerCollider")
        {
            if (GameManager.instance.collectedStacks.Count!=0)
            {
                throwingRate += Time.deltaTime;
                if (throwingRate > throwingTime)
                {
                    throwingRate = 0f;
                    var stack = GameManager.instance.collectedStacks[GameManager.instance.collectedStacks.Count - 1];
                    ThrowToBuilding(stack);
                    stack.GetComponent<StackController>().targetPosition = new Vector3(recyclerCollecting.transform.position.x,
                            stack.transform.position.y, recyclerCollecting.transform.position.z);
                    stack.GetComponent<StackController>().beginToMove = true;
                }
            }
        }

        if (other.gameObject.tag == "archerBuildingGivingCollider")
        {
            if (GameManager.instance.collectedStacks.Count != 0)
            {
                throwingRate += Time.deltaTime;
                if (throwingRate > throwingTime)
                {
                    throwingRate = 0f;
                    var stack = GameManager.instance.collectedStacks[GameManager.instance.collectedStacks.Count - 1];
                    if (stack.gameObject.tag == "goldStack" && ArcherBuildingController.instance.goldNeedCount>0)
                    {
                        ThrowToBuilding(stack);
                        ArcherBuildingController.instance.goldNeedCount -= 1;
                        stack.GetComponent<StackController>().targetPosition = new Vector3(archerCollecting.transform.position.x,
                            stack.transform.position.y, archerCollecting.transform.position.z);
                        stack.GetComponent<StackController>().beginToMove = true;
                    }
                    else if (stack.gameObject.tag == "woodStack" && ArcherBuildingController.instance.woodNeedCount > 0)
                    {
                        ThrowToBuilding(stack);
                        ArcherBuildingController.instance.woodNeedCount -= 1;
                        stack.GetComponent<StackController>().targetPosition = new Vector3(archerCollecting.transform.position.x,
                            stack.transform.position.y, archerCollecting.transform.position.z);
                        stack.GetComponent<StackController>().beginToMove = true;
                    }
                    else if (stack.gameObject.tag == "stoneStack" && ArcherBuildingController.instance.stoneNeedCount > 0)
                    {
                        ThrowToBuilding(stack);
                        ArcherBuildingController.instance.stoneNeedCount -= 1;
                        stack.GetComponent<StackController>().targetPosition = new Vector3(archerCollecting.transform.position.x,
                            stack.transform.position.y, archerCollecting.transform.position.z);
                        stack.GetComponent<StackController>().beginToMove = true;
                    }

                    //stack.GetComponent<StackController>().targetPosition = new Vector3(archerCollecting.transform.position.x,
                    //        stack.transform.position.y, archerCollecting.transform.position.z);
                    //stack.GetComponent<StackController>().beginToMove = true;
                }
            }
        }

        if (other.gameObject.tag == "spearmanBuildingGivingCollider")
        {
            if (GameManager.instance.collectedStacks.Count != 0)
            {
                throwingRate += Time.deltaTime;
                if (throwingRate > throwingTime)
                {
                    throwingRate = 0f;
                    var stack = GameManager.instance.collectedStacks[GameManager.instance.collectedStacks.Count - 1];
                    if (stack.gameObject.tag == "goldStack" && SpearmanBuildingController.instance.goldNeedCount > 0)
                    {
                        ThrowToBuilding(stack);
                        SpearmanBuildingController.instance.goldNeedCount -= 1;
                        stack.GetComponent<StackController>().targetPosition = new Vector3(spearmanCollecting.transform.position.x,
                            stack.transform.position.y, spearmanCollecting.transform.position.z);
                        stack.GetComponent<StackController>().beginToMove = true;
                    }
                    else if (stack.gameObject.tag == "woodStack" && SpearmanBuildingController.instance.woodNeedCount > 0)
                    {
                        ThrowToBuilding(stack);
                        SpearmanBuildingController.instance.woodNeedCount -= 1;
                        stack.GetComponent<StackController>().targetPosition = new Vector3(spearmanCollecting.transform.position.x,
                            stack.transform.position.y, spearmanCollecting.transform.position.z);
                        stack.GetComponent<StackController>().beginToMove = true;
                    }
                    else if (stack.gameObject.tag == "stoneStack" && SpearmanBuildingController.instance.stoneNeedCount > 0)
                    {
                        ThrowToBuilding(stack);
                        SpearmanBuildingController.instance.stoneNeedCount -= 1;
                        stack.GetComponent<StackController>().targetPosition = new Vector3(spearmanCollecting.transform.position.x,
                            stack.transform.position.y, spearmanCollecting.transform.position.z);
                        stack.GetComponent<StackController>().beginToMove = true;
                    }

                    //stack.GetComponent<StackController>().targetPosition = new Vector3(spearmanCollecting.transform.position.x,
                    //        stack.transform.position.y, spearmanCollecting.transform.position.z);
                    //stack.GetComponent<StackController>().beginToMove = true;
                }
            }
        }
    }

    void ThrowToBuilding(GameObject stack)
    {
        GameManager.instance.collectedStacks.Remove(stack);
        GameManager.instance.InstantiateSound(GameManager.instance.throwingStackSoundPrefab, transform.position);
    }
}

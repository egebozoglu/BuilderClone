using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackController : MonoBehaviour
{
    public static StackController instance;

    float speed = 3f;

    public bool beginToMove = false;

    public Vector3 targetPosition;

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
        
    }

    private void FixedUpdate()
    {
        if (beginToMove)
        {
            Movement(targetPosition);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "destroyStackPosition")
        {
            Destroy(this.gameObject, 0f);
        }
    }

    public void Movement(Vector3 targetPosition)
    {
        float step = speed * Time.deltaTime;

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
    }
}

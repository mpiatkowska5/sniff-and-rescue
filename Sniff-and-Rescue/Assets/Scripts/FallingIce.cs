using UnityEngine;
using System.Collections;

public class FallingIce : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] float waitTimeFall = 0.01f;
    [SerializeField] float waitTimeRise = 4f;
    bool hasFallen = false;
    bool isDown = false;
    Vector3 startPosition;

    private void Awake()
    {
        rb = GetComponentInChildren<Rigidbody>();
        startPosition = rb.transform.position;
    }

    private void Update()
    {
        if(Physics.Raycast(rb.transform.position, Vector3.down, 0.01f))
        {
            isDown = true;
            Debug.Log("isDown)");
        }
        if (isDown && hasFallen)
        {
            Debug.Log("down");
            //Rise();
            StartCoroutine(StartRising());
        }
        if (rb.transform.position == startPosition)
        {
            hasFallen = false;
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            StartCoroutine(StartFalling());
            hasFallen = true;
            Debug.Log("player detected");
        }
    }

    private void Rise()
    {
        StartCoroutine(StartRising());
        //rb.transform.position = Vector3.MoveTowards(rb.transform.position, startPosition, 5f * Time.deltaTime);
    }

    IEnumerator StartFalling()
    {
        yield return new WaitForSeconds(waitTimeFall);
        rb.AddForce(new Vector3(0,-20,0), ForceMode.Force);
    }

    IEnumerator StartRising()
    {
        yield return new WaitForSeconds(waitTimeRise);
        //while (rb.position != startPosition)
        //{
        //    rb.AddForce(new Vector3(0, 20, 0), ForceMode.Force);
        //}
        rb.position = startPosition;
        //hasFallen = false;
    }
}

    

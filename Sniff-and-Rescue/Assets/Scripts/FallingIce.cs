using UnityEngine;
using System.Collections;

public class FallingIce : MonoBehaviour
{
    //Rigidbody rb;
    [SerializeField] float waitTimeFall = 0.01f;
    [SerializeField] float waitTimeRise = 4f;
    [SerializeField] Transform dropTarget;
    //bool hasFallen = false;
    //bool isDown = false;
    [SerializeField] bool playerIsPresent;
    Vector3 startPosition;

    private void Awake()
    {
        //rb = GetComponentInChildren<Rigidbody>();
        startPosition = transform.position;
    }

    private void Update()
    {
        //if(Physics.Raycast(rb.transform.position, Vector3.down, 0.01f))
        //{
        //    isDown = true;
        //    Debug.Log("isDown)");
        //}
        //if (isDown && hasFallen)
        //{
        //    Debug.Log("down");
        //    //Rise();
        //    StartCoroutine(StartRising());
        //}
        //if (rb.transform.position == startPosition)
        //{
        //    hasFallen = false;
        //}

        if (playerIsPresent)
        {
            transform.position = Vector3.MoveTowards(transform.position, dropTarget.position, 0.5f*Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, startPosition, 0.5f * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            //StartCoroutine(StartFalling());
            //hasFallen = true;
            //Debug.Log("player detected");
            playerIsPresent = true;
            

            if (collider.TryGetComponent<PlayerController>(out PlayerController player))
            {
                player.SetParent(transform);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(StartRising());
            //playerIsPresent = false;
            
            if (other.TryGetComponent<PlayerController>(out PlayerController player))
            {
                player.SetParent(null);
            }
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
        //rb.AddForce(new Vector3(0,-20,0), ForceMode.Force);
    }

    IEnumerator StartRising()
    {
        yield return new WaitForSeconds(waitTimeRise);
        playerIsPresent = false;
        //while (rb.position != startPosition)
        //{
        //    rb.AddForce(new Vector3(0, 20, 0), ForceMode.Force);
        //}
        //rb.position = startPosition;
        //hasFallen = false;
    }
}

    

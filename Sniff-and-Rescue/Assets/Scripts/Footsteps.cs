using UnityEngine;
using UnityEngine.InputSystem;

public class Footsteps : MonoBehaviour
{
    public AudioSource source;
    public AudioClip[] sounds;
    
    [SerializeField] private float minPitch = 0.9f;
    [SerializeField] private float maxPitch = 1.1f;
    
    PlayerController controller;
    Vector2 movement;

    void Start()
    {
        controller = GetComponent<PlayerController>();
        StartCoroutine(Loop());
    }

    void Update()
    {
        movement = controller.moveInput;
        if (movement == new Vector2(0,0))
        {
            Debug.Log("not moving");
            source.enabled = false;
        }
    }

    void OnMove()
    {
        source.enabled = true;
        
        Debug.Log("moved");

        //source.clip = sounds[Random.Range(0, sounds.Length)];
        //source.Play();
        
    }

        private System.Collections.IEnumerator Loop()
    {
        while (true)
        {
            source.enabled = true;
            float waitTime = source.clip.length + 0.1f;
            yield return new WaitForSeconds(waitTime);
            if (sounds.Length > 0)
            {
                source.clip = sounds[Random.Range(0, sounds.Length)];
                source.pitch = Random.Range(minPitch, maxPitch);
                source.Play();
            }
        }
    }
    
}

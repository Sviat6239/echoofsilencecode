using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSounds : MonoBehaviour
{
    // звук шагов при движении игрока

    private AudioSource source;
    public AudioClip[] clips;
    private Material material;

    private bool ismoving = false;
    private bool isrunning = false;
    private int currentmaterial = 0;
    public float timebetweensounds = 1f;

    private void Start()
    {
        InvokeRepeating("MoveSound", 0, timebetweensounds);
        InvokeRepeating("RunSound", 0, timebetweensounds / 2f);
        source = GetComponent<AudioSource>();
    }

    void Update()
    {
        RaycastHit hit;

        if (Input.GetAxis("Vertical") > 0 || Input.GetAxis("Vertical") < 0 || Input.GetAxis("Horizontal") > 0 || Input.GetAxis("Horizontal") < 0)
        {
            if (Input.GetKey(KeyCode.LeftShift) && Input.GetAxis("Vertical") > 0)
            {
                ismoving = false;
                isrunning = true;
            }
            else
            {
                ismoving = true;
                isrunning = false;
            }
        }
        else
        {
            ismoving = false;
            isrunning = false;
        }
    }

    void MoveSound()
    {
        if (ismoving)
        {
            source.clip = clips[currentmaterial];
            source.Play();
        }
    }

    void RunSound()
    {
        if (isrunning)
        {
            source.clip = clips[currentmaterial];
            source.Play();
        }
    }
}
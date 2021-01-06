using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform firePoint;
    public GameObject bulletObject;
    public new AudioSource audio;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            this.audio.Play();
            Instantiate(bulletObject, firePoint.position, firePoint.rotation);
        }
    }
}

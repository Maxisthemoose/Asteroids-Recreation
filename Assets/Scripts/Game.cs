using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public GameObject asteroid;
    public GameObject ufo;

    public int count;
    public int level = 1;
    public int score = 0;

    public int interval = 5000;
    public int inter = 5000;

    public int ufoInterval = 5000;
    public int ufoInter = 5000;

    public AudioSource LargeExplosion;
    public AudioSource MediumExplosion;
    public AudioSource SmallExplosion;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < this.count + (level - 1); i++)
        {
            GameObject asteroidNew = Instantiate(this.asteroid);

            asteroidNew.transform.localScale = new Vector2(
                asteroidNew.GetComponent<Asteroid>().size, 
                asteroidNew.GetComponent<Asteroid>().size
            );
        }
    }

    void FixedUpdate()
    {
        Asteroid[] allAsteroids = FindObjectsOfType<Asteroid>();
        if (allAsteroids.Length < 1)
        {
            this.level++;

            for (int i = 0; i < this.count + (level - 1); i++)
            {
                GameObject asteroidNew = Instantiate(this.asteroid, transform);

                asteroidNew.transform.localScale = new Vector2(
                    asteroidNew.GetComponent<Asteroid>().size,
                    asteroidNew.GetComponent<Asteroid>().size
                );
            }
        }

        if (this.score > this.ufoInterval)
        {
            this.ufoInterval += this.ufoInter;
            Instantiate(this.ufo);
        }
    }

    public void PlayExplosion(int size)
    {
        if (size == 3)
        {
            LargeExplosion.Play();
        }
        else if (size == 2)
        {
            MediumExplosion.Play();
        }
        else if (size == 1)
        {
            SmallExplosion.Play();
        }
    }
}

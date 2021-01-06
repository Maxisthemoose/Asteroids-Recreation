using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public int size;
    void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        float ranX = Random.Range(-6f, 6f);
        float ranY = Random.Range(-6f, 6f);
        Vector2 newVelocity = new Vector2(ranX, ranY);

        if (this.size == 3)
        {

            Vector2 pos = transform.position;

            bool spawnXspawnY = new List<bool> { true, false }[Random.Range(0, 2)];

            if (spawnXspawnY)
            {
                // Spawning above or below, along the x axis

                int y = new List<int> { -14, 14 }[Random.Range(0, 2)];
                float x = Random.Range(-17f, 18f);

                pos.x = x;
                pos.y = y;
            }
            else
            {
                // Spawning to the left or right, along the y axis
                int x = new List<int> { -19, 19 }[Random.Range(0, 2)];
                float y = Random.Range(-12f, 13f);

                pos.x = x;
                pos.y = y;
            }

            transform.position = pos;
        }


        rb.velocity = newVelocity;
    }

    void FixedUpdate()
    {
        Vector2 pos = transform.position;


        if (pos.y > 13)
        {
            pos.y = -13;
            //pos.x = -pos.x;
        }
        else if (pos.y < -13)
        {
            pos.y = 13;
            //pos.x = -pos.x;
        }

        if (pos.x > 17)
        {
            pos.x = -17;
            //pos.y = -pos.y;
        }
        else if (pos.x < -17)
        {
            pos.x = 17;
            //pos.y = -pos.y;
        }

        transform.position = pos;
    }

    public void SplitAsteroid(Asteroid oldAsteroid)
    {
        int oldSize = oldAsteroid.size;
        oldSize--;
        for (int i = 0; i < 2; i++)
        {
            Asteroid newAsteroid = Instantiate(oldAsteroid);

            if (oldSize < 1)
            {
                Destroy(newAsteroid.gameObject);
                continue;
            }

            newAsteroid.transform.position = oldAsteroid.transform.position;

            newAsteroid.transform.localScale = new Vector2(oldSize, oldSize);

            newAsteroid.size = oldSize;

        }

        Destroy(oldAsteroid.gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UFOFire : MonoBehaviour
{
    public float speed = 20f;
    public float lifetime = 2;

    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = transform.right * this.speed;
        Destroy(gameObject, this.lifetime);
    }

    void FixedUpdate()
    {
        Vector3 pos = transform.position;

        if (pos.y > 13)
        {
            pos.y = -13;
        }
        else if (pos.y < -13)
        {
            pos.y = 13;
        }

        if (pos.x > 17)
        {
            pos.x = -17;
        }
        else if (pos.x < -17)
        {
            pos.x = 17;
        }

        transform.position = pos;
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        StartCoroutine(this.ShipHit(hitInfo));
    }

    IEnumerator ShipHit(Collider2D hitInfo)
    {
        Control ship = hitInfo.GetComponent<Control>();

        Canvas canvas = FindObjectOfType<Canvas>();
        Transform child = canvas.transform.GetChild(1);
        Text textComponent = child.gameObject.GetComponent<Text>();

        Animator animator = hitInfo.GetComponent<Animator>();

        if (ship != null)
        {
            animator.SetBool("Dying", true);
            Time.timeScale = 0;

            yield return new WaitForSecondsRealtime(animator.GetCurrentAnimatorStateInfo(0).length - 0.2f);

            ship.lives--;
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);

            textComponent.text = "Lives: " + ship.lives;

            Vector2 pos = gameObject.transform.position;

            pos.x = 0;
            pos.y = 0;
            gameObject.transform.position = pos;

            Asteroid[] asteroids = FindObjectsOfType<Asteroid>();

            foreach (Asteroid asteroid in asteroids)
            {
                List<int> exclude = new List<int>() { -4, -3, -2, -1, 0, 1, 2, 3, 4 };
                if (exclude.Contains((int)asteroid.transform.position.x) && exclude.Contains((int)asteroid.transform.position.y))
                {
                    int x;
                    do
                    {
                        x = Random.Range(-17, 17);
                    }
                    while (exclude.Contains(x));

                    int y;
                    do
                    {
                        y = Random.Range(-12, 12);
                    }
                    while (exclude.Contains(y));

                    Vector2 position = asteroid.transform.position;
                    position.x = x;
                    position.y = y;

                    asteroid.transform.position = position;
                }
            }

            if (ship.lives == 0)
            {
                Destroy(gameObject);

                foreach (Asteroid asteroid in asteroids)
                {
                    Destroy(asteroid);
                }
            }

            Time.timeScale = 1;
            animator.SetBool("Dying", false);
        }
    }
}

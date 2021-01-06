using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bullet : MonoBehaviour
{
    public float speed = 15f;
    public Rigidbody2D rb;
    public float lifetime = 2;

    void Start()
    {
        rb.velocity = transform.right * speed;
        Destroy(gameObject, lifetime);
    }

    void Update()
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

        Asteroid asteroid = hitInfo.GetComponent<Asteroid>();
        UFOController ufo = hitInfo.GetComponent<UFOController>();

        if (asteroid != null)
        {

            FindObjectOfType<Game>().PlayExplosion(asteroid.size);

            int asteroidSizePoints = asteroid.size == 3 ? 20 : asteroid.size == 2 ? 50 : 100;

            Canvas canvas = FindObjectOfType<Canvas>();
            Text textComponent = canvas.transform.GetChild(0).gameObject.GetComponent<Text>();
            Text lives = canvas.transform.GetChild(1).gameObject.GetComponent<Text>();

            if (textComponent != null)
            {
                Game game = FindObjectOfType<Game>();

                int score = game.score + asteroidSizePoints;
                game.score += asteroidSizePoints;

                textComponent.text = "Score: " + score;

                if (game.score > game.interval)
                {
                    game.interval += game.inter;
                    Control ship = FindObjectOfType<Control>();
                    ship.lives++;
                    lives.text = "Lives: " + ship.lives;
                }

            }

            asteroid.SplitAsteroid(asteroid);

            Destroy(gameObject);
        }

        if (ufo != null)
        {
            FindObjectOfType<Game>().PlayExplosion(2);
            Canvas canvas = FindObjectOfType<Canvas>();
            Text textComponent = canvas.transform.GetChild(0).gameObject.GetComponent<Text>();
            Text lives = canvas.transform.GetChild(1).gameObject.GetComponent<Text>();

            if (textComponent != null)
            {
                Game game = FindObjectOfType<Game>();
                game.score += 500;
                int score = game.score;

                textComponent.text = "Score: " + score;

                if (game.score > game.interval)
                {
                    game.interval += game.inter;
                    Control ship = FindObjectOfType<Control>();
                    ship.lives++;
                    lives.text = "Lives: " + ship.lives;
                }

            }

            Destroy(gameObject);
            Destroy(ufo.gameObject);

        }
    }
}

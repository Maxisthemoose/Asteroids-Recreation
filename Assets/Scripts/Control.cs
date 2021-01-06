using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Control : MonoBehaviour
{
    public int turnSpeed;
    public int lives;
    public float thrustSpeed = 25.0f;

    public Camera cam;
    public float velocity;

    public AudioSource thrust;

    private float maxVelocity;
    private float sqrMaxVelocity;

    void Awake()
    {
        SetMaxVelocity(this.velocity);
    }
    void FixedUpdate()
    {
        Rigidbody2D rigidBody = GetComponent<Rigidbody2D>();
        Vector3 pos = transform.position;
        Vector3 velocity = rigidBody.velocity;

        if (velocity.sqrMagnitude > sqrMaxVelocity)
        {
            rigidBody.velocity = maxVelocity * velocity.normalized;
        }

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

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * turnSpeed);

        } 
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.forward * -turnSpeed);
        }
        
        if (Input.GetKey(KeyCode.W))
        {
            if (!thrust.isPlaying) thrust.Play();
            GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.up * thrustSpeed, ForceMode2D.Force);
        }
    }

    void SetMaxVelocity(float maxVelocity)
    {
        this.maxVelocity = velocity;
        this.sqrMaxVelocity = maxVelocity * maxVelocity;
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        StartCoroutine(HitAsteroid(hitInfo));
    }

    IEnumerator HitAsteroid(Collider2D hitInfo)
    {
        Asteroid ast = hitInfo.GetComponent<Asteroid>();

        Canvas canvas = FindObjectOfType<Canvas>();
        Transform child = canvas.transform.GetChild(1);
        Text textComponent = child.gameObject.GetComponent<Text>();

        Animator animator = GetComponent<Animator>();

        if (ast != null)
        {
            animator.SetBool("Dying", true);
            Time.timeScale = 0;

            yield return new WaitForSecondsRealtime(animator.GetCurrentAnimatorStateInfo(0).length - 0.2f);

            this.lives--;
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);

            textComponent.text = "Lives: " + this.lives;

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

            if (this.lives == 0)
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

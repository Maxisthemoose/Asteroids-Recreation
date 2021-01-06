using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UFOController : MonoBehaviour
{
    private Vector3 _startPosition;
    private bool _leftRight;

    public float moveSpeed = 3f;
    public float frequency = 5f;
    public float magnitude = 0.5f;

    public float timer;
    public int shootInterval = 2;

    public Transform shootPoint;
    public GameObject ufoBullet;

    void Start()
    {

        Vector3 pos = transform.position;

        float x = new List<float> { -17f, 17f }[Random.Range(0, 2)];
        float y = Random.Range(-11f, 12f);

        pos.x = x;
        pos.y = y;

        transform.position = pos;

        this._startPosition = transform.position;

        this._leftRight = x < 0;
    }


    void FixedUpdate()
    {

        this.timer += Time.deltaTime;
        if (this.timer >= this.shootInterval)
        {
            // Shoot
            shootPoint.Rotate(new Vector2(transform.rotation.x, transform.rotation.y + Random.Range(0, 360)));
            Instantiate(this.ufoBullet, shootPoint.position, shootPoint.rotation);
            this.timer = 0f;
        }


        if (this._leftRight)
        {
            // Rightwards sinusoidal movement
            this._startPosition += transform.right * Time.deltaTime * this.moveSpeed;
            transform.position = this._startPosition + transform.up * Mathf.Sin(Time.time * this.frequency) * this.magnitude;

            if (transform.position.x > 18)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            // Leftwards sinusoidal movement
            this._startPosition -= transform.right * Time.deltaTime * this.moveSpeed;
            transform.position = this._startPosition + transform.up * Mathf.Sin(Time.time * this.frequency) * this.magnitude;

            if (transform.position.x < -18)
            {
                Destroy(gameObject);
            }
        }

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

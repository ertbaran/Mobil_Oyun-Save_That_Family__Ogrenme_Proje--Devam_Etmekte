using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnemyScript : MonoBehaviour
{
    GameManager gameManager;
    public GameObject explodeEffect;
    Transform shooter;
    GameObject playerSlider;
    public AudioClip enemyHit, enemyDeath, playerDamaged;
    Rigidbody2D rb;
    [SerializeField]
    public int enemyHealth = 100;
    public int enemyMoveSpeed = 50;
    float rotateSpeed = 5;
    Color enemyColor;
    Color counterColor;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        rb = GetComponent<Rigidbody2D>();
        shooter = gameManager.shooter;
        enemyColor = GetComponent<SpriteRenderer>().color;
        counterColor = gameManager.counterText.color;
    }
    private void FixedUpdate()
    {
        Vector2 direction = transform.position - shooter.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90;

        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotateSpeed * Time.deltaTime);

        rb.AddForce(-direction / 60 * enemyMoveSpeed);

        if (gameManager.count == 9)
        {
            gameManager.waveNumber = 1;
        }
        else if (gameManager.count == 19)
        {
            gameManager.waveNumber = 2;
        }
        else if (gameManager.count == 29)
        {
            gameManager.waveNumber = 3;
        }
        else if (gameManager.count == 39)
        {
            gameManager.waveNumber = 4;
        }
        if (gameManager.playerHealth == 0)
        {
            print("Game Over");
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.name == "bullet")
        {
            if (enemyHealth > 20)
            {
                StartCoroutine(EnemyDamaged());
            }
            else
            {
                StartCoroutine(SlowMo());
                EnemyDead();
            }
            Destroy(collision.gameObject);  // Undestroyed Bullet ? Woav
        }
        if (collision.collider.name == "ShooterMain")
        {
            PlayerDamaged();
        }
    }
    void PlayerDamaged()
    {
        gameManager.playerHealth--;
        gameManager.audioSource.PlayOneShot(playerDamaged);
        playerSlider = GameObject.Find("PlayerHealthSlider");
        playerSlider.GetComponent<Slider>().value = gameManager.playerHealth;
        Destroy(gameObject);
    }
    void EnemyDead()
    {
        gameManager.count++;
        gameManager.counterText.text = gameManager.count.ToString();
        Instantiate(explodeEffect, gameObject.transform.position, Quaternion.identity);
        gameManager.audioSource.PlayOneShot(enemyDeath);
    }
    IEnumerator EnemyDamaged()
    {
        enemyHealth -= 20;
        gameManager.audioSource.PlayOneShot(enemyHit);
        gameObject.GetComponent<SpriteRenderer>().color = new Color(100, 0, 0);
        yield return new WaitForSeconds(0.1f);
        gameObject.GetComponent<SpriteRenderer>().color = enemyColor;
    }
    IEnumerator SlowMo()
    {
        gameManager.canShoot = false;
        Time.timeScale = 0.3f;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
        Destroy(gameObject.GetComponent<CircleCollider2D>());
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameManager.counterText.color = Color.red;
        gameManager.counterText.transform.localScale = Vector3.one*1.5f;
        yield return new WaitForSeconds(0.2f);
        gameManager.counterText.color = counterColor;
        gameManager.counterText.transform.localScale = new Vector3(0.7f,0.7f,0);
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
        gameManager.canShoot = true;
        Destroy(gameObject);
    }
}

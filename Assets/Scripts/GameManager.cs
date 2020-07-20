using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
public class GameManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip effectBullet;
    public Text counterText;
    public Transform shooter, bulletSpawn, controlObject, control;
    public GameObject bullet;
    public GameObject[] setDisabled;
    GameObject newBullet;
    public int waveNumber = 0;
    public GameObject gameOverPanel;
    bool isGameOver;

    public float rotateSpeed = 5;
    public int bulletSpeed = 1;
    public bool canShoot = true;
    public int count;
    public int playerHealth = 10;

    private void Start()
    {
        shooter = shooter.GetComponent<Transform>();
        bulletSpawn = bulletSpawn.GetComponent<Transform>();
    }
    void Update()
    {
        Vector3 controlDistance = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0);
        controlObject.transform.position = controlDistance - new Vector3(4, 2, 0);

        Vector2 direction = controlObject.position - shooter.position; // Fire direction(touch)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;

        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        shooter.rotation = Quaternion.Lerp(shooter.rotation, rotation, rotateSpeed * 0.02f);

        if (Input.GetMouseButton(0) && canShoot)
        {
            StartCoroutine(Shoot(direction));
        }
        if (playerHealth <=0 && isGameOver == false)
        {
            StartCoroutine(GameOver());
        }
    }
    IEnumerator Shoot(Vector2 direction)
    {
        canShoot = false;
        newBullet = Instantiate(bullet, bulletSpawn.position, Quaternion.identity);
        newBullet.GetComponent<Rigidbody2D>().AddForce(bulletSpawn.transform.up * bulletSpeed * 0.02f);
        newBullet.name = "bullet";
        audioSource.PlayOneShot(effectBullet);
        Destroy(newBullet, 2);
        yield return new WaitForSeconds(0.1f);
        if (Time.timeScale == 1)
        {
            canShoot = true;
        }
    }
    IEnumerator GameOver()
    {
        isGameOver = true;
        gameOverPanel.GetComponent<CanvasGroup>().DOFade(1, 0.4f).SetEase(Ease.OutSine);
        for (int i = 0; i < setDisabled.Length; i++)
        {
            setDisabled[i].SetActive(false);
            setDisabled[i].GetComponent<Transform>().DOScale(0, 0.3f);
        };

        yield return new WaitForSeconds(1f);
        Time.timeScale = 0;
    }

}

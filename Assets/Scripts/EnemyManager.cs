using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class EnemyManager : MonoBehaviour
{
    GameManager gameManager;
    public Transform[] spawnPoints;
    public GameObject[] enemyPrefab;
    public GameObject enemy;
    GameObject nextWaveText;
    public Text currentWave;
    public bool canSpawn = true;
    int holder = 1 ;
    float spawnTime = 2;
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        nextWaveText = GameObject.Find("WaveText");
        //InvokeRepeating("EnemySpawn", 3, 2);
    }
    void Update()
    {
        if (canSpawn)
        {
            StartCoroutine(EnemySpawn());
        }
    }
    IEnumerator EnemySpawn()
    {
        canSpawn = false;
        yield return new WaitForSeconds(spawnTime);
        canSpawn = true;
        switch (gameManager.waveNumber)
            {
                case 0:
                spawnTime = 2;
                    enemy = Instantiate(enemyPrefab[0], spawnPoints[Random.Range(0, spawnPoints.Length)]);
                    break;
                case 1:
                spawnTime = 1;
                    if (holder == 1)
                    {
                        StartCoroutine(NextWave());
                    }
                    enemy = Instantiate(enemyPrefab[1], spawnPoints[Random.Range(0, spawnPoints.Length)]);

                    break;
                case 2:
                spawnTime = 3;
                    if (holder == 2)
                    {
                        StartCoroutine(NextWave());
                    }
                    enemy = Instantiate(enemyPrefab[2], spawnPoints[Random.Range(0, spawnPoints.Length)]);
                    break;
                case 3:
                spawnTime = 0.6f;
                    if (holder == 3)
                    {
                        StartCoroutine(NextWave());
                    }
                    enemy = Instantiate(enemyPrefab[3], spawnPoints[Random.Range(0, spawnPoints.Length)]);
                    break;
                case 4:
                spawnTime = 2;
                    if (holder == 4)
                    {
                        StartCoroutine(NextWave());
                    }
                    enemy = Instantiate(enemyPrefab[3], spawnPoints[Random.Range(0, spawnPoints.Length)]);
                    break;
                case 5:
                spawnTime = 2;
                    if (holder == 5)
                    {
                        StartCoroutine(NextWave());
                    }
                    enemy = Instantiate(enemyPrefab[3], spawnPoints[Random.Range(0, spawnPoints.Length)]);
                    break;
                default:
                    break;
            }
    }
    IEnumerator NextWave()
    {
        canSpawn = false;
        nextWaveText.GetComponent<Text>().text = "Stage " + (gameManager.waveNumber + 1); // Geçerli Wave + 1
        currentWave.text = "Current Stage: " + (gameManager.waveNumber + 1);
        nextWaveText.transform.DOScale(1.5f, 0.2f);
        yield return new WaitForSeconds(1.8f);
        nextWaveText.transform.DOScale(0, 0.2f);
        canSpawn = true;
        holder++;
    }
}
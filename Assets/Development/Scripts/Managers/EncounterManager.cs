using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterManager : MonoBehaviour
{
    private static EncounterManager instance;
    public static EncounterManager Instance
    {
        get
        {
            if (instance != null)
            {
                return instance;
            }
            else
            {
                instance = FindObjectOfType<EncounterManager>();
                return instance;
            }
        }
    }

    public bool encounterActive = false;
    public GameObject Barrier;
    public GameObject Enemy;
    public int amountOfEnemiesToSpawn;
    public GameObject TriggerZone;
    public List<GameObject> EnemiesRemaining;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(encounterActive)
        {
            if(EnemiesRemaining.Count == 0)
            {
                EndEncounter();
            }
        }
    }

    public void StartEncounter()
    {
        GameManager.Instance.ChangeState(GameState.Battle);
        encounterActive = true;
        Barrier.SetActive(true);
        TriggerZone.SetActive(false);
        SpawnEnemy();
    }

    void EndEncounter()
    {
        GameManager.Instance.ChangeState(GameState.FreeRoam);
        encounterActive = false;
        Barrier.SetActive(false);
        DOTween.Sequence()
            .AppendInterval(5)
            .AppendCallback(() =>
            {
                TriggerZone.SetActive(true);
            });
    }

    void SpawnEnemy()
    {
        for (int i = 0; i < amountOfEnemiesToSpawn; i++)
        {
            EnemiesRemaining.Add(Instantiate(Enemy,TriggerZone.transform.position, Quaternion.identity));
        }
    }

    public void UpdateEnemyCount()
    {
        for (int i = 0; i < EnemiesRemaining.Count; i++)
        {
            if (EnemiesRemaining[i] == null)
            {
                EnemiesRemaining.Remove(EnemiesRemaining[i]);
            }
        }
    }
}

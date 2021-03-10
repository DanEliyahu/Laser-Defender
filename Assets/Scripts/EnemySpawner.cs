using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<WaveConfig> waveConfigs;
    [SerializeField] private int startingWave = 0;
    [SerializeField] private bool looping = false;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        do
        {
            yield return StartCoroutine(SpawnAllWaves());
        } while (looping);
    }

    private IEnumerator SpawnAllWaves()
    {
        var rand = new System.Random();
        var shuffledWaveConfigs = waveConfigs.OrderBy(waveConfig => rand.Next()).ToList();
        for (int waveIndex = startingWave; waveIndex < shuffledWaveConfigs.Count; waveIndex++)
        {
            var currentWave = shuffledWaveConfigs[waveIndex];
            yield return StartCoroutine(SpawnAllEnemiesInWave(currentWave));
            yield return new WaitForSeconds(currentWave.TimeAfterWave);
        }
    }

    private IEnumerator SpawnAllEnemiesInWave(WaveConfig waveConfig)
    {
        var startPos = waveConfig.GetWaypoints()[0].position;
        for (int enemyCount = 0; enemyCount < waveConfig.NumOfEnemies; enemyCount++)
        {
            var newEnemy = Instantiate(waveConfig.EnemyPrefab, startPos, Quaternion.identity);
            newEnemy.GetComponent<EnemyPathing>().SetWaveConfig(waveConfig);
            yield return new WaitForSeconds(waveConfig.TimeBetweenSpawns);
        }
    }
}
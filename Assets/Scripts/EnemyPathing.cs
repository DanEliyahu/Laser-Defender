using System.Collections.Generic;
using UnityEngine;

public class EnemyPathing : MonoBehaviour
{
    [SerializeField] private List<Transform> waypoints;

    private int _waypointIndex = 0;
    private WaveConfig _waveConfig;

    private void Start()
    {
        waypoints = _waveConfig.GetWaypoints();
        transform.position = waypoints[_waypointIndex].position;
        _waypointIndex++;
    }

    private void Update()
    {
        Move();
    }

    public void SetWaveConfig(WaveConfig waveConfig)
    {
        _waveConfig = waveConfig;
    }
    private void Move()
    {
        if (_waypointIndex <= waypoints.Count - 1)
        {
            var targetPosition = waypoints[_waypointIndex].position;
            var movementThisFrame = _waveConfig.MoveSpeed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementThisFrame);
            if (transform.position == targetPosition)
            {
                _waypointIndex++;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
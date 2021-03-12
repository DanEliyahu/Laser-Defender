using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    private Text _healthText;
    private Player _player;
    
    // Start is called before the first frame update
    void Start()
    {
        _healthText = GetComponent<Text>();
        _player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        var health = Mathf.Max(_player.GetHealth(), 0);
        _healthText.text = health.ToString();
    }
}

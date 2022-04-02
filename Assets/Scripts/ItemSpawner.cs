using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public static Dictionary<int, ItemSpawner> spawners = new Dictionary<int, ItemSpawner>();
    public static int nextSpawnerId = 1;

    public int spawnerId;
    public bool hasItem = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasItem && other.CompareTag("Player"))
        {
            Player _player = other.GetComponent<Player>();
                if (_player.AttemptPickupItem())
            {
                ItemPickedUp(_player.id, _player);
            }
        }
    }

    private void Start()
    {
        hasItem = false;
        spawnerId = nextSpawnerId;
        nextSpawnerId++;
        spawners.Add(spawnerId, this);

        StartCoroutine(SpawnItem());
    }

    private IEnumerator SpawnItem()
    {
        yield return new WaitForSeconds(10f);

        hasItem = true;
        ServerSend.ItemSpawned(spawnerId);
    }

    private void ItemPickedUp(int _byPlayer, Player _player)
    {
        hasItem = false;
        ServerSend.ItemPickedUp(spawnerId, _byPlayer);
        StartCoroutine(SpawnItem());

        if (_player.health < 100)
        {
            _player.health += 25;

            if(_player.health == 100)
            {
                _player.health = _player.maxHealth;
            }

            ServerSend.PlayerHealth(_player);
        }
        

    }
}
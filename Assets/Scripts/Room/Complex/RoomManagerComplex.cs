using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManagerComplex : MonoBehaviour
{
    [Header("--Enemies--")]
    public List<GameObject> enemies;
    [Header("--Datas--")]
    public List<EnemyData> datas;
    [Header("--Weights--")]
    public List<int> weights;

    List<EntityFSM> _enemies;
    List<RoomSpawnersComplex> _spawnPositions;
    RoomSpawnersComplex[] ts;

    [Header("--Doors--")]
    public List<Door> doors;

    private RoomDropManager _droper;

    void Start()
    {
        _enemies = new List<EntityFSM>();
        _droper = gameObject.GetComponent<RoomDropManager>();
        _spawnPositions = new List<RoomSpawnersComplex>();
        ts = gameObject.GetComponentsInChildren<RoomSpawnersComplex>();

        foreach (RoomSpawnersComplex t in ts)
        {
            if (t != null && t.gameObject != null)
                _spawnPositions.Add(t);
        }

        for (int i = 0; i < enemies.Count; i++)
        {
            for (int j = 0; j < weights[i]; j++)
            {
                GameObject e = null;
                e = Instantiate(enemies[i]);
                var enemy = e.GetComponentInChildren<EntityFSM>();
                enemy.data = datas[i];
                e.transform.parent = transform;
                //enemy.triggerComplex = GetComponentInChildren<TriggerComplex>();
                enemy.openDoors += Remove;
                _enemies.Add(enemy);

                //if (e != null)
                //{
                //    //var r = Random.Range(0, _spawnPositions.Count);

                //    //if (_spawnPositions[r] != null && _spawnPositions[r].spawnEnemyType == enemy.enemyTypeEnum)
                //    //{
                //    //    e.transform.position = _spawnPositions[r].transform.position;
                //    //    enemy.triggerComplex = _spawnPositions[r].trigger;
                //    //    _spawnPositions.Remove(_spawnPositions[r]);
                //    //}                    
                //}

                //for (int r = 0; r < _enemies.Count; r++)
                //{
                //    _enemies[r].transform.position = _spawnPositions[r].transform.position;
                //    _enemies[r].triggerComplex = _spawnPositions[r].trigger;
                //    _spawnPositions.Remove(_spawnPositions[i]);
                //}

                var count = 0;

                while (enemy.enemyTypeEnum != _spawnPositions[count].spawnEnemyType)
                {
                    count++;
                }

                e.transform.position = _spawnPositions[count].transform.position;
                enemy.triggerComplex = _spawnPositions[count].trigger;
                _spawnPositions.Remove(_spawnPositions[count]);
            }
        }

        //for (int i = 0; i < _enemies.Count; i++)
        //{
        //    if (_spawnPositions[i] != null)
        //    {
        //        int count = _spawnPositions.Count;

                

        //        _enemies[i].transform.position = _spawnPositions[count].transform.position;
        //        _enemies[i].triggerComplex = _spawnPositions[count].trigger;
        //        _spawnPositions.Remove(_spawnPositions[i]);
        //    }
                
        //    //for (int j = 0; j < _spawnPositions.Count; j++)
        //    //{
        //    //    if (_spawnPositions[j] != null && _spawnPositions[j].spawnEnemyType == _enemies[i].enemyTypeEnum)
        //    //    {
                    
        //    //    }
        //    //}
        //}
    }

    void Remove()
    {
        if (_enemies.Count > 0)
        {
            for (int i = 0; i < _enemies.Count; i++)
            {
                if (_enemies[i].GetComponentInChildren<EntityFSM>().health <= 0)
                {
                    _enemies.RemoveAt(i);
                    OpenDoors();
                    break;
                }
            }

        }
    }

    void OpenDoors()
    {
        if (_enemies.Count <= 0)
        {
            for (int i = 0; i < doors.Count; i++)
            {
                doors[i].OpenDoor();
            }
            if (_droper != null)
            {
                _droper.drop();
            }

        }
    }
}

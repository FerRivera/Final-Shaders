using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManagerSimple : MonoBehaviour
{
    //public int roomWeight;
    [Header("--Enemies--")]
    public List<GameObject> enemies;
    [Header("--Datas--")]
    public List<EnemyData> datas;
    [Header("--Weights--")]
    public List<int> weights;

    //public RouletteWheel wheelSelection;

    List<GameObject> _enemies;
    List<RoomSpawners> _spawnPositions;
    RoomSpawners[] ts;

    [Header("--Doors--")]
    public List<Door> doors;

    private RoomDropManager _droper;
    
    //int _enemySelected;

    void Start ()
    {
        _enemies = new List<GameObject>();
        _droper = gameObject.GetComponent<RoomDropManager>();
        _spawnPositions = new List<RoomSpawners>();
        ts = gameObject.GetComponentsInChildren<RoomSpawners>();
        //wheelSelection = new RouletteWheel();

        foreach (RoomSpawners t in ts)
        {
            if (t != null && t.gameObject != null)
                _spawnPositions.Add(t);
        }

        for (int i = 0; i < enemies.Count; i++)
        {
            //wheelSelection.add(i, weights[i]);
            for (int j = 0; j < weights[i]; j++)
            {
                GameObject e = null;
                e = Instantiate(enemies[i]);
                var enemy = e.GetComponentInChildren<EntityFSM>();
                enemy.data = datas[i];
                e.transform.parent = transform;
                enemy.trigger = GetComponentInChildren<Trigger>();
                enemy.openDoors += Remove;
                _enemies.Add(e);

                if (e != null)
                {
                    var r = Random.Range(0, _spawnPositions.Count);

                    if (_spawnPositions[r] != null)
                    {
                        e.transform.position = _spawnPositions[r].transform.position;
                        _spawnPositions.Remove(_spawnPositions[r]);
                    }
                }
            }

        }

        //wheelSelection.countActions();

        //while (roomWeight > 0)
        //{
        //    _enemySelected = wheelSelection.getResult();
        //    GameObject e = null;

        //    e = Instantiate(enemies[_enemySelected]);
        //    e.GetComponentInChildren<EntityFSM>().data = datas[_enemySelected];
        //    e.transform.parent = transform.parent;
        //    e.GetComponentInChildren<EntityFSM>().trigger = GetComponentInChildren<Trigger>();
        //    e.GetComponentInChildren<EntityFSM>().openDoors += Remove;
        //    _enemies.Add(e);
        //    roomWeight -= weights[_enemySelected];

        //    if (e != null)
        //    {
        //        var r = Random.Range(0, _spawnPositions.Count);

        //        if (_spawnPositions[r] != null)
        //        {
        //            e.transform.position = _spawnPositions[r].transform.position;
        //            _spawnPositions.Remove(_spawnPositions[r]);
        //        }
        //    }
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

    //void EnemiesLVL1(GameObject e)
    //{
    //    if (_enemySelected == (int)enemiesEnum.minotaur)
    //    {
    //        e = Instantiate(minotaur);
    //        e.transform.parent = transform.parent;
    //        e.GetComponentInChildren<EntityFSM>().trigger = GetComponentInChildren<Trigger>();
    //        e.GetComponentInChildren<EntityFSM>().openDoors += Remove;
    //        _enemies.Add(e);
    //        roomWeight -= minotaurWeight;
    //    }
    //}
}

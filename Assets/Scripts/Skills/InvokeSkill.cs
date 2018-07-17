using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InvokeSkill : MonoBehaviour
{
    public GameObject invokePrefab;
    [Range(0,8)]public int count;
    public float distance;
    List<Tuple<int, int>> numbers;
	
	void Update ()
    {
	
	}

    public void Init(Vector3 pos)
    {
        numbers = new List<Tuple<int, int>>();
        numbers.Add(Tuple.New(-1, -1));
        numbers.Add(Tuple.New(-1, 0));
        numbers.Add(Tuple.New(-1, 1));
        numbers.Add(Tuple.New(0, -1));
        numbers.Add(Tuple.New(0, 1));
        numbers.Add(Tuple.New(1, -1));
        numbers.Add(Tuple.New(1, 0));
        numbers.Add(Tuple.New(1, 1));

        for (int i = 0; i < count; i++)
        {
            var a = Instantiate(invokePrefab);
            var r = Random.Range(0,numbers.Count);
            var randomTuple = numbers[r];
            numbers.RemoveAt(r);
            var rd = new Vector3(randomTuple.First * distance, pos.y - 0.5f, randomTuple.Second * distance);
            rd = rd.normalized * distance;
            a.transform.position = new Vector3(rd.x + pos.x, pos.y - 0.5f, rd.z + pos.z);
        }
    }
}

    using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RouletteWheel
{
    public Dictionary<int, int> actions = new Dictionary<int, int>();

    private int _totalWeight;
	
	public void add (int action , int value)
    {      
        actions.Add(action, value);
	}
    public void countActions()
    {
        // por cada action en el diccionario de actions le sumo action.value al _totalWeight
        foreach (KeyValuePair<int, int> action in actions)
        {
            _totalWeight += action.Value;
        }
    }
	public void clearDictionary()
    {
        actions.Clear();
    }      	
	public int getResult()
    {
        float random = Random.Range(0,_totalWeight);

        foreach (KeyValuePair<int, int> action in actions)
        {
            random -= action.Value;
            if (random<0)
            {
                return action.Key;
            }          
        }
        return getResult();
    }
}

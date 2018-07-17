using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Utility : MonoBehaviour {



    public GameObject underBarStartPos;
    public int counter;
    private List<GameObject> _lc = new List<GameObject>();
    List<Vector3> _possPositions = new List<Vector3>();


    public int lowest = 0;
    public static Utility instance;

    void Awake () {
        instance = this;
    }

	void Update () {

	}


    public void SetUIPosition(GameObject toPos)
    {

        
        for (int i = 0; i < 7; i++)
        {
            _possPositions.Add(new Vector3(underBarStartPos.transform.position.x + (60 * i) - 70, underBarStartPos.transform.position.y - 50));
        }
        toPos.transform.SetParent(underBarStartPos.transform);

         _lc.Add(toPos);
        if(counter >= 0)
        {
            toPos.transform.position = _possPositions[counter];

        }
        counter++;


    }

    public void RemoveUIPosition(GameObject toPos)
    {



       
        for (int i = 0; i < _lc.Count; i++)
        {
            if (i > 0)
            {
                _lc[i].transform.position = _possPositions[i - 1];
            }
          
        }


        _lc.Remove(toPos);
        Destroy(toPos.gameObject);
        if(counter > 0)
        {
            counter--;

        }



    }

}

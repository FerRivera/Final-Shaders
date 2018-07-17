using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropsSkills : MonoBehaviour
{
    public List<int> ids;
    public List<Transform> positions;
    public PickupItem droppedSkill;
    public float DistanceToPickup;

	// Use this for initialization
	void Start ()
    {
        for (int i = 0; i < positions.Count; i++)
        {
            var temp = Instantiate(droppedSkill);
            temp.isDropSkillRoom = true;
            temp.pickupDist = DistanceToPickup;
            temp.transform.position = positions[i].transform.position;
            temp.myItem = ids[i];
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

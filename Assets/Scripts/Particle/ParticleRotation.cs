using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleRotation : MonoBehaviour {

    public void Rotate(Vector3 dir)
    {
        GetComponent<ParticleSystem>().startRotation = dir.y * Mathf.Deg2Rad;
    }

    public static float CalculateAngle(Vector3 dir)
    {
        return Quaternion.FromToRotation(Vector3.up, dir).eulerAngles.x;

    }
}

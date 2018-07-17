using UnityEngine;
using System.Collections;

public class SwordSkillInstantiator : SkillSelfCast
{
    public GameObject swordPrefab;
    public int count;
    Vector3 _dir;

    protected override void Start()
    {
        base.Start();
        Init(caster.transform);
    }

    public void Init(Transform pos)
    {
        int c = 180/count;
        float rotAngle = 360 / count;
        float rotAngleSum = 0;

        for (int j = 0; j < count; j++)
        {
            GameObject a = Instantiate(swordPrefab);
            var r = c * Mathf.Deg2Rad;
            a.GetComponent<ParticleSystem>().startRotation = r;
            _dir = new Vector3(0,c,0);
            c += 360/count;
            a.transform.parent = pos;
            a.transform.localPosition = new Vector3(1,1,1);
            a.transform.RotateAround(pos.position, Vector3.up,rotAngleSum);
            a.transform.rotation = Quaternion.Euler(_dir);
            rotAngleSum += rotAngle;
            a.transform.parent = null;
        }
    }

}

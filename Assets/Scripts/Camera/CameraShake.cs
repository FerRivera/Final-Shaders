using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Vector3 originPosition;
    private Quaternion originRotation;
    float _shake_decay;
    float _shake_intensity;
    public float shake_decay;
    public float shake_intensity;
    public static CameraShake instance;
    public bool shaking;

    private void Start()
    {
        instance = this;
    }

    void OnGUI()
    {
        //if (GUI.Button(new Rect(20, 40, 80, 20), "Shake"))
        //{
        //    Shake();
        //}
    }

    void Update()
    {
        if (_shake_intensity > 0)
        {
            transform.position = originPosition + Random.insideUnitSphere * _shake_intensity;
            transform.rotation = new Quaternion(
            originRotation.x + Random.Range(-_shake_intensity, _shake_intensity) * .2f,
            originRotation.y + Random.Range(-_shake_intensity, _shake_intensity) * .2f,
            originRotation.z + Random.Range(-_shake_intensity, _shake_intensity) * .2f,
            originRotation.w + Random.Range(-_shake_intensity, _shake_intensity) * .2f);
            _shake_intensity -= _shake_decay;
        }
        else
        {
            shaking = false;
        }
    }

    public void Shake()
    {
        shaking = true;
        originPosition = transform.position;
        originRotation = transform.rotation;
        _shake_intensity = shake_intensity;
        _shake_decay = shake_decay;
    }
}

//public class CameraShakev1 : MonoBehaviour
//{

//    private Vector3 originPosition;

//    private Quaternion originRotation;

//    public float shake_decay;

//    public float shake_intensity;

//    private bool shaking;

//    private Transform _transform;

//    void OnGUI()
//    {

//        if (GUI.Button(new Rect(20, 40, 80, 20), "Shake"))
//        {

//            Shake();

//        }

//    }

//    void OnEnable()
//    {
//        _transform = transform;

//    }

//    void Update()
//    {

//        if (!shaking)

//            return;

//        if (shake_intensity > 0f)
//        {

//            //_transform.localPosition = originPosition + Random.insideUnitSphere * shake_intensity;

//            _transform.localRotation = new Quaternion(

//            originRotation.x + Random.Range(-shake_intensity, shake_intensity) * .2f,

//            originRotation.y + Random.Range(-shake_intensity, shake_intensity) * .2f,

//            originRotation.z + Random.Range(-shake_intensity, shake_intensity) * .2f,

//            originRotation.w + Random.Range(-shake_intensity, shake_intensity) * .2f);

//            shake_intensity -= shake_decay;

//        }
//        else
//        {

//            Debug.Log("stopped shaking");

//            shaking = false;

//            //_transform.localPosition = originPosition;

//            //_transform.localRotation = originRotation;

//        }

//    }

//    void Shake()
//    {

//        if (!shaking)
//        {

//            //originPosition = _transform.localPosition;

//            //originRotation = _transform.localRotation;

//        }

//        shaking = true;

//        shake_intensity = .1f;

//        shake_decay = 0.002f;

//    }

//}

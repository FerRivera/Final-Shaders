using UnityEngine;
using System.Collections;
using System;

public class Door : MonoBehaviour
{
    //public GameObject topLimit;
    //public GameObject botLimit;
    bool _opened;
    bool _closed;
    //public int speed;
    Animation _animation;
    //public event Action OnOpen = delegate { };
    public Collider collider;
    //public AnimationClip openClip;
    //public AnimationClip closeClip;

    void Start ()
    {
        _animation = GetComponent<Animation>();
	}
	
	void Update ()
    {

    }
    public void OpenDoor()
    {
        if (_opened)
            return;

        _opened = true;

        if(collider != null)
            collider.enabled = false;

        _animation.Play("Open");
    }
    public void CloseDoor()
    {
        if (_closed)
            return;

        _closed = true;

        if (collider != null)
            collider.enabled = true;

       _animation.Play("Close");
    }
}

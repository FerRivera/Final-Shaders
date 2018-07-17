using UnityEngine;
using System.Collections;

public class MiniMap : MonoBehaviour {

    public Transform posToFollow;

    Vector3 _savePos;
    Camera _cam;
    Rect _saveRect;

	void Start () {
        _savePos = transform.position;
        _cam = GetComponent<Camera>();
        _saveRect = _cam.rect;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            if(_cam.orthographicSize > 20)
            {
               _cam.orthographicSize -= 10;
                transform.position = new Vector3(posToFollow.transform.position.x,transform.position.y,posToFollow.transform.position.z);
            }

        }
        if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            if (_cam.orthographicSize < 50)
            {
                _cam.orthographicSize += 10;
                if(_cam.orthographicSize > 80)
                {
                    transform.position = _savePos;

                }
            }

        }
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            _cam.rect = new Rect(0.25f,0.25f,0.5f,0.5f);
        }
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            _cam.rect = _saveRect;
        }

       
            
            transform.position = new Vector3(posToFollow.transform.position.x, transform.position.y, posToFollow.transform.position.z);
        

    }
}

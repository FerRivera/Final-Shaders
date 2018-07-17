using UnityEngine;
using UnityEngine.UI;

namespace UnityStandardAssets.Utility
{
	public class SmoothFollow : MonoBehaviour
	{
		// The target we are following
		[SerializeField]
		private Transform target;
		// The distance in the x-z plane to the target
		[SerializeField]
		private float distance = 10.0f;
		// the height we want the camera to be above the target
		[SerializeField]
		private float height = 5.0f;

		[SerializeField]
		private float rotationDamping;
		[SerializeField]
		private float heightDamping;
        public float MIN_X;
        //public float MIN_Y;
        public float MIN_Z;

        public float MAX_X;
        //public float MAX_Y;
        public float MAX_Z;

        public Transform minx;
        //public Transform miny;
        public Transform minz;

        public Transform maxx;
        //public Transform maxy;
        public Transform maxz;

        bool isClamping;

        public LayerMask walls;

        // Use this for initialization
        void Start()
        {
                 
            if (minx != null && minz != null && maxx != null && maxz != null)
            {
                MIN_X = minx.transform.position.x;
                MIN_Z = minz.transform.position.z + distance;

                MAX_X = maxx.transform.position.x;
                MAX_Z = maxz.transform.position.z;
            }                
        }
       


        //private void Update()
        //{
        //    if (!target)
        //        return;

        //    RaycastHit ray;

        //    if (Physics.Raycast(transform.position, target.transform.position, out ray, Mathf.Infinity, walls))
        //    {
        //        if(ray.transform != null)
        //        {
        //            var color = ray.transform.GetComponent<MeshRenderer>().material.color;
        //            ray.collider.transform.GetComponent<MeshRenderer>().material.SetFloat("_Cutoff", 1);
        //            ray.collider.transform.GetComponent<MeshRenderer>().material.color = new Color(color.r, color.g, color.b, 0);
        //        }

        //    }
        //}

        // Update is called once per frame
        void LateUpdate()
        {
            // Early out if we don't have a target
            if (!target)
                return;

            var wantedHeight = target.position.y + height;
            transform.position = target.position;
            transform.position = new Vector3(
             Mathf.Clamp(transform.position.x, MIN_X, MAX_X),
             wantedHeight,
            Mathf.Clamp(transform.position.z, MIN_Z, MAX_Z));
            transform.position -= Vector3.forward * distance;

            // Set the height of the camera
            transform.position = new Vector3(transform.position.x, wantedHeight, transform.position.z);
            //Debug.Log(currentHeight);

            if (minx != null && minz != null && maxx != null && maxz != null)
            {
                if (transform.position.x <= MIN_X || transform.position.x >= MAX_X || transform.position.z <= MIN_Z || transform.position.z >= MAX_Z)
                    isClamping = true;
                else
                    isClamping = false;
            }

            if (CameraShake.instance.shaking)
                return;

            if (!isClamping)
            {
                transform.LookAt(target);
            }

            if (!Finder.Instance.inventoryPanel.gameObject.activeSelf)
            {
                if (Input.GetAxis("Mouse ScrollWheel") < 0)
                {
                    if (distance < 8 && height < 8)
                    {
                        distance++;
                        height++;
                    }
                }
                if (Input.GetAxis("Mouse ScrollWheel") > 0)
                {
                    if (distance > 4 && height > 4)
                    {
                        distance--;
                        height--;
                    }
                }
            }

        }
	}
}
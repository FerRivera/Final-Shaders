using UnityEngine;
using System.Collections;

public class DropDownFix : MonoBehaviour {


        private CanvasGroup _dropdownListGroup;

        private void Update()
        {
            if (_dropdownListGroup == null)
            {
                Transform dropdownTransform = transform.Find("Dropdown List");
                if (dropdownTransform != null)
                {
                    _dropdownListGroup = dropdownTransform.GetComponent<CanvasGroup>();
                }
            }
            else if (_dropdownListGroup.alpha == 0f)
            {
                Destroy(_dropdownListGroup.gameObject);
                _dropdownListGroup = null;
            }
        }
    
}

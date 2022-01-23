using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace PopupUI {

    // attachable component decides where to place the popup
    // and also renders line

    public class FollowObject : MonoBehaviour {

        // boolean flag will decide whether the popup follows 
        public bool doesFollow = true;

        // denotes how far above the object the popup should display
        [SerializeField]
        private Vector3 offset = new Vector3(0, 1);

        // this object will be followed by the pop-up
        [SerializeField]
        private GameObject target = null;

        // reference to the actual pop-up object
        // (is used to enable/disable the popup)
        private GameObject popupObject;

        // stores RectTransform of this object
        private RectTransform rectTransform;

        // stores the LineRenderer component to be used for the UI
        private LineRenderer lineRenderer;

        // callback is called when the object is first initialized
        private void Awake() {
            popupObject = transform.Find("Popup").gameObject;

            if(popupObject == null) {
                throw new System.NullReferenceException("No popup object found in children");
            }

            rectTransform = GetComponent<RectTransform>();
            lineRenderer = transform.parent.Find("Line").GetComponent<LineRenderer>();

            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("UI"), LayerMask.NameToLayer("Ball"));
        }

        // callback is called once every frame
        private void Update() {
            if(target != null && popupObject.activeSelf) {
                if(doesFollow){
                    MovePopup();
                }
                DrawLine();
            }
        }

        // casts a ray from the target to the popup in order to draw a line that
        // ends when it reaches the popup
        private void DrawLine() {
            LayerMask mask = LayerMask.GetMask("UI");
            Vector2 origin = target.transform.position;
            Vector2 direction = transform.position - target.transform.position;
            RaycastHit2D ray = Physics2D.Raycast(origin, direction, 100f, mask);

            if(ray) {
                lineRenderer.SetPositions(new Vector3[]{target.transform.position, ray.point});
            }
        }

        // moves the popup with accordance to the target (while remaining onscreen)
        private void MovePopup() {
            float vertExtent = Camera.main.orthographicSize;
            float horzExtent = vertExtent * Screen.width / Screen.height;

            Debug.Log((rectTransform.sizeDelta.x * rectTransform.localScale.x) / 2);

            vertExtent -= (rectTransform.sizeDelta.y * rectTransform.localScale.y) / 2;
            horzExtent -= (rectTransform.sizeDelta.x * rectTransform.localScale.x) / 2;
            Vector3 newPosition = target.transform.position;

            // render popup below target if target is near top of screen
            if (target.transform.position.y + offset.y > vertExtent) {
                newPosition.y -= offset.y;
            } else {
                newPosition.y += offset.y;
            }

            newPosition.x += offset.x;

            // make sure that popup is within view
            newPosition.x = Mathf.Clamp(newPosition.x, -horzExtent, horzExtent);
            newPosition.y = Mathf.Clamp(newPosition.y, -vertExtent, vertExtent);

            transform.position = newPosition;
        }

        // getter and setter for the target
        public void SetTarget(GameObject target) { this.target = target; }

        public GameObject GetTarget() { return target; }


        // enable/disable pop-ups
        public void SetPopupActive(bool value) { popupObject.SetActive(value); }
    }
}
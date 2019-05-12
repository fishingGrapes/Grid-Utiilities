using NaughtyAttributes;
using System;
using UnityEngine;

namespace IGT
{
    public class CameraMove : MonoBehaviour
    {
        private Transform cameraTransform;
        private Vector3 direction;
        private Camera camera;

        [SerializeField]
        private float speed = 2;

        [SerializeField, MinMaxSlider(5, 50)]
        private Vector2 zoomConstraints = Vector2.one * 15;

        // Start is called before the first frame update
        void Start()
        {
            cameraTransform = this.transform;
            camera = this.GetComponent<Camera>();
        }

        // Update is called once per frame
        void LateUpdate()
        {
            this.Move();
            this.Zoom();
        }

        private void Zoom()
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
                camera.orthographicSize = Mathf.Clamp(--camera.orthographicSize, zoomConstraints.x, zoomConstraints.y);
            else if (Input.GetAxis("Mouse ScrollWheel") < 0)
                camera.orthographicSize = Mathf.Clamp(++camera.orthographicSize, zoomConstraints.x, zoomConstraints.y);
        }

        private void Move()
        {
            direction = Vector3.zero;

            if (Input.GetKey(KeyCode.W))
                direction += Vector3.up;
            else if (Input.GetKey(KeyCode.S))
                direction += Vector3.down;

            if (Input.GetKey(KeyCode.A))
                direction += Vector3.left;
            else if (Input.GetKey(KeyCode.D))
                direction += Vector3.right;


            cameraTransform.position = cameraTransform.position + direction * speed * Time.deltaTime;
        }
    }
}

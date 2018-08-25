using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CameraBehaviour
{
    public class CameraCollision : MonoBehaviour {

        [Header("Camera Controls")]
        public float minDistance = 1.0f;
        public float maxDistance = 4.0f;
        public float smooth = 10.0f;
        Vector3 dollyDir;

        public Vector3 dollyDirAdjusted;
        public float distance;

        public void Awake()
        {
            dollyDir = transform.localPosition.normalized;
            distance = transform.localPosition.magnitude;
        }

        public void Update()
        {
            Vector3 desiredCamPos = transform.parent.TransformPoint(dollyDir * maxDistance);
            RaycastHit hit;
            var layerMask = ~(  (1 << 11)| (1 << 13)  ); // ignore Checker Layer so it wont click collisions that are just checkers
            if(Physics.Linecast(transform.parent.position, desiredCamPos, out hit,layerMask ))
            {
                if(hit.transform.gameObject.layer != LayerMask.NameToLayer("Resources") || hit.transform.gameObject.layer != LayerMask.NameToLayer("Items"))
                {
                    distance = Mathf.Clamp((hit.distance * 0.9f), minDistance, maxDistance);
                }
            }
            else
            {
                distance = maxDistance;
            }

            transform.localPosition = Vector3.Lerp(transform.localPosition, dollyDir * distance, Time.deltaTime * smooth);
        }
    }
}

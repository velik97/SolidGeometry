using Runtime.CameraManagement;
using UnityEngine;

namespace Visual
{
    public class LookAtCamera : MonoBehaviour
    {
        private void LateUpdate()
        {
            if (!CameraOwner.Instance.HasCamera)
            {
                return;
            }

            transform.forward = CameraOwner.Instance.CameraTransform.forward;
        }
    }
}

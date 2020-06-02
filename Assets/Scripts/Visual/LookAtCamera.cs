using Runtime.Access.Camera;
using UnityEngine;

namespace Visual
{
    public class LookAtCamera : MonoBehaviour
    {
        private void LateUpdate()
        {
            if (!CameraAccess.Instance.HasCamera)
            {
                return;
            }

            transform.forward = CameraAccess.Instance.CameraTransform.forward;
        }
    }
}

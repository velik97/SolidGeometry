using DG.Tweening;
using UnityEngine;

namespace Util
{
    public static class TransformExtensions
    {
        public static void Reset(this Transform transform)
        {
            transform.DOKill();
            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;
            transform.localScale = Vector3.one;
        }
    }
}
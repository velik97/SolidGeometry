using UnityEngine;

namespace Util
{
    public static class Vector3Extensions
    {
        private const float EPSILON = 0.025f;
        private const float EPSILON_2 = EPSILON * EPSILON;
        private const float EPSILON_3 = EPSILON * EPSILON * EPSILON;

        private const float INVERSE_EPSILON = 1 / EPSILON;

        public static bool IsZero(this Vector3 vector3)
        {
            return vector3.sqrMagnitude < EPSILON_2;
        }

        public static bool SameWith(this Vector3 a, Vector3 b)
        {
            return IsZero(a - b);
        }

        public static bool ParallelWith(this Vector3 a, Vector3 b)
        {
            return IsZero(Vector3.Cross(a, b));
        }

        public static bool CollinearWith(this Vector3 a, Vector3 b, Vector3 c)
        {
            return Mathf.Abs(Vector3.Dot(Vector3.Cross(a, b), c)) < EPSILON_3;
        }

        public static int GetEpsilonHashCode(this Vector3 vector3)
        {
            return RoundToEpsilon(vector3.x).GetHashCode() ^
                   RoundToEpsilon(vector3.y).GetHashCode() << 2 ^
                   RoundToEpsilon(vector3.z).GetHashCode() >> 2;
        }

        private static float RoundToEpsilon(float value)
        {
            int div = (int) (value * INVERSE_EPSILON);
            return div * EPSILON;
        }

        public static Vector3 ClampComponentWise(this Vector3 vector3, float min, float max)
        {
            if (min >= max)
            {
                return Vector3.one * min;
            }
            
            return new Vector3(
                Mathf.Clamp(vector3.x, min, max),
                Mathf.Clamp(vector3.y, min, max),
                Mathf.Clamp(vector3.z, min, max)
                );
        }

        public static Vector3 ClampMagnitude(this Vector3 vector3, float clamp)
        {
            if (clamp <= 0f)
            {
                return Vector3.zero;
            }

            if (vector3.sqrMagnitude <= clamp * clamp)
            {
                return vector3;
            }

            return vector3 * clamp / vector3.magnitude;
        }
    }
}
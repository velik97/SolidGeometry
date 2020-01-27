using System;
using UnityEngine;

namespace Util
{
    public static class GeometryUtils
    {
        /// <summary>
        /// Checks if line segments p1p2 and q1q2 intersects
        /// </summary>
        public static bool LineSegmentsIntersects(Vector3 p1, Vector3 p2, Vector3 q1, Vector3 q2)
        {
            Vector3 orientation1 = Vector3.Cross(p2 - p1, q1 - p1);
            Vector3 orientation2 = Vector3.Cross(p2 - p1, q2 - p1);
            
            Vector3 orientation3 = Vector3.Cross(q2 - q1, p1 - q1);
            Vector3 orientation4 = Vector3.Cross(q2 - q1, p2 - q1);

            if (Vector3.Dot(orientation1, orientation2) < 0 &&
                Vector3.Dot(orientation3, orientation4) < 0)
            {
                return true;
            }
            
            bool LiesOnSegment(Vector3 p, Vector3 r, Vector3 q)
            {
                return q.x <= Math.Max(p.x, r.x) && q.x >= Math.Min(p.x, r.x) && 
                       q.y <= Math.Max(p.y, r.y) && q.y >= Math.Min(p.y, r.y) &&
                       q.z <= Math.Max(p.z, r.z) && q.z >= Math.Min(p.z, r.z);
            }

            if (orientation1.sqrMagnitude == 0f && LiesOnSegment(p1, p2, q1))
            {
                return true;
            }
            if (orientation2.sqrMagnitude == 0f && LiesOnSegment(p1, p2, q2))
            {
                return true;
            }
            if (orientation3.sqrMagnitude == 0f && LiesOnSegment(q1, q2, p1))
            {
                return true;
            }
            if (orientation4.sqrMagnitude == 0f && LiesOnSegment(q1, q2, p1))
            {
                return true;
            }

            return false;
        }
    }
}
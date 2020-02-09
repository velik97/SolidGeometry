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

            if (orientation1.IsZero() && LiesOnSegment(p1, p2, q1))
            {
                return true;
            }
            if (orientation2.IsZero() && LiesOnSegment(p1, p2, q2))
            {
                return true;
            }
            if (orientation3.IsZero() && LiesOnSegment(q1, q2, p1))
            {
                return true;
            }
            if (orientation4.IsZero() && LiesOnSegment(q1, q2, p1))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Return projection of source on line firstOnLine-secondOnLine
        /// </summary>
        public static Vector3 ProjectionOnLine(Vector3 source, Vector3 firstOnLine, Vector3 secondOnLine)
        {
            if (firstOnLine == secondOnLine)
            {
                return firstOnLine;
            }
            
            Vector3 fromSourceToFirst = source - firstOnLine;
            Vector3 lineVector = (secondOnLine - firstOnLine).normalized;

            Vector3 projection = lineVector * Vector3.Dot(lineVector, fromSourceToFirst);
            
            return firstOnLine + projection;
        }

        /// <summary>
        /// Do lines p1p2 and q1q2 intersects
        /// </summary>
        public static bool LinesIntersect(Vector3 p1, Vector3 p2, Vector3 q1, Vector3 q2)
        {
            Vector3 p1p2 = p2 - p1;
            Vector3 q1q2 = q2 - q1;
            Vector3 p1q1 = q1 - p1;
            
            return !p1p2.ParallelWith(q1q2) &&
                   p1p2.CollinearWith(q1q2, p1q1);
        }

        /// <summary>
        /// Returns point of intersection of lines p1p2 and q1q2
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        public static Vector3 PointOfIntersection(Vector3 p1, Vector3 p2, Vector3 q1, Vector3 q2)
        {
            // Solution from here https://math.stackexchange.com/questions/270767/find-intersection-of-two-3d-lines/271366
            Vector3 e = p2 - p1;
            Vector3 f = q2 - q1;
            Vector3 g = q1 - p1;
            
            Vector3 h = Vector3.Cross(f, g);
            Vector3 k = Vector3.Cross(f, e);

            if (k.IsZero())
            {
                throw new ArgumentException("lines don't intersect");
            }

            float sign = Mathf.Sign(Vector3.Dot(h, k));

            return p1 + sign * (h.magnitude / k.magnitude) * e;
        }
    }
}
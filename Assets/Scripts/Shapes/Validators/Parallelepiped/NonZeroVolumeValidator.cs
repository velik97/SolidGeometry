using UnityEngine;

namespace Shapes.Validators.Parallelepiped
{
    public class NonZeroVolumeValidator : Validator
    {
        private Vector3[] m_Axes;

        public NonZeroVolumeValidator(Vector3[] axes)
        {
            m_Axes = axes;
        }

        public void Update()
        {
            UpdateValidState();
        }

        protected override bool CheckIsValid()
        {
            if (m_Axes == null || m_Axes.Length != 3)
            {
                return false;
            }

            return Vector3.Dot(Vector3.Cross(m_Axes[0], m_Axes[1]), m_Axes[2]) != 0f;
        }

        public override string GetNotValidMessage()
        {
            return "Volume have to be more than zero";
        }
    }
}
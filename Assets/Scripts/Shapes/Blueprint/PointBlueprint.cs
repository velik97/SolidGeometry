using Shapes.Data;
using UnityEngine;

namespace Shapes.Blueprint
{
    [System.Serializable]
    public class PointBlueprint : ShapeBlueprint
    {
        [SerializeField] private Vector3 m_Position;
        [SerializeField] private string m_PointName;
        [SerializeField] private bool m_IsAccessory;

        private const string NAME_CANT_BE_EMPTY = "Point name can't be empty";
        private const string ALREADY_HAVE_POINT = "Already have point {0}";
        
        public PointBlueprint(ShapeDataFactory dataFactory) : base(dataFactory)
        {
        }

        public override bool IsValid(out string notValidMessage)
        {
            if (string.IsNullOrEmpty(m_PointName))
            {
                notValidMessage = NAME_CANT_BE_EMPTY;
                return false;
            }
            if (DataFactory.ContainsPointName(m_PointName))
            {
                notValidMessage = string.Format(ALREADY_HAVE_POINT, m_PointName);
                return false;
            }
            notValidMessage = string.Empty;
            return true;
        }

        public override void Create()
        {
            MyShapeDatas.Add(DataFactory.CreatePointData(m_Position, m_PointName, m_IsAccessory));
        }
    }
}
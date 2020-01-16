using System;
using Shapes.Data;

namespace Shapes.Validators.Point
{
    public class PointNameNotEmptyValidator : IValidator
    {
        private readonly PointData m_PointData;
            
        private bool m_IsNotEmpty;

        private string PointName => m_PointData.PointName;
        
        public event Action ValidStateChanged;
        
        public PointNameNotEmptyValidator(PointData pointData)
        {
            m_PointData = pointData;
            m_PointData.NameUpdated += CheckIfNameIsEmpty;
        }
            
        private void CheckIfNameIsEmpty()
        {
            bool notEmpty = !string.IsNullOrEmpty(PointName);
            if (notEmpty == m_IsNotEmpty)
            {
                return;
            }
            m_IsNotEmpty = notEmpty;
            ValidStateChanged?.Invoke();
        }
        
        public bool IsValid()
        {
            return m_IsNotEmpty;
        }

        public string GetNotValidMessage()
        {
            return "Name should not be empty";
        }
    }
}
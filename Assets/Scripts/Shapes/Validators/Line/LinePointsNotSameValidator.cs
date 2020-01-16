using System;
using Shapes.Data;

namespace Shapes.Validators.Line
{
    public class LinePointsNotSameValidator : IValidator
    {
        private readonly LineData m_LineData; 
            
        private bool m_AreNotSame;
        
        public event Action ValidStateChanged;
        
        public LinePointsNotSameValidator(LineData lineData)
        {
            m_LineData = lineData;
            m_LineData.NameUpdated += CheckIfPointsAreSame;
        }
            
        private void CheckIfPointsAreSame()
        {
            bool notSame = m_LineData.StartPoint != m_LineData.EndPoint;
            if (notSame == m_AreNotSame)
            {
                return;
            }
            m_AreNotSame = notSame;
            ValidStateChanged?.Invoke();
        }
        
        public bool IsValid()
        {
            return m_AreNotSame;
        }

        public string GetNotValidMessage()
        {
            return "Points should be distinct";
        }
    }
}
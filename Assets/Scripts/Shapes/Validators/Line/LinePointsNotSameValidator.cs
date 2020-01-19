using System;
using Shapes.Data;

namespace Shapes.Validators.Line
{
    public class LinePointsNotSameValidator : Validator
    {
        private readonly LineData m_LineData;

        public LinePointsNotSameValidator(LineData lineData)
        {
            m_LineData = lineData;
            m_LineData.NameUpdated += UpdateValidState;
        }
        
        protected override bool CheckIsValid()
        {
            return m_LineData.StartPoint != m_LineData.EndPoint;
        }

        public override string GetNotValidMessage()
        {
            return "Points should be distinct";
        }
    }
}
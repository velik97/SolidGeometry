using System;
using Shapes.Data;

namespace Lesson.Shapes.Validators
{
    public class PointNotEmptyValidator : IValidator
    {
        private readonly Func<PointData> m_GetPointFunc;
            
        private bool m_AreNotEmpty;
        
        public event Action ValidStateChanged;
        
        public PointNotEmptyValidator(Func<PointData> getPointFunc, Action<Action> subscribeAction)
        {
            m_GetPointFunc = getPointFunc;

            subscribeAction(CheckIfPointIsEmpty);
        }
            
        private void CheckIfPointIsEmpty()
        {
            bool notEmpty = m_GetPointFunc() != null;
            if (notEmpty == m_AreNotEmpty)
            {
                return;
            }
            m_AreNotEmpty = notEmpty;
            ValidStateChanged?.Invoke();
        }
        
        public bool IsValid()
        {
            return m_AreNotEmpty;
        }

        public string GetNotValidMessage()
        {
            return "Point should be set";
        }
    }
}
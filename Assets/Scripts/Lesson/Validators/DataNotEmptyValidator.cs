using System;

namespace Lesson.Validators
{
    public class DataNotEmptyValidator<TData> : IValidator where TData : class
    {
        private readonly Func<TData> m_GetPointFunc;
            
        private bool m_AreNotEmpty;
        
        public event Action ValidStateChanged;
        
        public DataNotEmptyValidator(Func<TData> getPointFunc, Action<Action> subscribeAction)
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
            return "Data should be set";
        }
    }
}
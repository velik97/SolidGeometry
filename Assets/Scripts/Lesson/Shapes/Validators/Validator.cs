using System;

namespace Lesson.Shapes.Validators
{
    public abstract class Validator : IValidator
    {
        public event Action ValidStateChanged;
        
        private bool m_IsValid;

        protected void UpdateValidState()
        {
            bool isValid = CheckIsValid();
            if (isValid == m_IsValid)
            {
                return;
            }
            m_IsValid = isValid;
            ValidStateChanged?.Invoke();
        }

        protected abstract bool CheckIsValid();
        
        public abstract string GetNotValidMessage();
        
        public bool IsValid()
        {
            return m_IsValid;
        }

    }
}
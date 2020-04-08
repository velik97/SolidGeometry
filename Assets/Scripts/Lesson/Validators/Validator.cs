using System;

namespace Lesson.Validators
{
    public abstract class Validator : IValidator
    {
        public event Action ValidStateChanged;
        
        private bool m_IsValid;

        public void Update()
        {
            bool isValid = CheckIsValid();
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
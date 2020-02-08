using System;

namespace Shapes.Validators.Uniqueness
{
    public abstract class UniquenessValidator<TValidatable> : IValidator, IUniquenessValidator<TValidatable>
    {
        public event Action UniqueDeterminingPropertyUpdated;

        public event Action ValidStateChanged;

        private bool m_IsUnique;
        
        public bool IsValid()
        {
            return m_IsUnique;
        }
        
        public void SetIsUnique(bool unique)
        {
            if (m_IsUnique == unique)
            {
                return;
            }
            m_IsUnique = unique;
            ValidStateChanged?.Invoke();
        }
        
        protected void OnUniqueDeterminingPropertyUpdated()
        {
            UniqueDeterminingPropertyUpdated?.Invoke();
        }
        
        public abstract string GetNotValidMessage();

        public abstract int GetUniqueHashCode();

        public abstract bool UniqueEquals(TValidatable validatable);

    }
}
using System;

namespace Lesson.Validators.Uniqueness
{
    public interface IUniquenessValidator<in TValidatable>
    {
        event Action UniqueDeterminingPropertyUpdated;
        
        void SetIsUnique(bool unique);

        int GetUniqueHashCode();

        bool UniqueEquals(TValidatable validatable);
    }
}
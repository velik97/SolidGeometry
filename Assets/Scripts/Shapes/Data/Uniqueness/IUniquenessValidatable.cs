using System;

namespace Shapes.Data.Uniqueness
{
    public interface IUniquenessValidatable<in TValidatable>
    {
        event Action UniqueDeterminingPropertyUpdated;
        event Action UniquenessUpdated;

        void SetIsUnique(bool unique);

        int GetUniqueHashCode();

        bool UniqueEquals(TValidatable validatable);
    }
}
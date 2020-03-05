using System;

namespace Lesson.Validators
{
    public interface IValidator
    {
        event Action ValidStateChanged;
        
        bool IsValid();

        string GetNotValidMessage();
    }
}
using System;

namespace Shapes.Validators
{
    public interface IValidator
    {
        event Action ValidStateChanged;
        
        bool IsValid();

        string GetNotValidMessage();
    }
}
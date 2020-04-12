using Lesson.Validators;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.VisualElementsExtensions
{
    public class ValidatorField : Label
    {
        private readonly IValidator m_Validator;
        
        public ValidatorField(IValidator validator)
        {
            m_Validator = validator;
            
            AddToClassList("validator");
            
            validator.ValidStateChanged += UpdateField;
            UpdateField();
        }
        
        private void UpdateField()
        {
            bool isValid = m_Validator.IsValid();
            style.display = isValid ? DisplayStyle.None : DisplayStyle.Flex;
            text = isValid ? string.Empty : m_Validator.GetNotValidMessage();
        }
    }
}
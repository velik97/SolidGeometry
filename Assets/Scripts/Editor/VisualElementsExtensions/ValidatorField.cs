using Lesson.Shapes.Validators;
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
            style.color = new StyleColor(Color.red);
            
            validator.ValidStateChanged += UpdateField;
            UpdateField();
        }
        
        private void UpdateField()
        {
            text = m_Validator.IsValid() ? string.Empty : m_Validator.GetNotValidMessage();
        }
    }
}
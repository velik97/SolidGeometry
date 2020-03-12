using Lesson.Shapes.Datas;

namespace Lesson.Validators.Point
{
    public class PointNameNotEmptyValidator : Validator
    {
        private readonly PointData m_PointData;
        
        public PointNameNotEmptyValidator(PointData pointData)
        {
            m_PointData = pointData;
            m_PointData.NameUpdated += Update;
            Update();
        }

        protected override bool CheckIsValid()
        {
            return !string.IsNullOrEmpty(m_PointData.PointName);
        }

        public override string GetNotValidMessage()
        {
            return "Name should not be empty";
        }
    }
}
namespace Lesson.Shapes.Views
{
    public interface IShapeView
    {
        bool Active { get; set; }
        HighlightType Highlight { get; set; }
    }

    public enum HighlightType
    {
        Subtle,
        Normal,
        SemiHighlighted,
        Highlighted,
        Important
    }
}
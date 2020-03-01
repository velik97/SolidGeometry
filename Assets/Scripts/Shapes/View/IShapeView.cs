namespace Shapes.View
{
    public interface IShapeView
    {
        bool Active { get; set; }
        HighlightType Highlight { get; set; }
        void Release();
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
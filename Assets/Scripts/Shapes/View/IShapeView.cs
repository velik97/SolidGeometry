namespace Shapes.View
{
    public interface IShapeView
    {
        void SetActive(bool value);
        void SetHighlight(HighlightType highlightType);
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
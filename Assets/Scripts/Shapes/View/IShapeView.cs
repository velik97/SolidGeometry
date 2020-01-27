namespace Shapes.View
{
    public interface IShapeView
    {
        void SetActive(bool value);
        void SetHighlight(HighlightType highlightType);
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
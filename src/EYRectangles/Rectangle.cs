namespace EYRectangles;

// An axis-aligned rectangle anchored at its bottom-left corner. Immutable and compared by value.
// A record class rather than a struct, so `default` cannot bypass the validation below.
public sealed record Rectangle
{
    public Rectangle(int leftX, int bottomY, int width, int height)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(width);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(height);

        LeftX = leftX;
        BottomY = bottomY;
        Width = width;
        Height = height;
    }

    public int LeftX { get; }

    public int BottomY { get; }

    public int Width { get; }

    public int Height { get; }

    public int RightX => LeftX + Width;

    public int TopY => BottomY + Height;

    public override string ToString() => $"[({LeftX},{BottomY}) {Width}x{Height}]";
}

namespace EYRectangles.Tests;

public sealed class AdjacencyTests
{
    [Theory]
    [InlineData(0, 0, 4, 4, 4, 0, 4, 4)]     // second sits to the right
    [InlineData(4, 0, 4, 4, 0, 0, 4, 4)]     // second sits to the left
    [InlineData(0, 0, 4, 4, 0, 4, 4, 4)]     // second sits above
    [InlineData(0, 4, 4, 4, 0, 0, 4, 4)]     // second sits below
    [InlineData(-8, -6, 4, 4, -4, -6, 4, 4)] // negative coordinates
    public void GetAdjacency_IsProper_WhenBothSidesAreFullyShared(
        int firstLeftX, int firstBottomY, int firstWidth, int firstHeight,
        int secondLeftX, int secondBottomY, int secondWidth, int secondHeight)
    {
        var first = new Rectangle(firstLeftX, firstBottomY, firstWidth, firstHeight);
        var second = new Rectangle(secondLeftX, secondBottomY, secondWidth, secondHeight);

        Assert.Equal(AdjacencyType.Proper, RectangleAnalyzer.GetAdjacency(first, second));
    }

    [Theory]
    [InlineData(0, 0, 4, 6, 4, 1, 3, 3)] // the second rectangle's side lies inside the first's
    [InlineData(4, 1, 3, 3, 0, 0, 4, 6)] // the same pair, arguments swapped
    [InlineData(0, 0, 6, 4, 1, 4, 3, 3)] // horizontal touch instead of vertical
    public void GetAdjacency_IsSubLine_WhenExactlyOneSideIsFullyShared(
        int firstLeftX, int firstBottomY, int firstWidth, int firstHeight,
        int secondLeftX, int secondBottomY, int secondWidth, int secondHeight)
    {
        var first = new Rectangle(firstLeftX, firstBottomY, firstWidth, firstHeight);
        var second = new Rectangle(secondLeftX, secondBottomY, secondWidth, secondHeight);

        Assert.Equal(AdjacencyType.SubLine, RectangleAnalyzer.GetAdjacency(first, second));
    }

    [Theory]
    [InlineData(0, 0, 4, 4, 4, 2, 4, 4)] // vertical touch, sides offset
    [InlineData(0, 0, 6, 4, 3, 4, 6, 3)] // horizontal touch, sides offset
    [InlineData(0, 0, 4, 4, 4, 3, 4, 4)] // the smallest possible share: one unit of length
    public void GetAdjacency_IsPartial_WhenNeitherSideIsFullyShared(
        int firstLeftX, int firstBottomY, int firstWidth, int firstHeight,
        int secondLeftX, int secondBottomY, int secondWidth, int secondHeight)
    {
        var first = new Rectangle(firstLeftX, firstBottomY, firstWidth, firstHeight);
        var second = new Rectangle(secondLeftX, secondBottomY, secondWidth, secondHeight);

        Assert.Equal(AdjacencyType.Partial, RectangleAnalyzer.GetAdjacency(first, second));
    }

    [Theory]
    [InlineData(0, 0, 4, 4, 6, 0, 4, 4)]   // a gap between the facing sides
    [InlineData(0, 0, 4, 4, 10, 10, 4, 4)] // nowhere near each other
    [InlineData(0, 0, 4, 4, 4, 4, 4, 4)]   // corner contact only, so no shared segment
    [InlineData(0, 0, 4, 4, 4, 6, 4, 4)]   // sides on the same line but not overlapping
    [InlineData(0, 0, 4, 4, 2, 2, 4, 4)]   // overlapping rectangles share no side
    [InlineData(0, 0, 6, 6, 2, 0, 4, 4)]   // a contained rectangle flush with a side is not adjacent
    public void GetAdjacency_IsNone_WhenNoSideSegmentIsShared(
        int firstLeftX, int firstBottomY, int firstWidth, int firstHeight,
        int secondLeftX, int secondBottomY, int secondWidth, int secondHeight)
    {
        var first = new Rectangle(firstLeftX, firstBottomY, firstWidth, firstHeight);
        var second = new Rectangle(secondLeftX, secondBottomY, secondWidth, secondHeight);

        Assert.Equal(AdjacencyType.None, RectangleAnalyzer.GetAdjacency(first, second));
    }

    [Theory]
    [InlineData(0, 0, 4, 4, 4, 0, 4, 4)]
    [InlineData(0, 0, 4, 6, 4, 1, 3, 3)]
    [InlineData(0, 0, 4, 4, 4, 2, 4, 4)]
    [InlineData(0, 0, 4, 4, 4, 4, 4, 4)]
    [InlineData(0, 0, 6, 4, 1, 4, 3, 3)]
    public void GetAdjacency_GivesTheSameAnswerRegardlessOfArgumentOrder(
        int firstLeftX, int firstBottomY, int firstWidth, int firstHeight,
        int secondLeftX, int secondBottomY, int secondWidth, int secondHeight)
    {
        var first = new Rectangle(firstLeftX, firstBottomY, firstWidth, firstHeight);
        var second = new Rectangle(secondLeftX, secondBottomY, secondWidth, secondHeight);

        Assert.Equal(
            RectangleAnalyzer.GetAdjacency(first, second),
            RectangleAnalyzer.GetAdjacency(second, first));
    }
}

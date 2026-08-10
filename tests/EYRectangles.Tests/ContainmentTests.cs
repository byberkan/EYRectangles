namespace EYRectangles.Tests;

public sealed class ContainmentTests
{
    [Fact]
    public void Contains_IsTrue_WhenTheInnerRectangleIsStrictlyInside()
    {
        var outer = new Rectangle(0, 0, 10, 10);
        var inner = new Rectangle(2, 2, 4, 4);

        Assert.True(RectangleAnalyzer.Contains(outer, inner));
    }

    [Theory]
    [InlineData(0, 2, 4, 4)]   // flush with the left side
    [InlineData(6, 2, 4, 4)]   // flush with the right side
    [InlineData(2, 0, 4, 4)]   // flush with the bottom side
    [InlineData(2, 6, 4, 4)]   // flush with the top side
    [InlineData(0, 0, 10, 10)] // flush with every side
    public void Contains_IsTrue_WhenTheInnerRectangleTouchesTheBoundaryFromInside(
        int leftX, int bottomY, int width, int height)
    {
        var outer = new Rectangle(0, 0, 10, 10);
        var inner = new Rectangle(leftX, bottomY, width, height);

        Assert.True(RectangleAnalyzer.Contains(outer, inner));
    }

    [Fact]
    public void Contains_IsTrue_InBothDirections_ForIdenticalRectangles()
    {
        var first = new Rectangle(1, 1, 5, 5);
        var second = new Rectangle(1, 1, 5, 5);

        Assert.True(RectangleAnalyzer.Contains(first, second));
        Assert.True(RectangleAnalyzer.Contains(second, first));
    }

    [Fact]
    public void Contains_IsFalse_WhenTheRectanglesOnlyOverlap()
    {
        var outer = new Rectangle(0, 0, 6, 6);
        var inner = new Rectangle(4, 2, 5, 2);

        Assert.False(RectangleAnalyzer.Contains(outer, inner));
    }

    [Fact]
    public void Contains_IsFalse_WhenTheRectanglesAreSeparated()
    {
        var outer = new Rectangle(0, 0, 10, 8);
        var inner = new Rectangle(14, 3, 4, 2);

        Assert.False(RectangleAnalyzer.Contains(outer, inner));
    }

    [Fact]
    public void Contains_IsFalse_WhenTheInnerRectangleEscapesOnASingleAxis()
    {
        var outer = new Rectangle(0, 0, 10, 10);
        var inner = new Rectangle(2, -1, 4, 4);

        Assert.False(RectangleAnalyzer.Contains(outer, inner));
    }

    [Fact]
    public void Contains_IsDirectional()
    {
        var outer = new Rectangle(0, 0, 10, 10);
        var inner = new Rectangle(2, 2, 4, 4);

        Assert.True(RectangleAnalyzer.Contains(outer, inner));
        Assert.False(RectangleAnalyzer.Contains(inner, outer));
    }

    [Fact]
    public void Contains_WorksWithNegativeCoordinates()
    {
        var outer = new Rectangle(-10, -10, 8, 8);
        var inner = new Rectangle(-8, -8, 2, 2);

        Assert.True(RectangleAnalyzer.Contains(outer, inner));
    }
}

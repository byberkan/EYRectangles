namespace EYRectangles.Tests;

public sealed class RectangleTests
{
    [Fact]
    public void Constructor_KeepsTheGivenPositionAndSize()
    {
        var rectangle = new Rectangle(2, 3, 5, 7);

        Assert.Equal(2, rectangle.LeftX);
        Assert.Equal(3, rectangle.BottomY);
        Assert.Equal(5, rectangle.Width);
        Assert.Equal(7, rectangle.Height);
    }

    [Fact]
    public void Constructor_DerivesTheRightAndTopSides()
    {
        var rectangle = new Rectangle(2, 3, 5, 7);

        Assert.Equal(7, rectangle.RightX);
        Assert.Equal(10, rectangle.TopY);
    }

    [Fact]
    public void Constructor_AcceptsNegativeCoordinates()
    {
        var rectangle = new Rectangle(-10, -20, 4, 6);

        Assert.Equal(-6, rectangle.RightX);
        Assert.Equal(-14, rectangle.TopY);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Constructor_RejectsNonPositiveWidth(int width)
    {
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => new Rectangle(0, 0, width, 5));

        Assert.Equal("width", exception.ParamName);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Constructor_RejectsNonPositiveHeight(int height)
    {
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => new Rectangle(0, 0, 5, height));

        Assert.Equal("height", exception.ParamName);
    }

    [Fact]
    public void Rectangles_WithTheSameValues_AreEqual()
    {
        Assert.Equal(new Rectangle(1, 2, 3, 4), new Rectangle(1, 2, 3, 4));
        Assert.NotEqual(new Rectangle(1, 2, 3, 4), new Rectangle(1, 2, 3, 5));
    }

    [Fact]
    public void ToString_ShowsThePositionAndSize()
    {
        Assert.Equal("[(-1,2) 3x4]", new Rectangle(-1, 2, 3, 4).ToString());
    }
}

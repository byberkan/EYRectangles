namespace EYRectangles.Tests;

public sealed class IntersectionTests
{
    [Fact]
    public void GetIntersectionPoints_FindsFourPoints_WhenOneRectanglePassesThroughTheOther()
    {
        // The second rectangle is taller than the first, so both of its vertical sides cut both
        // horizontal sides of the first: two sides times two sides gives four crossings.
        var first = new Rectangle(0, 0, 10, 6);
        var second = new Rectangle(3, -2, 4, 10);

        AssertPoints(
            RectangleAnalyzer.GetIntersectionPoints(first, second),
            new Point2D(3, 0), new Point2D(3, 6), new Point2D(7, 0), new Point2D(7, 6));
    }

    [Fact]
    public void GetIntersectionPoints_FindsTwoPoints_WhenOnlyTheCornersOverlap()
    {
        // Here just one side of each rectangle cuts one side of the other, so there are two crossings.
        var first = new Rectangle(0, 0, 6, 4);
        var second = new Rectangle(4, 2, 6, 4);

        AssertPoints(
            RectangleAnalyzer.GetIntersectionPoints(first, second),
            new Point2D(4, 4), new Point2D(6, 2));
    }

    [Fact]
    public void GetIntersectionPoints_FindsTwoPoints_ForIntersectionWithoutContainment()
    {
        var first = new Rectangle(0, 0, 6, 6);
        var second = new Rectangle(4, 2, 5, 2);

        AssertPoints(
            RectangleAnalyzer.GetIntersectionPoints(first, second),
            new Point2D(6, 2), new Point2D(6, 4));
    }

    [Fact]
    public void GetIntersectionPoints_FindsNothing_WhenTheRectanglesAreSeparated()
    {
        var first = new Rectangle(0, 0, 4, 4);
        var second = new Rectangle(10, 10, 3, 3);

        Assert.Empty(RectangleAnalyzer.GetIntersectionPoints(first, second));
    }

    [Fact]
    public void GetIntersectionPoints_FindsNothing_WhenOneRectangleIsStrictlyInsideTheOther()
    {
        var outer = new Rectangle(0, 0, 10, 10);
        var inner = new Rectangle(2, 2, 4, 4);

        Assert.Empty(RectangleAnalyzer.GetIntersectionPoints(outer, inner));
    }

    [Fact]
    public void GetIntersectionPoints_FindsTheTouchPoints_WhenAContainedRectangleRestsOnTheBoundary()
    {
        var outer = new Rectangle(0, 0, 10, 10);
        var inner = new Rectangle(0, 2, 4, 3);

        AssertPoints(
            RectangleAnalyzer.GetIntersectionPoints(outer, inner),
            new Point2D(0, 2), new Point2D(0, 5));
    }

    [Fact]
    public void GetIntersectionPoints_FindsASinglePoint_WhenTheRectanglesMeetAtACorner()
    {
        var first = new Rectangle(0, 0, 4, 4);
        var second = new Rectangle(4, 4, 4, 4);

        AssertPoints(
            RectangleAnalyzer.GetIntersectionPoints(first, second),
            new Point2D(4, 4));
    }

    [Fact]
    public void GetIntersectionPoints_FindsTheFourCorners_ForIdenticalRectangles()
    {
        // The outlines coincide everywhere. The corners are the only crossings that are not part of a
        // collinear overlap, and collinear overlaps are reported as adjacency instead.
        var first = new Rectangle(0, 0, 4, 4);
        var second = new Rectangle(0, 0, 4, 4);

        AssertPoints(
            RectangleAnalyzer.GetIntersectionPoints(first, second),
            new Point2D(0, 0), new Point2D(0, 4), new Point2D(4, 0), new Point2D(4, 4));
    }

    [Theory]
    [InlineData(0, 0, 4, 4, 4, 0, 4, 4, 4, 0, 4, 4)] // proper adjacency
    [InlineData(0, 0, 4, 6, 4, 1, 3, 3, 4, 1, 4, 4)] // sub-line adjacency
    [InlineData(0, 0, 4, 4, 4, 2, 4, 4, 4, 2, 4, 4)] // partial adjacency
    public void GetIntersectionPoints_ReturnsTheEndsOfTheSharedSide_ForAdjacentRectangles(
        int firstLeftX, int firstBottomY, int firstWidth, int firstHeight,
        int secondLeftX, int secondBottomY, int secondWidth, int secondHeight,
        int lowerX, int lowerY, int upperX, int upperY)
    {
        var first = new Rectangle(firstLeftX, firstBottomY, firstWidth, firstHeight);
        var second = new Rectangle(secondLeftX, secondBottomY, secondWidth, secondHeight);

        AssertPoints(
            RectangleAnalyzer.GetIntersectionPoints(first, second),
            new Point2D(lowerX, lowerY), new Point2D(upperX, upperY));
    }

    [Fact]
    public void GetIntersectionPoints_WorksWithNegativeCoordinates()
    {
        var first = new Rectangle(-8, -6, 4, 4);
        var second = new Rectangle(-4, -6, 4, 4);

        AssertPoints(
            RectangleAnalyzer.GetIntersectionPoints(first, second),
            new Point2D(-4, -6), new Point2D(-4, -2));
    }

    [Fact]
    public void GetIntersectionPoints_ReturnsDistinctPoints()
    {
        var first = new Rectangle(0, 0, 4, 4);
        var second = new Rectangle(0, 0, 4, 4);

        var points = RectangleAnalyzer.GetIntersectionPoints(first, second);

        Assert.Equal(points.Distinct().Count(), points.Count);
    }

    [Theory]
    [InlineData(0, 0, 10, 6, 3, -2, 4, 10)]
    [InlineData(0, 0, 6, 4, 4, 2, 6, 4)]
    [InlineData(0, 0, 4, 4, 4, 4, 4, 4)]
    [InlineData(0, 0, 10, 10, 2, 2, 4, 4)]
    public void GetIntersectionPoints_GivesTheSameAnswerRegardlessOfArgumentOrder(
        int firstLeftX, int firstBottomY, int firstWidth, int firstHeight,
        int secondLeftX, int secondBottomY, int secondWidth, int secondHeight)
    {
        var first = new Rectangle(firstLeftX, firstBottomY, firstWidth, firstHeight);
        var second = new Rectangle(secondLeftX, secondBottomY, secondWidth, secondHeight);

        Assert.Equal(
            Ordered(RectangleAnalyzer.GetIntersectionPoints(first, second)),
            Ordered(RectangleAnalyzer.GetIntersectionPoints(second, first)));
    }

    [Theory]
    [InlineData(0, 0, 10, 6, 3, -2, 4, 10, true)]
    [InlineData(0, 0, 4, 4, 10, 10, 3, 3, false)]
    [InlineData(0, 0, 10, 10, 2, 2, 4, 4, false)]
    [InlineData(0, 0, 4, 4, 4, 4, 4, 4, true)]
    public void Intersects_AgreesWithThePointList(
        int firstLeftX, int firstBottomY, int firstWidth, int firstHeight,
        int secondLeftX, int secondBottomY, int secondWidth, int secondHeight,
        bool expected)
    {
        var first = new Rectangle(firstLeftX, firstBottomY, firstWidth, firstHeight);
        var second = new Rectangle(secondLeftX, secondBottomY, secondWidth, secondHeight);

        Assert.Equal(expected, RectangleAnalyzer.Intersects(first, second));
    }

    // The exercise asks which points are found, not in what order, so comparisons normalise the order.
    private static void AssertPoints(IReadOnlyList<Point2D> actual, params Point2D[] expected) =>
        Assert.Equal(Ordered(expected), Ordered(actual));

    private static IEnumerable<Point2D> Ordered(IEnumerable<Point2D> points) =>
        points.OrderBy(point => point.X).ThenBy(point => point.Y);
}

namespace EYRectangles;

// The three algorithms required by the exercise. Every method is pure and runs in constant time.
public static class RectangleAnalyzer
{
  // True when inner lies wholly inside outer. Touching the boundary from the inside still counts.
  public static bool Contains(Rectangle outer, Rectangle inner)
  {
    return inner.LeftX >= outer.LeftX &&
           inner.RightX <= outer.RightX &&
           inner.BottomY >= outer.BottomY &&
           inner.TopY <= outer.TopY;
  }

  // Classifies how two rectangles share a side.
  // Only one branch can apply: if the left/right sides touch, the X ranges meet at a single line and
  // cannot also overlap, so a vertical touch rules out a horizontal one.
  public static AdjacencyType GetAdjacency(Rectangle first, Rectangle second)
  {
    // Left and right sides touch, so the shared side is vertical: measure along Y.
    if (first.RightX == second.LeftX || first.LeftX == second.RightX)
      return Classify(
        Overlap(first.BottomY, first.TopY, second.BottomY, second.TopY),
        first.Height,
        second.Height);

    // Top and bottom sides touch, so the shared side is horizontal: measure along X.
    if (first.TopY == second.BottomY || first.BottomY == second.TopY)
      return Classify(
        Overlap(first.LeftX, first.RightX, second.LeftX, second.RightX),
        first.Width,
        second.Width);

    return AdjacencyType.None;
  }

  // Length shared by two ranges on one axis; zero when they do not overlap.
  private static int Overlap(int firstStart, int firstEnd, int secondStart, int secondEnd)
  {
    return Math.Max(0, Math.Min(firstEnd, secondEnd) - Math.Max(firstStart, secondStart));
  }

  // Names the adjacency by asking which of the two sides the shared length covers completely.
  private static AdjacencyType Classify(int sharedLength, int firstSide, int secondSide)
  {
    if (sharedLength == 0) return AdjacencyType.None; // no contact, or a corner touch

    var coversFirst = sharedLength == firstSide;
    var coversSecond = sharedLength == secondSide;

    if (coversFirst && coversSecond) return AdjacencyType.Proper;
    if (coversFirst || coversSecond) return AdjacencyType.SubLine;

    return AdjacencyType.Partial;
  }

  // True when the two outlines meet anywhere.
  public static bool Intersects(Rectangle first, Rectangle second)
  {
    return GetIntersectionPoints(first, second).Count > 0;
  }

  // Every distinct point where the outline of one rectangle crosses the outline of the other.
  // Only a vertical side and a horizontal side can meet in a single point, so those are the pairs
  // tested. Two collinear sides overlap along a whole segment instead, which is what GetAdjacency
  // reports; the ends of that segment still show up here through the perpendicular pairs.
  public static IReadOnlyList<Point2D> GetIntersectionPoints(Rectangle first, Rectangle second)
  {
    var points = new List<Point2D>();

    // The 2 vertical of first against the 2 horizontal of second, and vice versa.
    // Each vertical side of one rectangle can meet each horizontal side of the other rectangle in a single point.
    TryAddIntersection(points, first.LeftX, first.BottomY, first.TopY, second.BottomY, second.LeftX, second.RightX);
    TryAddIntersection(points, first.LeftX, first.BottomY, first.TopY, second.TopY, second.LeftX, second.RightX);

    TryAddIntersection(points, first.RightX, first.BottomY, first.TopY, second.BottomY, second.LeftX, second.RightX);
    TryAddIntersection(points, first.RightX, first.BottomY, first.TopY, second.TopY, second.LeftX, second.RightX);

    TryAddIntersection(points, second.LeftX, second.BottomY, second.TopY, first.BottomY, first.LeftX, first.RightX);
    TryAddIntersection(points, second.LeftX, second.BottomY, second.TopY, first.TopY, first.LeftX, first.RightX);

    TryAddIntersection(points, second.RightX, second.BottomY, second.TopY, first.BottomY, first.LeftX, first.RightX);
    TryAddIntersection(points, second.RightX, second.BottomY, second.TopY, first.TopY, first.LeftX, first.RightX);

    return points;
  }

  private static void TryAddIntersection(List<Point2D> points, int verticalX, int verticalBottom, int verticalTop,
    int horizontalY, int horizontalLeft, int horizontalRight)
  {
    var xIsInside = verticalX >= horizontalLeft && verticalX <= horizontalRight;
    var yIsInside = horizontalY >= verticalBottom && horizontalY <= verticalTop;

    if (xIsInside && yIsInside) {
      var point = new Point2D(verticalX, horizontalY);
      if (!points.Contains(point)) points.Add(point);
    }
  }
}
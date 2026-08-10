namespace EYRectangles;

// How two rectangles share a side, as defined in Appendix 3 of the exercise.
public enum AdjacencyType
{
    None,     // no shared side; a corner touch lands here too
    Proper,   // both rectangles share their complete side
    SubLine,  // the complete side of exactly one rectangle is shared
    Partial   // only part of each side is shared
}

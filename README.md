# Rectangles

An implementation of the **Rectangles** exercise: intersection, containment and adjacency analysis for
axis-aligned rectangles.

Written in **C# on .NET 10**, which the brief lists among the accepted languages ("Python/C or
C++/Java/C#/Javascript will all be accepted"). It compiles and runs on Linux — see
[Build, test and run](#build-test-and-run).

---

## Requirement coverage

Everything the brief asks for, and where to find it.

| Requirement | Where |
|---|---|
| **1. Intersection** — determine whether two rectangles have intersecting lines and identify the points | `RectangleAnalyzer.Intersects` / `RectangleAnalyzer.GetIntersectionPoints` |
| **2. Containment** — determine whether a rectangle is wholly contained within another | `RectangleAnalyzer.Contains` |
| **3. Adjacency** — detect adjacency, distinguishing proper / sub-line / partial | `RectangleAnalyzer.GetAdjacency`, `AdjacencyType` |
| Implementation of the rectangle entity | `Rectangle.cs` (plus `Point2D.cs` for the result type) |
| Appropriate documentation | this README, plus a comment on every method explaining what it does |
| Test cases / unit tests | 65 xUnit tests in `tests/EYRectangles.Tests` |
| Runs on Linux | `dotnet run`, or publish a native Linux executable — [below](#build-test-and-run) |
| Library / framework dependencies documented | [Dependencies](#dependencies) |
| Expansions documented | [Extensions beyond the brief](#extensions-beyond-the-brief) |

---

## Dependencies

| Dependency | Version | Needed for |
|---|---|---|
| .NET SDK | 10.0 | building and running |
| xunit | 2.9.3 | unit tests only |
| xunit.runner.visualstudio | 3.1.4 | unit tests only |
| Microsoft.NET.Test.Sdk | 17.14.1 | unit tests only |

**The application itself has no third-party dependencies** — only the .NET base class library. The three
test packages are restored automatically by `dotnet test`.

Installing the SDK on Linux: <https://learn.microsoft.com/dotnet/core/install/linux>

## Build, test and run

From the repository root:

```bash
dotnet build                             # compile
dotnet test                              # run the 65 unit tests
dotnet run --project src/EYRectangles    # print the report described below
```

To produce a standalone Linux executable:

```bash
dotnet publish src/EYRectangles -c Release -r linux-x64 --self-contained false -o ./publish
./publish/EYRectangles
```

Add `--self-contained true` if the target machine has no .NET runtime installed.

Running the executable prints the result of all three algorithms for every scenario drawn in Appendices
1–3, plus three edge cases.

## Project structure

```
EYRectangles.slnx
src/EYRectangles/
  Rectangle.cs           the rectangle entity
  Point2D.cs             an integer coordinate pair, used for intersection points
  AdjacencyType.cs       None / Proper / SubLine / Partial
  RectangleAnalyzer.cs   the three algorithms
  Program.cs             console report over the appendix scenarios
tests/EYRectangles.Tests/
  RectangleTests.cs  ContainmentTests.cs  AdjacencyTests.cs  IntersectionTests.cs
```

---

## The rectangle entity

```csharp
var rectangle = new Rectangle(leftX: 0, bottomY: 0, width: 4, height: 4);

rectangle.LeftX;    // 0    given
rectangle.BottomY;  // 0    given
rectangle.Width;    // 4    given, must be > 0
rectangle.Height;   // 4    given, must be > 0
rectangle.RightX;   // 4    derived: LeftX + Width
rectangle.TopY;     // 4    derived: BottomY + Height
```

A rectangle is anchored at its bottom-left corner and sized by a positive width and height. It is
immutable, validated in the constructor, and compared by value.

## The algorithms

All three are pure static methods on `RectangleAnalyzer`, and all run in constant time.

### 1. Intersection

> *"You must be able to determine whether two rectangles have one or more intersecting lines and produce a
> result identifying the points of intersection."*

```csharp
bool intersects = RectangleAnalyzer.Intersects(first, second);
IReadOnlyList<Point2D> points = RectangleAnalyzer.GetIntersectionPoints(first, second);
```

The boolean answers "do they intersect", the list identifies *where* — the points at which the two
outlines cross. Each vertical side is tested against each horizontal side of the other rectangle, eight
pairs in total, so at most eight points are produced. Points are distinct and ordered by X then Y.

### 2. Containment

> *"You must be able to determine whether a rectangle is wholly contained within another rectangle."*

```csharp
bool contained = RectangleAnalyzer.Contains(outer: first, inner: second);
```

Four boundary comparisons. The call is directional — it asks whether `inner` sits inside `outer`, not the
other way round.

### 3. Adjacency

> *"Adjacency is defined as the sharing of at least one side. Side sharing may be proper, sub-line or
> partial. A sub-line share is a share where one side of rectangle A is a line that exists as a set of
> points wholly contained on some other side of rectangle B, where partial is one where some line segment
> on a side of rectangle A exists as a set of points on some side of Rectangle B."*

```csharp
AdjacencyType adjacency = RectangleAnalyzer.GetAdjacency(first, second);
```

The problem reduces to a one-dimensional overlap. If two facing sides lie on the same line, the shared
length is the overlap of the two ranges along the other axis. Comparing that length against the two side
lengths names the kind of share:

| Result | Shared length | Meaning | Example |
|---|---|---|---|
| `Proper` | equals **both** side lengths | the two sides are identical | `[(0,0) 4x4]` and `[(4,0) 4x4]` |
| `SubLine` | equals **exactly one** side length | one side lies wholly within the other | `[(0,0) 4x6]` and `[(4,1) 3x3]` |
| `Partial` | greater than zero, equal to neither | the sides overlap only in part | `[(0,0) 4x4]` and `[(4,2) 4x4]` |
| `None` | zero | no shared segment, or a corner touch | `[(0,0) 4x4]` and `[(6,0) 4x4]` |

---

## Reading the intersection output

Intersection points are where the two **outlines cross**, so the count depends on the arrangement. Two
examples from the report, since the difference surprises people:

**Four points — one rectangle passes through the other.**
`A = [(0,0) 10x6]`, `B = [(3,-2) 4x10]`. B is taller than A, so it enters through the bottom and leaves
through the top:

```
   y=8       +---+              B pokes out the top
             |   |
   y=6 ......|...|.........  <- A's top side      crossed at (3,6) and (7,6)
       +-----+---+--------+
       |     | B |    A   |
       +-----+---+--------+
   y=0 ......|...|.........  <- A's bottom side   crossed at (3,0) and (7,0)
             |   |
  y=-2       +---+              B pokes out the bottom
      x=0    3   7       10
```

Both of B's vertical sides cut both of A's horizontal sides: 2 x 2 = **4 points**.

**Two points — the corners overlap.**
`A = [(0,0) 6x4]`, `B = [(4,2) 6x4]`:

```
   y=6         +----------+
               |          |
   y=4 ........+---+      |   <- (4,4): B's left side cuts A's top side
       +-------|///|   B  |
       |   A   |   |      |
       |       +---+------+   <- (6,2): A's right side cuts B's bottom side
       |           |
       +-----------+
   y=0
      x=0      4   6     10
```

Only one side of each cuts one side of the other: 1 + 1 = **2 points**.

In full: passing through gives 4, a corner overlap gives 2, a corner touch gives 1, separated rectangles
give 0, and 8 is the maximum.

---

## Assumptions

The brief leaves several things open. These are the decisions taken, and why.

### Geometry and representation

- Rectangles are axis-aligned and cannot be rotated. The brief's diagrams show only axis-aligned
  rectangles, and rotation would change every algorithm.
- Coordinates and dimensions are represented using integers.
- The Y axis increases upwards, so `BottomY < TopY`.
- `LeftX` and `BottomY` define the rectangle's starting coordinates (the bottom-left corner).
- `RightX = LeftX + Width` and `TopY = BottomY + Height`.
- Width and height must be greater than zero. A zero-width or zero-height "rectangle" is a line or a
  point, for which side sharing and containment stop being meaningful, so the constructor rejects it with
  `ArgumentOutOfRangeException`.
- Negative coordinates are supported.
- Coordinates are assumed to stay well inside the `int` range; the derived `RightX` / `TopY` are not
  overflow-checked.
- Nullable reference types are enabled, so passing `null` is a compile-time error rather than a runtime
  guard.

### Containment

- Containment includes rectangles touching the containing rectangle's boundary from the inside — the
  brief says *wholly contained*, and a rectangle flush against the inside of a boundary is still wholly
  inside it.
- Containment is directional: `Contains(outer, inner)` asks whether `inner` sits inside `outer`.
- Identical rectangles therefore contain each other.

### Adjacency

- Adjacency requires a shared side segment with **positive length**; corner-only contact is not adjacency.
  The brief says sharing of a *side*, and a single point is not a side.
- **Proper** adjacency means the complete side of both rectangles is shared.
- **Sub-line** adjacency means the complete side of exactly one rectangle is shared.
- **Partial** adjacency means only part of both sides is shared.
- The three kinds are tested most-specific-first. Taken literally, the brief's definition of *partial*
  ("some line segment on a side of A exists as a set of points on some side of B") also describes proper
  and sub-line shares, so `Partial` is used only when neither of the stricter cases applies.
- The rectangles must sit on **opposite sides** of the shared line. A contained rectangle whose side is
  flush with a side of its container is not adjacent — the two overlap rather than sit side by side.
- Adjacency is symmetric, and exactly one classification is returned. A vertical touch pins the X ranges
  to a single line so they cannot also overlap, which makes the vertical and horizontal cases mutually
  exclusive.

### Intersection

- Intersection refers to **discrete boundary intersection points**, not the overlapping area between
  rectangles. The brief asks for "the points of intersection", and an overlap region is not a point set
  that can be listed.
- Only a vertical side and a horizontal side can meet in a single point, so those are the pairs tested.
- Collinear shared sides are treated as adjacency rather than discrete intersection points: two collinear
  overlapping sides meet along a whole segment, which is not a finite set of points. The ends of that
  segment still surface through the perpendicular side pairs, which is why adjacent rectangles report the
  two ends of the side they share.
- Consequences worth calling out:
  - Rectangles meeting at a single corner report one intersection point but no adjacency.
  - A strictly contained rectangle reports no intersection points — matching the brief's Appendix 2, where
    containment and intersection are shown as separate outcomes.
  - Identical rectangles report their four corners.
- Results are distinct and ordered by X then Y, so the output does not depend on argument order.

---

## Design notes

**`Rectangle` is a `record` class, not a struct.** A struct can be produced through `default`, which
bypasses the constructor and would yield a zero-sized rectangle that the validation is meant to prevent.
`record` still gives value equality and a readable `ToString`.

**Adjacency reduces to one dimension.** Once the facing sides are known to lie on the same line, the
shared length is `max(0, min(ends) - max(starts))` along the other axis. Everything else is naming the
result. A shared length of zero covers both "no contact" and "corner touch", so both fall out of the same
branch.

**Intersection is a perpendicular-pairs sweep.** A vertical side at `x` meets a horizontal side at `y`
exactly when `x` falls within the horizontal side's span and `y` falls within the vertical side's span,
giving the point `(x, y)`. Shared corners are reachable from both rectangles, so the list is kept distinct.

**Everything is pure and constant-time.** No shared state and no mutation of the inputs, so the algorithms
are trivially safe to call concurrently.

## Tests

65 xUnit tests, run with `dotnet test`:

| File | Covers |
|---|---|
| `RectangleTests.cs` | construction, derived sides, rejection of non-positive width/height, negative coordinates, value equality |
| `ContainmentTests.cs` | strict containment, boundary contact, identical rectangles, directionality, overlap, separation |
| `AdjacencyTests.cs` | proper / sub-line / partial in every orientation, corner contact, collinear non-overlap, contained-and-flush, symmetry |
| `IntersectionTests.cs` | passing through, corner overlap, containment, separation, corner touch, identical rectangles, adjacency endpoints, distinctness, ordering, argument-order independence |

The scenarios from Appendices 1–3 all appear as test cases, alongside the degenerate cases the diagrams
do not cover.

## Extensions beyond the brief

Documented additions, as the brief invites:

1. **`Intersects`** — a boolean shorthand over the point list, so the "determine whether" and "identify the
   points" halves of requirement 1 are each directly answerable.
2. **A console report** covering every scenario from Appendices 1–3 plus three edge cases (corner-only
   contact, identical rectangles, negative coordinates), so running the executable demonstrates the whole
   feature set without writing any code.
3. **Deterministic ordering** of intersection points, which makes the output stable and the tests readable.
4. **Warnings treated as errors** across both projects.

using System.Numerics;

namespace Daily.Drawables
{
    public class TaskProgressIndicatorDrawable : IDrawable
    {
        public int repeatCount;
        public int targetRepeatCount;

        public Color incompletedColor = Colors.White;
        public Color completedColor = Colors.Green;

        private const float bigEllipseSize = 20f;
        private const float mediumEllipseSize = bigEllipseSize * mediumSizeFactor;
        private const float smallEllipseSize = bigEllipseSize * smallSizeFactor;

        private const float mediumSizeFactor = 0.7f;
        private const float smallSizeFactor = 0.6f;

        private const float doubleOffset = 0.45f;
        private const float tripleOffset = 0.75f;

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            Action<ICanvas, RectF> drawAction;

            switch (targetRepeatCount)
            {
                case 1:
                    drawAction = DrawSingle;
                    break;
                case 2:
                    drawAction = DrawDouble;
                    break;
                case 3:
                    drawAction = DrawTriple;
                    break;
                default:
                    throw new Exception($"Invalid {nameof(targetRepeatCount)}: {targetRepeatCount}");
            }

            drawAction.Invoke(canvas, dirtyRect);
        }

        private void DrawSingle(ICanvas canvas, RectF rect)
        {
            Vector2 center = CalculateCenterDrawPoint(rect.Width, rect.Height, bigEllipseSize);

            canvas.FillColor = targetRepeatCount == repeatCount ? completedColor : incompletedColor;

            canvas.FillEllipse(center.X, center.Y, bigEllipseSize, bigEllipseSize);
        }

        private void DrawDouble(ICanvas canvas, RectF rect)
        {
            Vector2 center = CalculateCenterDrawPoint(rect.Width, rect.Height, mediumEllipseSize);

            Span<float> xPositions = stackalloc float[]
            { 
                center.X - (center.X * doubleOffset),
                center.X + (center.X * doubleOffset)
            };

            DrawMultiple(canvas, ref xPositions, center.Y, mediumEllipseSize);
        }

        private void DrawTriple(ICanvas canvas, RectF rect)
        {
            Vector2 center = CalculateCenterDrawPoint(rect.Width, rect.Height, smallEllipseSize);

            Span<float> xPositions = stackalloc float[]
            { 
                center.X - (center.X * tripleOffset),
                center.X,
                center.X + (center.X * tripleOffset)
            };

            DrawMultiple(canvas, ref xPositions, center.Y, smallEllipseSize);
        }

        private Vector2 CalculateCenterDrawPoint(float width, float height, float ellipseSize)
        {
            float CalculateByAxis(float axis) => (axis / 2f) - (ellipseSize / 2f);

            Vector2 result = new Vector2();

            result.X = CalculateByAxis(width);
            result.Y = CalculateByAxis(height);

            return result;
        }

        private void DrawMultiple(ICanvas canvas, ref Span<float> xPositions, float yPosition, float ellipseSize)
        {
            bool isCompleted;

            for (int i = 0; i < targetRepeatCount; i++)
            {
                isCompleted = repeatCount >= i + 1;

                canvas.FillColor = isCompleted ? completedColor : incompletedColor;

                canvas.FillEllipse(xPositions[i], yPosition, ellipseSize, ellipseSize);
            }
        }
    }
}
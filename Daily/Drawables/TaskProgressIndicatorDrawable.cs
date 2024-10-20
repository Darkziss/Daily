using System.Numerics;

namespace Daily.Drawables
{
    public class TaskProgressIndicatorDrawable : IDrawable
    {
        public int repeatedCount = 0;
        public int repeatCount = 1;

        public Color incompletedColor = Colors.White;
        public Color completedColor = Colors.Green;

        private const float bigEllipseSize = 20f;
        private const float mediumEllipseSize = bigEllipseSize * mediumEllipseSizeFactor;
        private const float smallEllipseSize = bigEllipseSize * smallEllipseSizeFactor;

        private const float mediumEllipseSizeFactor = 0.7f;
        private const float smallEllipseSizeFactor = 0.6f;

        private const float tripleOffset = 1.7f;

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            Action<ICanvas, RectF> drawAction;

            switch (repeatCount)
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
                    throw new Exception($"Invalid {nameof(repeatCount)}: {repeatCount}");
            }

            drawAction.Invoke(canvas, dirtyRect);
        }

        private void DrawSingle(ICanvas canvas, RectF dirtyRect)
        {
            canvas.FillColor = repeatedCount == repeatCount ? completedColor : incompletedColor;
            
            Vector2 center = CalculateCenterDrawPoint(dirtyRect.Width, dirtyRect.Height, bigEllipseSize);

            canvas.FillEllipse(center.X, center.Y, bigEllipseSize, bigEllipseSize);
        }

        private void DrawDouble(ICanvas canvas, RectF dirtyRect)
        {
            Vector2 center = CalculateCenterDrawPoint(dirtyRect.Width, dirtyRect.Height, mediumEllipseSize);

            float[] xPositions = new float[] 
            { 
                center.X - center.X,
                center.X + center.X
            };

            Span<bool> completed = stackalloc bool[2];

            for (int i = 0; i < 2; i++)
            {
                completed[i] = repeatedCount >= i + 1;

                canvas.FillColor = completed[i] ? completedColor : incompletedColor;

                canvas.FillEllipse(xPositions[i], center.Y, mediumEllipseSize, mediumEllipseSize);
            }
        }

        private void DrawTriple(ICanvas canvas, RectF dirtyRect)
        {
            Vector2 center = CalculateCenterDrawPoint(dirtyRect.Width, dirtyRect.Height, smallEllipseSize);

            float[] xPositions = new float[] 
            { 
                center.X - (center.X * tripleOffset),
                center.X,
                center.X + (center.X * tripleOffset)
            };

            Span<bool> completed = stackalloc bool[3];

            for (int i = 0; i < 3; i++)
            {
                completed[i] = repeatedCount >= i + 1;
                
                canvas.FillColor = completed[i] ? completedColor : incompletedColor;

                canvas.FillEllipse(xPositions[i], center.Y, smallEllipseSize, smallEllipseSize);
            }
        }

        private Vector2 CalculateCenterDrawPoint(float width, float height, float ellipseSize)
        {
            float CalculateByAxis(float axis) => (axis / 2f) - (ellipseSize / 2f);

            Vector2 result = new Vector2();

            result.X = CalculateByAxis(width);
            result.Y = CalculateByAxis(height);

            return result;
        }
    }
}
using System.Numerics;

namespace Daily.Drawables
{
    public class RadialProgressBarDrawable : IDrawable
    {
        public int repeatCount;
        public int targetRepeatCount;

        public Color strokeColor;
        public float strokeSize;

        public Color progressFillColor = Colors.White;

        public Color completedFillColor = Colors.Green;

        private const float startAngle = 90f;

        private const bool clockwise = true;
        private const bool closed = false;

        public RadialProgressBarDrawable(int repeatCount, int targetRepeatCount, Color strokeColor, float strokeSize, 
            Color progressFillColor, Color completedColor)
        {
            this.repeatCount = repeatCount;
            this.targetRepeatCount = targetRepeatCount;

            this.strokeColor = strokeColor;
            this.strokeSize = strokeSize;

            this.progressFillColor = progressFillColor;

            this.completedFillColor = completedColor;
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            float width = dirtyRect.Width;
            float height = dirtyRect.Height;

            float radius = height;

            Vector2 center = new Vector2(width / 2f, height / 2f);

            bool isEmpty = repeatCount == 0;
            bool isFull = repeatCount == targetRepeatCount;

            if (!isEmpty && !isFull)
            {
                DrawProgressArc(canvas, width, height, radius, center);
            }

            if (isFull)
            {
                DrawCompletedCircle(canvas, radius, center);
            }
            else
            {
                DrawEllipseStroke(canvas, width, height, center);
            }
        }

        private void DrawCompletedCircle(ICanvas canvas, float radius, Vector2 center)
        {
            canvas.FillColor = completedFillColor;
            canvas.StrokeColor = Colors.Transparent;
            canvas.StrokeSize = 0f;
            canvas.FillCircle(center.X, center.Y, radius);
        }

        private void DrawProgressArc(ICanvas canvas, float width, float height, float radius, Vector2 center)
        {
            Vector2 drawPoint = new Vector2(center.X - width / 2f, center.Y - height / 2f);

            float fillAmount = (float)repeatCount / (float)targetRepeatCount;
            float endAngle = startAngle - fillAmount * 360f;

            canvas.FillColor = Colors.Transparent;
            canvas.StrokeColor = progressFillColor;
            canvas.StrokeSize = radius;
            canvas.DrawArc(drawPoint.X, drawPoint.Y, width, height, startAngle, endAngle,
                clockwise, closed);
        }

        private void DrawEllipseStroke(ICanvas canvas, float width, float height, Vector2 center)
        {
            RectF rectF = new(center.X - width, center.Y - height, width * 2f, height * 2f);

            canvas.FillColor = Colors.Transparent;
            canvas.StrokeColor = strokeColor;
            canvas.StrokeSize = strokeSize;
            canvas.DrawEllipse(rectF);
        }
    }
}
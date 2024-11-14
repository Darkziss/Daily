using System.Numerics;

namespace Daily.Drawables
{
    public class RadialProgressBarDrawable : IDrawable
    {
        public int repeatCount;
        public int targetRepeatCount;

        public Color progressFillColor = Colors.White;
        public Color backgroundFillColor = Colors.Gray;

        public Color completedColor = Colors.Green;

        public RadialProgressBarDrawable(int repeatCount, int targetRepeatCount, Color progressFillColor, Color backgroundFillColor, Color completedColor)
        {
            this.repeatCount = repeatCount;
            this.targetRepeatCount = targetRepeatCount;

            this.progressFillColor = progressFillColor;
            this.backgroundFillColor = backgroundFillColor;

            this.completedColor = completedColor;
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            float radius = dirtyRect.Width / 2f;
            Vector2 center = CalculateCenterDrawPoint(dirtyRect.Width, dirtyRect.Height, radius);

            bool isEmpty = repeatCount == 0;
            bool isFull = repeatCount == targetRepeatCount;

            if (isEmpty)
            {
                canvas.FillColor = backgroundFillColor;
                canvas.FillCircle(center.X, center.Y, radius);
            }
            else if (isFull)
            {
                canvas.FillColor = completedColor;

                canvas.FillCircle(center.X, center.Y, radius);
            }
            else
            {
                const float startAngle = 90f;
                const bool clockwise = true;

                float fillAmount = (float)repeatCount / (float)targetRepeatCount;
                float endAngle = fillAmount * 360f;

                canvas.FillColor = backgroundFillColor;
                canvas.FillCircle(center.X, center.Y, radius);

                canvas.FillColor = progressFillColor;
                canvas.FillArc(center.X, center.Y, radius, radius, startAngle, endAngle, clockwise);
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
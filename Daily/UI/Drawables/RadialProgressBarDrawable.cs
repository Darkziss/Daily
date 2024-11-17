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
            const float radiusDivisor = 4f;
            float radius = dirtyRect.Height / radiusDivisor;

            Vector2 center = new Vector2(dirtyRect.Width / 2f, dirtyRect.Height / 2f);

            const float startAngle = 90f;

            const bool clockwise = true;
            const bool closed = false;

            bool isEmpty = repeatCount == 0;
            bool isFull = repeatCount == targetRepeatCount;

            canvas.FillColor = isFull ? completedColor : backgroundFillColor;
            canvas.FillCircle(center.X, center.Y, radius);

            if (!isEmpty && !isFull)
            {
                Vector2 arcCenter = new Vector2(center.X - radius / 2f, center.Y - radius / 2f);

                float fillAmount = (float)repeatCount / (float)targetRepeatCount;
                float endAngle = startAngle - fillAmount * 360f;

                canvas.FillCircle(center.X, center.Y, radius);

                canvas.StrokeColor = progressFillColor;
                canvas.StrokeSize = radius;

                canvas.DrawArc(arcCenter.X, arcCenter.Y, radius, radius, startAngle, endAngle, 
                    clockwise, closed);
            }
        }
    }
}
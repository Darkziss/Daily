using System.Numerics;

namespace Daily.Drawables
{
    public class RadialProgressBarDrawable : IDrawable
    {
        public int repeatCount;
        public int targetRepeatCount;

        public Color progressFillColor = Colors.White;
        public Color backgroundFillColor = Colors.Gray;

        public Color completedFillColor = Colors.Green;

        private const float startAngle = 90f;

        private const bool clockwise = true;
        private const bool closed = false;

        public RadialProgressBarDrawable(int repeatCount, int targetRepeatCount, Color progressFillColor, Color backgroundFillColor, Color completedColor)
        {
            this.repeatCount = repeatCount;
            this.targetRepeatCount = targetRepeatCount;

            this.progressFillColor = progressFillColor;
            this.backgroundFillColor = backgroundFillColor;

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

            canvas.FillColor = isFull ? completedFillColor : backgroundFillColor;
            canvas.FillCircle(center.X, center.Y, radius);

            if (!isEmpty && !isFull)
            {
                Vector2 arcCenter = new Vector2(center.X - width / 2f, center.Y - height / 2f);

                float fillAmount = (float)repeatCount / (float)targetRepeatCount;
                float endAngle = startAngle - fillAmount * 360f;

                canvas.StrokeColor = progressFillColor;
                canvas.StrokeSize = radius;
                canvas.DrawArc(arcCenter.X, arcCenter.Y, width, height, startAngle, endAngle,
                    clockwise, closed);
            }
        }
    }
}
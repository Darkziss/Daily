
namespace Daily.Drawables
{
    public class TaskProgressIndicatorDrawable : BindableObject, IDrawable
    {
        public Color IndicatorColor { get; set; } = Colors.White;

        private const float ellipseSize = 20f;
        
        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.FillColor = IndicatorColor;

            float x = (dirtyRect.Width / 2f) - (ellipseSize / 2f);
            float y = (dirtyRect.Height / 2f) - (ellipseSize / 2f);

            canvas.FillEllipse(x, y, ellipseSize, ellipseSize);
        }
    }
}
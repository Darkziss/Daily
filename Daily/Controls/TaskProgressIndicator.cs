using Daily.Drawables;

namespace Daily.Controls
{
    public class TaskProgressIndicator : GraphicsView
    {
        private readonly TaskProgressIndicatorDrawable _indicatorDrawable;

        public Color IndicatorColor
        {
            get { return _indicatorDrawable.IndicatorColor; }
            set { OnIndicatorColorChanged(value); }
        }

        public TaskProgressIndicator()
        {
            _indicatorDrawable = new TaskProgressIndicatorDrawable();
            Drawable = _indicatorDrawable;
        }

        private void OnIndicatorColorChanged(object color)
        {
            _indicatorDrawable.IndicatorColor = (Color)color;
            Invalidate();
        }
    }
}
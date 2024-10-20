using Daily.Drawables;

namespace Daily.Controls
{
    public class TaskProgressIndicator : GraphicsView
    {
        private readonly TaskProgressIndicatorDrawable _indicatorDrawable;

        public int RepeatedCount
        {
            get { return _indicatorDrawable.repeatedCount; }
            set
            {
                _indicatorDrawable.repeatedCount = value;
                Invalidate();
            }
        }

        public int RepeatCount
        {
            get { return _indicatorDrawable.repeatCount; }
            set
            {
                _indicatorDrawable.repeatCount = value;
                Invalidate();
            }
        }

        public Color IncompletedColor
        {
            get { return _indicatorDrawable.incompletedColor; }
            set
            {
                _indicatorDrawable.incompletedColor = value;
                Invalidate();
            }
        }

        public Color CompletedColor
        {
            get { return _indicatorDrawable.completedColor; }
            set
            {
                _indicatorDrawable.completedColor = value;
                Invalidate();
            }
        }

        public TaskProgressIndicator()
        {
            _indicatorDrawable = new TaskProgressIndicatorDrawable();
            Drawable = _indicatorDrawable;
        }
    }
}
using Daily.Drawables;

namespace Daily.Controls
{
    public class TaskProgressIndicator : GraphicsView
    {
        public static readonly BindableProperty RepeatCountProperty =
            BindableProperty.Create(nameof(RepeatCount), typeof(int), 
                typeof(TaskProgressIndicator), defaultRepeatCount, propertyChanged: OnRepeatCountChanged);

        public static readonly BindableProperty TargetRepeatCountProperty =
            BindableProperty.Create(nameof(TargetRepeatCount), typeof(int), 
                typeof(TaskProgressIndicator), defaultTargetRepeatCount, propertyChanged: OnTargetRepeatCountChanged);

        public static readonly BindableProperty ProgressFillColorProperty =
            BindableProperty.Create(nameof(ProgressFillColor), typeof(Color),
                typeof(TaskProgressIndicator), Colors.White, propertyChanged: OnProgressFillColorChanged);

        public static readonly BindableProperty CompletedFillColorProperty =
            BindableProperty.Create(nameof(CompletedFillColor), typeof(Color),
                typeof(TaskProgressIndicator), Colors.Green, propertyChanged: OnCompletedColorChanged);

        public static readonly BindableProperty StrokeColorProperty =
            BindableProperty.Create(nameof(StrokeColor), typeof(Color),
                typeof(TaskProgressIndicator), Colors.Black, propertyChanged: OnStrokeColorChanged);

        public static readonly BindableProperty StrokeSizeProperty =
            BindableProperty.Create(nameof(StrokeSize), typeof(float),
                typeof(TaskProgressIndicator), 1f, propertyChanged: OnStrokeSizeChanged);

        private const int defaultRepeatCount = 0;
        private const int defaultTargetRepeatCount = 1;

        public int RepeatCount
        {
            get { return (int)GetValue(RepeatCountProperty); }
            set { SetValue(RepeatCountProperty, value); }
        }

        public int TargetRepeatCount
        {
            get { return (int)GetValue(TargetRepeatCountProperty); }
            set { SetValue(TargetRepeatCountProperty, value); }
        }

        public Color ProgressFillColor
        {
            get { return (Color)GetValue(ProgressFillColorProperty); }
            set { SetValue(ProgressFillColorProperty, value); }
        }

        public Color CompletedFillColor
        {
            get { return (Color)GetValue(CompletedFillColorProperty); }
            set { SetValue(CompletedFillColorProperty, value); }
        }

        public Color StrokeColor
        {
            get { return (Color)GetValue(StrokeColorProperty); }
            set { SetValue(StrokeColorProperty, value); }
        }

        public float StrokeSize
        {
            get { return (float)GetValue(StrokeSizeProperty); }
            set { SetValue(StrokeSizeProperty, value); }
        }

        public TaskProgressIndicator()
        {
            Drawable = new RadialProgressBarDrawable(RepeatCount, TargetRepeatCount, StrokeColor, StrokeSize, 
                ProgressFillColor, CompletedFillColor);
        }

        private static void GetGraphicsViewAndDrawable(BindableObject bindable, out GraphicsView graphicsView, out RadialProgressBarDrawable drawable)
        {
            graphicsView = (GraphicsView)bindable;
            drawable = (RadialProgressBarDrawable)graphicsView.Drawable;
        }

        private static void OnRepeatCountChanged(BindableObject bindable, object oldValue, object newValue)
        {
            GetGraphicsViewAndDrawable(bindable, out GraphicsView graphicsView, out var drawable);

            drawable.repeatCount = (int)newValue;
            graphicsView.Invalidate();
        }

        private static void OnTargetRepeatCountChanged(BindableObject bindable, object oldValue, object newValue)
        {
            GetGraphicsViewAndDrawable(bindable, out GraphicsView graphicsView, out var drawable);

            drawable.targetRepeatCount = (int)newValue;

            graphicsView.Invalidate();
        }

        private static void OnProgressFillColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            GetGraphicsViewAndDrawable(bindable, out GraphicsView graphicsView, out var drawable);

            drawable.progressFillColor = (Color)newValue;
            graphicsView.Invalidate();
        }

        private static void OnCompletedColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            GetGraphicsViewAndDrawable(bindable, out GraphicsView graphicsView, out var drawable);

            drawable.completedFillColor = (Color)newValue;
            graphicsView.Invalidate();
        }

        private static void OnStrokeColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            GetGraphicsViewAndDrawable(bindable, out GraphicsView graphicsView, out var drawable);

            drawable.strokeColor = (Color)newValue;
            graphicsView.Invalidate();
        }

        private static void OnStrokeSizeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            GetGraphicsViewAndDrawable(bindable, out GraphicsView graphicsView, out var drawable);

            drawable.strokeSize = (float)newValue;
            graphicsView.Invalidate();
        }
    }
}
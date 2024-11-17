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

        public static readonly BindableProperty BackgroundFillColorProperty =
            BindableProperty.Create(nameof(BackgroundFillColor), typeof(Color),
                typeof(TaskProgressIndicator), Colors.Gray, propertyChanged: OnBackgroundFillColorChanged);

        public static readonly BindableProperty CompletedColorProperty =
            BindableProperty.Create(nameof(CompletedColor), typeof(Color),
                typeof(TaskProgressIndicator), Colors.Green, propertyChanged: OnCompletedColorChanged);

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

        public Color BackgroundFillColor
        {
            get { return (Color)GetValue(BackgroundFillColorProperty); }
            set { SetValue(BackgroundFillColorProperty, value); }
        }

        public Color CompletedColor
        {
            get { return (Color)GetValue(CompletedColorProperty); }
            set { SetValue(CompletedColorProperty, value); }
        }

        public TaskProgressIndicator()
        {
            var drawable = new RadialProgressBarDrawable(RepeatCount, TargetRepeatCount, ProgressFillColor, 
                BackgroundFillColor, CompletedColor);

            Drawable = drawable;
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

        private static void OnBackgroundFillColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            GetGraphicsViewAndDrawable(bindable, out GraphicsView graphicsView, out var drawable);

            drawable.backgroundFillColor = (Color)newValue;
            graphicsView.Invalidate();
        }

        private static void OnCompletedColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            GetGraphicsViewAndDrawable(bindable, out GraphicsView graphicsView, out var drawable);

            drawable.completedColor = (Color)newValue;
            graphicsView.Invalidate();
        }
    }
}
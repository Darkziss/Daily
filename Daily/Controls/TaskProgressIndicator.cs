using Daily.Drawables;

namespace Daily.Controls
{
    public class TaskProgressIndicator : GraphicsView
    {
        public static readonly BindableProperty RepeatCountProperty =
            BindableProperty.Create(nameof(RepeatCount), typeof(int), 
                typeof(TaskProgressIndicator), 0, propertyChanged: OnRepeatCountChanged);

        public static readonly BindableProperty TargetRepeatCountProperty =
            BindableProperty.Create(nameof(TargetRepeatCount), typeof(int), 
                typeof(TaskProgressIndicator), 1, propertyChanged: OnTargetRepeatCountChanged);

        public static readonly BindableProperty IncompletedColorProperty =
            BindableProperty.Create(nameof(IncompletedColor), typeof(Color), 
                typeof(TaskProgressIndicator), Colors.White, propertyChanged: OnIncompletedColorChanged);

        public static readonly BindableProperty CompletedColorProperty =
            BindableProperty.Create(nameof(CompletedColor), typeof(Color),
                typeof(TaskProgressIndicator), Colors.Green, propertyChanged: OnCompletedColorChanged);

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

        public Color IncompletedColor
        {
            get { return (Color)GetValue(IncompletedColorProperty); }
            set { SetValue(IncompletedColorProperty, value); }
        }

        public Color CompletedColor
        {
            get { return (Color)GetValue(CompletedColorProperty); }
            set { SetValue(CompletedColorProperty, value); }
        }

        public TaskProgressIndicator()
        {
            Drawable = new TaskProgressIndicatorDrawable();
        }

        private static void GetThisGraphicsViewAndDrawable(BindableObject bindable, out GraphicsView graphicsView, out TaskProgressIndicatorDrawable drawable)
        {
            graphicsView = (GraphicsView)bindable;
            drawable = (TaskProgressIndicatorDrawable)graphicsView.Drawable;
        }

        private static void OnRepeatCountChanged(BindableObject bindable, object oldValue, object newValue)
        {
            GetThisGraphicsViewAndDrawable(bindable, out GraphicsView graphicsView, out TaskProgressIndicatorDrawable drawable);

            drawable.repeatCount = (int)newValue;
            graphicsView.Invalidate();
        }

        private static void OnTargetRepeatCountChanged(BindableObject bindable, object oldValue, object newValue)
        {
            GetThisGraphicsViewAndDrawable(bindable, out GraphicsView graphicsView, out TaskProgressIndicatorDrawable drawable);

            drawable.targetRepeatCount = (int)newValue;

            graphicsView.Invalidate();
        }

        private static void OnIncompletedColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            GetThisGraphicsViewAndDrawable(bindable, out GraphicsView graphicsView, out TaskProgressIndicatorDrawable drawable);

            drawable.incompletedColor = (Color)newValue;
            graphicsView.Invalidate();
        }

        private static void OnCompletedColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            GetThisGraphicsViewAndDrawable(bindable, out GraphicsView graphicsView, out TaskProgressIndicatorDrawable drawable);

            drawable.completedColor = (Color)newValue;
            graphicsView.Invalidate();
        }
    }
}
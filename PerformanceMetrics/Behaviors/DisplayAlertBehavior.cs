namespace MauiAIAssistViewPerformanceMetrics.Behaviors;

public sealed class DisplayAlertBehavior : Behavior<Button>
{
    public static readonly BindableProperty TitleProperty = BindableProperty.Create(
        nameof(Title),
        typeof(string),
        typeof(DisplayAlertBehavior),
        "Alert");

    public static readonly BindableProperty MessageProperty = BindableProperty.Create(
        nameof(Message),
        typeof(string),
        typeof(DisplayAlertBehavior),
        string.Empty);

    public static readonly BindableProperty CancelProperty = BindableProperty.Create(
        nameof(Cancel),
        typeof(string),
        typeof(DisplayAlertBehavior),
        "OK");

    private bool _isShowing;

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public string Message
    {
        get => (string)GetValue(MessageProperty);
        set => SetValue(MessageProperty, value);
    }

    public string Cancel
    {
        get => (string)GetValue(CancelProperty);
        set => SetValue(CancelProperty, value);
    }

    protected override void OnAttachedTo(Button bindable)
    {
        base.OnAttachedTo(bindable);
        BindingContext = bindable.BindingContext;
        bindable.BindingContextChanged += OnBindingContextChanged;
        bindable.Clicked += OnClicked;
    }

    protected override void OnDetachingFrom(Button bindable)
    {
        bindable.BindingContextChanged -= OnBindingContextChanged;
        bindable.Clicked -= OnClicked;
        BindingContext = null;
        base.OnDetachingFrom(bindable);
    }

    private void OnBindingContextChanged(object? sender, EventArgs e)
    {
        if (sender is Button button)
        {
            BindingContext = button.BindingContext;
        }
    }

    private async void OnClicked(object? sender, EventArgs e)
    {
        if (_isShowing || sender is not Button button)
        {
            return;
        }

        var rootPage = button.Window?.Page;
        var page = rootPage is NavigationPage navigationPage
            ? navigationPage.CurrentPage
            : rootPage;

        if (page is null)
        {
            throw new InvalidOperationException("The active page is unavailable.");
        }

        _isShowing = true;
        try
        {
            await page.DisplayAlertAsync(Title, Message, Cancel);
        }
        finally
        {
            _isShowing = false;
        }
    }
}
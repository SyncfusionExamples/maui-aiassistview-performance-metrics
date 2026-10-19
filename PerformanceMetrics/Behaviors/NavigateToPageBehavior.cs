using System.Windows.Input;

namespace MauiAIAssistViewPerformanceMetrics.Behaviors;

public sealed class NavigateToPageBehavior : Behavior<Button>
{
    public static readonly BindableProperty CommandProperty = BindableProperty.Create(
        nameof(Command),
        typeof(ICommand),
        typeof(NavigateToPageBehavior));

    public static readonly BindableProperty DestinationPageTypeProperty = BindableProperty.Create(
        nameof(DestinationPageType),
        typeof(Type),
        typeof(NavigateToPageBehavior));

    private bool _isNavigating;

    public ICommand? Command
    {
        get => (ICommand?)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public Type? DestinationPageType
    {
        get => (Type?)GetValue(DestinationPageTypeProperty);
        set => SetValue(DestinationPageTypeProperty, value);
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
        if (_isNavigating || sender is not Button button || Command is not { } command || !command.CanExecute(null))
        {
            return;
        }

        var destinationPage = Activator.CreateInstance(DestinationPageType
                ?? throw new InvalidOperationException("A destination page type is required.")) as Page
            ?? throw new InvalidOperationException("The destination page type must be a Page with a public parameterless constructor.");

        destinationPage.BindingContext = button.BindingContext;
        _isNavigating = true;

        try
        {
            command.Execute(null);
            await button.Navigation.PushAsync(destinationPage);
        }
        finally
        {
            _isNavigating = false;
        }
    }
}
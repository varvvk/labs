using System.Windows.Input;

namespace MedicalCenter.Commands
{
    public static class CustomCommands
    {
        public static readonly RoutedUICommand ClearFields = new RoutedUICommand(
            "Очистить поля",
            "ClearFields",
            typeof(CustomCommands),
            new InputGestureCollection
            {
                new KeyGesture(Key.L, ModifierKeys.Control) // Ctrl + L
            });
    }
}

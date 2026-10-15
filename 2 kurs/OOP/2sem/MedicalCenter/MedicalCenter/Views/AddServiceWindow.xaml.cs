using System.Windows;
using MedicalCenter.Commands;
using System.Windows.Input;
using MedicalCenter.ViewModels;

namespace MedicalCenter.Views
{
    public partial class AddServiceWindow : Window
    {
        public AddServiceWindow(MainViewModel mainViewModel)
        {
            InitializeComponent();
            DataContext = new AddServiceViewModel(mainViewModel);

            CommandBindings.Add(new CommandBinding(CustomCommands.ClearFields, ExecuteClearFields, CanExecuteClearFields));
        }

        private void CanExecuteClearFields(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void ExecuteClearFields(object sender, ExecutedRoutedEventArgs e)
        {
            if (DataContext is AddServiceViewModel vm)
            {
                vm.ClearFieldsCommand.Execute(null);
            }
        }
    }

}

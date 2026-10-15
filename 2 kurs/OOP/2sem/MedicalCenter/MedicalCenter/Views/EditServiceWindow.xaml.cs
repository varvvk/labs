using System.Windows;
using MedicalCenter.ViewModels;

namespace MedicalCenter.Views;

public partial class EditServiceWindow : Window
{
    public EditServiceWindow()
    {
        InitializeComponent();
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        if (DataContext is EditServiceViewModel vm)
        {
            vm.SaveChanges();
            DialogResult = true;
            Close();
        }
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }
}

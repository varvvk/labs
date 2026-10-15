using MedicalCenter.Models;
using MedicalCenter.Repositories;
using System;
using System.Windows;
using System.Windows.Input;

namespace MedicalCenter.ViewModels;

public class ServiceDetailViewModel : ViewModelBase
{
    private readonly IServiceRepository _serviceRepository;
    private readonly Action _onUpdateRequested;
    private readonly Action _onDeleteRequested;

    public Service Service { get; }

    public string Title => Service.Title;
    public string Category => Service.Category;
    public string Description => Service.Description;
    public string Doctor => Service.Doctor;
    public decimal Price => Service.Price;

    public ICommand EditCommand { get; }
    public ICommand DeleteCommand { get; }

    public ServiceDetailViewModel(
        Service service,
        IServiceRepository serviceRepository,
        Action onUpdateRequested,
        Action onDeleteRequested)
    {
        Service = service;
        _serviceRepository = serviceRepository;

        _onUpdateRequested = onUpdateRequested;
        _onDeleteRequested = onDeleteRequested;

        EditCommand = new RelayCommand(_ => ExecuteEdit());
        DeleteCommand = new RelayCommand(_ => ExecuteDelete());
    }

    private void ExecuteEdit()
    {
        var editWindow = new Views.EditServiceWindow();
        var editVM = new EditServiceViewModel(Service);
        editWindow.DataContext = editVM;

        if (editWindow.ShowDialog() == true && editVM.UpdatedService != null)
        {
            _serviceRepository.Update(editVM.UpdatedService);
            EventBus.RaiseServicesUpdated(); // вместо _onUpdateRequested
                                             // Не вызываем _onUpdateRequested, чтобы избежать локальных обновлений
        }
    }

    private void ExecuteDelete()
    {
        var result = MessageBox.Show("Вы уверены, что хотите удалить этот сервис?", "Удаление", MessageBoxButton.YesNo);
        if (result == MessageBoxResult.Yes)
        {
            _serviceRepository.Delete(Service.Id);
            EventBus.RaiseServicesUpdated(); // вместо _onDeleteRequested
                                             // Не вызываем _onDeleteRequested, чтобы избежать локальных обновлений
        }
    }

}

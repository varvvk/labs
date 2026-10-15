using MedicalCenter.Models;
using MedicalCenter.Repositories;
using MedicalCenter.ViewModels;
using MedicalCenter.Views;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;

public class MainViewModel : ViewModelBase
{
    private readonly IServiceRepository _serviceRepository;

    private readonly Stack<Service> _undoStack = new();
    private readonly Stack<Service> _redoStack = new();

    public ICommand UndoCommand => new RelayCommand(_ => UndoLastService());
    public ICommand RedoCommand => new RelayCommand(_ => RedoLastUndoneService());



    private string _search;
    
    private string _selectedCategory = "All"; // ключ категории, не локализованный текст

    public string Search
    {
        get => _search;
        set
        {
            if (SetProperty(ref _search, value))
                LoadServicesAsync();
        }
    }

    public string SelectedCategory
    {
        get => _selectedCategory;
        set
        {
            if (SetProperty(ref _selectedCategory, value))
                LoadServicesAsync();
        }
    }

    private string _selectedSortOption = "Name";
    public string SelectedSortOption
    {
        get => _selectedSortOption;
        set
        {
            if (SetProperty(ref _selectedSortOption, value))
                LoadServicesAsync(); // пересчитываем после выбора
        }
    }



    public ObservableCollection<ServiceViewModel> Services { get; } = new();

    public ICollectionView FilteredServices { get; }

    public ICommand OpenAddServiceWindow => new RelayCommand(_ => ExecuteOpenAddServiceWindow());
    public ICommand ShowDetailsCommand { get; }

    private bool _isAdmin;
    public bool IsAdmin
    {
        get => _isAdmin;
        set => SetProperty(ref _isAdmin, value);
    }

    public MainViewModel(IServiceRepository serviceRepository)
    {
        _serviceRepository = serviceRepository;
        ShowDetailsCommand = new RelayCommand(ExecuteShowDetails);

        FilteredServices = CollectionViewSource.GetDefaultView(Services);
        FilteredServices.Filter = FilterService;

        EventBus.ServicesUpdated += async () => await LoadServicesAsync();

        _ = LoadServicesAsync(); // 

    }

    private async Task LoadServicesAsync()
    {
        var services = await _serviceRepository.GetFilteredServicesAsync(Search, SelectedCategory, SelectedSortOption);

        Services.Clear();

        foreach (var service in services)
        {
            Services.Add(new ServiceViewModel(service));
        }
    }

    private bool FilterService(object obj)
    {
        if (obj is not ServiceViewModel service)
            return false;

        bool matchesSearch = string.IsNullOrWhiteSpace(Search) ||
            service.Title.Contains(Search, StringComparison.InvariantCultureIgnoreCase);

        bool matchesCategory =
            string.IsNullOrWhiteSpace(SelectedCategory) ||
            SelectedCategory == "All" ||
            service.Category.Equals(SelectedCategory, StringComparison.OrdinalIgnoreCase);

        return matchesSearch && matchesCategory;
    }


    private void ExecuteOpenAddServiceWindow()
    {
        var addWindow = new AddServiceWindow(this); // ← передаём текущий MainViewModel
        var result = addWindow.ShowDialog();
    }



    public void InitializeUser(object user)
    {
        IsAdmin = user is Admin;
    }

    private void ExecuteShowDetails(object? parameter)
    {
        if (parameter is not ServiceViewModel vm) return;

        var window = new ServiceDetailWindow();

        var detailVM = new ServiceDetailViewModel(
            vm.Service,
            _serviceRepository,
            onUpdateRequested: () =>
            {
                window.DialogResult = true;
                window.Close();
            },
            onDeleteRequested: () =>
            {
                window.DialogResult = true;
                window.Close();
            });

        window.DataContext = detailVM;
        window.ShowDialog();
    }






    public void AddService(Service service)
    {
        _serviceRepository.Add(service);
        _undoStack.Push(service);
        _redoStack.Clear(); // после нового действия сбрасываем Redo

        EventBus.RaiseServicesUpdated();
    }

    private void UndoLastService()
    {
        if (_undoStack.Count == 0) return;

        var service = _undoStack.Pop();
        _serviceRepository.Delete(service.Id);
        _redoStack.Push(service);

        EventBus.RaiseServicesUpdated();
    }

    private void RedoLastUndoneService()
    {
        if (_redoStack.Count == 0) return;

        var service = _redoStack.Pop();

        // Создаем копию услуги с обнулением ID
        var newService = new Service
        {
            Title = service.Title,
            Doctor = service.Doctor,
            Category = service.Category,
            Description = service.Description,
            Price = service.Price
        };

        _serviceRepository.Add(newService);
        _undoStack.Push(newService);

        EventBus.RaiseServicesUpdated();
    }

}

using MedicalCenter.Models;

namespace MedicalCenter.ViewModels;

public class EditServiceViewModel : ViewModelBase
{
    private string _title;
    private string _category;
    private string _description;
    private string _doctor;
    private decimal _price;

    public Service? UpdatedService { get; private set; }

    public string Title
    {
        get => _title;
        set => SetProperty(ref _title, value);
    }

    public string Category
    {
        get => _category;
        set => SetProperty(ref _category, value);
    }

    public string Description
    {
        get => _description;
        set => SetProperty(ref _description, value);
    }

    public string Doctor
    {
        get => _doctor;
        set => SetProperty(ref _doctor, value);
    }

    public decimal Price
    {
        get => _price;
        set => SetProperty(ref _price, value);
    }

    public EditServiceViewModel(Service service)
    {
        // Инициализируем значения из оригинального сервиса
        _title = service.Title;
        _category = service.Category;
        _description = service.Description;
        _doctor = service.Doctor;
        _price = service.Price;

        // Держим оригинальный ID
        UpdatedService = new Service
        {
            Id = service.Id,
            Title = _title,
            Category = _category,
            Description = _description,
            Doctor = _doctor,
            Price = _price
        };
    }

    public void SaveChanges()
    {
        if (UpdatedService != null)
        {
            UpdatedService.Title = Title;
            UpdatedService.Category = Category;
            UpdatedService.Description = Description;
            UpdatedService.Doctor = Doctor;
            UpdatedService.Price = Price;
        }
    }
}

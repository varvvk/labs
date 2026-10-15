using MedicalCenter.Models;

public class ServiceViewModel : ViewModelBase
{
    private Service _service;

    public int Id => _service.Id;
    public string Title => _service.Title;
    public decimal Price => _service.Price;
    public string Doctor => _service.Doctor;
    public string Category => _service.Category;
    public string Description => _service.Description;

    public ServiceViewModel(Service service)
    {
        _service = service;
    }

    // 👉 Это нужно для редактирования
    public Service Service => _service;

    // 👉 Это нужно после редактирования, чтобы обновить значения в ViewModel
    public void UpdateFromModel(Service updated)
    {
        _service = updated;

        // Уведомляем UI о том, что свойства могли измениться
        OnPropertyChanged(nameof(Title));
        OnPropertyChanged(nameof(Price));
        OnPropertyChanged(nameof(Doctor));
        OnPropertyChanged(nameof(Category));
        OnPropertyChanged(nameof(Description));
    }
}

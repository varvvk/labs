using System.Windows;

public static class LanguageManager
{
    private static ResourceDictionary currentDictionary;

    public static event EventHandler LanguageChanged;

    public static void SetLanguage(string languageCode)
    {
        string dictionaryPath;
        if (languageCode == "ru")
        {
            dictionaryPath = "Resources/Russian.xaml";
        }
        else
        {
            dictionaryPath = "Resources/English.xaml";
        }

        currentDictionary = new ResourceDictionary()
        {
            Source = new Uri(dictionaryPath, UriKind.Relative)
        };

        // Обновляем словарь ресурсов приложения
        Application.Current.Resources.MergedDictionaries.Clear();
        Application.Current.Resources.MergedDictionaries.Add(currentDictionary);

        LanguageChanged?.Invoke(null, EventArgs.Empty);
    }
}
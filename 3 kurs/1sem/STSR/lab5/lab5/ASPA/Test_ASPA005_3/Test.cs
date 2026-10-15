//Test.cs
using System;
using System.Net.Http.Json;
using System.Text.Json;
using System.Globalization;

class Test
{
    class Answer<T>
    {
        public T? x { get; set; } = default(T?);
        public T? y { get; set; } = default(T?);
        public string? message { get; set; } = null;
    }

    public static string OK = "OK";
    public static string NOK = "NOK";

    HttpClient client;
    public Test()
    {
        var handler = new HttpClientHandler
        {
            // Отключаем проверку SSL для самоподписанных сертификатов
            ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };
        client = new HttpClient(handler);
    }

    public async Task ExecuteGET<T>(string path, Func<T?, T?, int, string> result)
    {
        await resultPRINT<T>("GET", path, await this.client.GetAsync(path), result);
    }
    public async Task ExecutePOST<T>(string path, Func<T?, T?, int, string> result)
    {
        await resultPRINT<T>("POST", path, await this.client.PostAsync(path, null), result);
    }
    public async Task ExecutePUT<T>(string path, Func<T?, T?, int, string> result)
    {
        await resultPRINT<T>("PUT", path, await this.client.PutAsync(path, null), result);
    }
    public async Task ExecuteDELETE<T>(string path, Func<T?, T?, int, string> result)
    {
        await resultPRINT<T>("DELETE", path, await this.client.DeleteAsync(path), result);
    }

    async Task resultPRINT<T>(string method, string path, HttpResponseMessage rm, Func<T?, T?, int, string> result)
    {
        int status = (int)rm.StatusCode;
        try
        {
            // Настройка JSON сериализатора для использования инвариантной культуры
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString
            };

            Answer<T>? answer = await rm.Content.ReadFromJsonAsync<Answer<T>>(options) ?? default(Answer<T>);
            string r = result(default(T), default(T), status);
            T? x = default(T), y = default(T);
            if (answer != null) r = result(x = answer.x, y = answer.y, status);

            // Форматирование вывода с инвариантной культурой
            string xStr = FormatValue(x);
            string yStr = FormatValue(y);

            Console.WriteLine($" {r}: {method} {path}, status = {status}, x = {xStr}, y = {yStr}, m = {answer?.message}");
        }
        catch (JsonException ex)
        {
            string r = result(default(T), default(T), status);
            Console.WriteLine($" {r}: {method} {path}, status = {status}, x = {null}, y = {null}, m = {ex.Message}");
        }
    }

    // Метод для форматирования значений с инвариантной культурой
    private string FormatValue<T>(T? value)
    {
        if (value == null) return "";

        // Для float/double используем точку как разделитель
        if (value is float f)
            return f.ToString(CultureInfo.InvariantCulture);
        if (value is double d)
            return d.ToString(CultureInfo.InvariantCulture);

        // Для bool используем true/false в нижнем регистре
        if (value is bool b)
            return b.ToString().ToLower();

        // Для DateTime используем формат, похожий на "2025-02-25 0:00:00"
        if (value is DateTime dt)
            return dt.ToString("yyyy-MM-dd HH:mm:ss");

        // Для остальных типов - обычное преобразование
        return value.ToString() ?? "";
    }
}
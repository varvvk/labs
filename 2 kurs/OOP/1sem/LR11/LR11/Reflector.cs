using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace LR11
{
    public static class Reflector
    {
        public static string GetAssemblyName(Type type)
        {
            return type.Assembly.FullName;
        }

        public static bool HasPublicConstructors(Type type)
        {
            return type.GetConstructors(BindingFlags.Instance | BindingFlags.Public).Any();
        }

        public static IEnumerable<string> GetPublicMethods(Type type)
        {
            return type.GetMethods(BindingFlags.Instance | BindingFlags.Public).Select(m => m.Name);
        }

        public static IEnumerable<string> GetFieldAndProperties(Type type)
        {
            var fields = type.GetFields()
                             .Select(f => $"Поле: {f.Name} (Тип: {f.FieldType.Name})");

            var properties = type.GetProperties()
                                 .Select(p => $"Свойство: {p.Name} (Тип: {p.PropertyType.Name})");

            return fields.Concat(properties);
        }

        public static IEnumerable<string> GetInterfaces(Type type)
        {
            return type.GetInterfaces().Select(i => i.Name);
        }

        public static void GetMethodsByParameterType(Type type, Type parameter)
        {
            foreach (MethodInfo method in type.GetMethods())
            {
                foreach (ParameterInfo param in method.GetParameters())
                {
                    if (param.ParameterType == parameter)
                    {
                        Console.WriteLine($"Name of method with {parameter} parameter: {method.Name}");
                    }
                }
            }
        }


        public static object[] GenerateArgs(ParameterInfo[] parameters)
        {
            object[] args = new object[parameters.Length];
            Random random = new Random();

            for (int i = 0; i < parameters.Length; i++)
            {
                Type paramType = parameters[i].ParameterType;
                if (paramType == typeof(int))
                    args[i] = random.Next(1, 100);
                else if (paramType == typeof(double))
                    args[i] = random.NextDouble() * 100;
                else if (paramType == typeof(string))
                    args[i] = $"String_{random.Next(1, 100)}";
                else if (paramType == typeof(bool))
                    args[i] = random.Next(0, 2) == 0; 
                else if (paramType.IsEnum)
                    args[i] = Enum.GetValues(paramType).GetValue(random.Next(0, Enum.GetValues(paramType).Length));
                else
                    args[i] = null;
            }

            return args;
        }

        public static object[] ReadArgsFromFile(string fileName)
        {
            try
            {
                string[] lines = File.ReadAllLines(fileName);
                return lines; 
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при чтении аргументов из файла: {ex.Message}");
                return null;
            }
        }

        public static void Invoke(object obj, string methodName, string argsFileName = null)
        {
            Type type = obj.GetType();
            MethodInfo method = type.GetMethod(methodName);

            if (method == null)
            {
                Console.WriteLine("Метод не найден");
                return;
            }

            ParameterInfo[] paramInfo = method.GetParameters();
            object[] args = null;

            if (!string.IsNullOrEmpty(argsFileName))
            {
                args = ReadArgsFromFile(argsFileName);
                if (args == null)
                {
                    Console.WriteLine("Не удалось получить аргументы из файла.");
                    return;
                }
            }
            else
            {
                args = GenerateArgs(paramInfo);
            }

            // Преобразуем аргументы к типу параметров метода
            object[] parameters = new object[args.Length];
            for (int i = 0; i < args.Length; i++)
            {
                parameters[i] = Convert.ChangeType(args[i], paramInfo[i].ParameterType);
            }

            var result = method.Invoke(obj, parameters);
            Console.WriteLine($"Результат вызова метода {methodName}: {result}");
        }


        public static T Create<T>()
        {
            Type type = typeof(T);

            ConstructorInfo constructor = type.GetConstructors()[0];

            object instance = constructor.Invoke(null); 

            return (T)instance;
        }
    }
}

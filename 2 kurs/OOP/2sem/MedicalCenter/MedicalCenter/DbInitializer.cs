using MedicalCenter.Models;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalCenter
{
    public static class DbInitializer
    {
        public static void Initialize(Context context)
        {
            if (!context.Services.Any())
            {
                var defaultServices = new List<Service>
            {
                new Service
                {
                    Category = "Косметология",
                    Title = "Лазерная эпиляция",
                    Price = 2500,
                    Description = "Безболезненное удаление нежелательных волос с помощью диодного лазера.",
                    Doctor = "Екатерина Лазарева"
                },
                new Service
                {
                    Category = "Косметология",
                    Title = "Контурная пластика губ",
                    Price = 9500,
                    Description = "Придание формы и объема губам с использованием гиалуроновой кислоты.",
                    Doctor = "Марина Волкова"
                },
                new Service
                {
                    Category = "Дерматология",
                    Title = "Лечение акне",
                    Price = 3000,
                    Description = "Комплексная терапия для устранения воспалений и высыпаний.",
                    Doctor = "Андрей Сухов"
                },
                new Service
                {
                    Category = "Дерматология",
                    Title = "Удаление родинок",
                    Price = 4500,
                    Description = "Быстрое и безопасное удаление новообразований кожи.",
                    Doctor = "Наталья Орлова"
                },
                new Service
                {
                    Category = "Неврология",
                    Title = "Консультация невролога",
                    Price = 1800,
                    Description = "Диагностика и лечение неврологических расстройств.",
                    Doctor = "Сергей Власов"
                },
                new Service
                {
                    Category = "Неврология",
                    Title = "ЭЭГ головного мозга",
                    Price = 2200,
                    Description = "Электроэнцефалография для оценки мозговой активности.",
                    Doctor = "Анна Крылова"
                },
                new Service
                {
                    Category = "Офтальмология",
                    Title = "Проверка зрения",
                    Price = 1000,
                    Description = "Полная диагностика остроты зрения и подбор очков.",
                    Doctor = "Дмитрий Павлов"
                },
                new Service
                {
                    Category = "Офтальмология",
                    Title = "Удаление халязиона",
                    Price = 6000,
                    Description = "Миниинвазивное хирургическое удаление воспаления века.",
                    Doctor = "Ольга Фомина"
                },
                new Service
                {
                    Category = "Офтальмология",
                    Title = "Лазерная коррекция зрения",
                    Price = 25000,
                    Description = "Современный способ восстановления зрения без очков и линз.",
                    Doctor = "Игорь Мельников"
                }
                };

                context.Services.AddRange(defaultServices);
            }

            if (!context.Clients.Any())
            {
                context.Clients.Add(new Client
                {
                    Name = "Иван Клиент",
                    Email = "ivan@mail.com",
                    Phone = "123456",
                    Password = "1111"
                });
            }

            if (!context.Admins.Any())
            {
                context.Admins.Add(new Admin
                {
                    Name = "Алексей Админ",
                    Login = "admin",
                    Password = "1234"
                });
            }

            context.SaveChanges();
        }
    }
}

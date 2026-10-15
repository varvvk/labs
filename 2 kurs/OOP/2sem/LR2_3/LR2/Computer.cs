using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace LR2
{
    public class Computer
    {
        [Required(ErrorMessage = "Тип компьютера обязателен.")]
        public string Type { get; set; }

        [Required(ErrorMessage = "Тип оперативной памяти обязателен.")]
        [RegularExpression(@"^(DDR[3-5]|LPDDR4)$", ErrorMessage = "Некорректный тип памяти. Допустимые значения: DDR3, DDR4, DDR5, LPDDR4.")]
        public string RamType { get; set; }

        [Required(ErrorMessage = "Размер оперативной памяти обязателен.")]
        public int RamSize { get; set; }

        [Required(ErrorMessage = "Тип диска обязателен.")]
        public string DiskType { get; set; }

        [Range(256, 1000, ErrorMessage = "Размер диска должен быть от 256 до 1000 ГБ.")]
        public int DiskSize { get; set; }

        [Required(ErrorMessage = "Дата получения обязательна.")]
        [PastDate(ErrorMessage = "Дата получения не может быть в будущем.")]
        public DateTime GetDate { get; set; }

        [Required(ErrorMessage = "Информация о процессоре обязательна.")]
        public Processor Cpu { get; set; }

        [Required(ErrorMessage = "Информация о видеокарте обязательна.")]
        public VideoCard Gpu { get; set; }

        public decimal Price { get; set; }
    }
}

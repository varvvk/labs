using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;


namespace LR13
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var exam = new FinalExam("Final Exam", DateTime.Now, 180);
            var binarySerializer = new BinarySerializer();
            binarySerializer.Serialize(exam, "exam.bin");
            FinalExam deserializedLion = binarySerializer.Deserialize<FinalExam>("exam.bin");
            Console.WriteLine(deserializedLion.Name + ", " + deserializedLion.Date + ", " + deserializedLion.Duration + " - bin");

            var exam2 = new FinalExam("Final Exam №2", DateTime.Now, 90);
            var jsonSerializer = new JsonnSerializer();
            jsonSerializer.Serialize(exam2, "exam.json");
            FinalExam deserializeddLion = jsonSerializer.Deserialize<FinalExam>("exam.json");
            Console.WriteLine(deserializeddLion.Name + ", " + deserializeddLion.Date + ", " + deserializeddLion.Duration + " - json");

            var exam3 = new FinalExam("Final Exam №3", DateTime.Now, 60);
            var xmlSerializer = new XmllSerializer();
            xmlSerializer.Serialize(exam3, "exam.xml");
            FinalExam deserializedddLion = xmlSerializer.Deserialize<FinalExam>("exam.xml");
            Console.WriteLine(deserializedddLion.Name + ", " + deserializedddLion.Date + ", " + deserializedddLion.Duration + " - xml");

            var exam4 = new FinalExam("Final Exam №4", DateTime.Now, 30);
            var soapSerializer = new SoapSerializer();
            soapSerializer.Serialize(exam4, "exam.soap");
            FinalExam deserializeddddLion = soapSerializer.Deserialize<FinalExam>("exam.soap");
            Console.WriteLine(deserializeddddLion.Name + ", " + deserializeddddLion.Date + ", " + deserializeddddLion.Duration + " - soap");

            FinalExam[] exams =
        {
            new FinalExam("Final Exam", DateTime.Now, 180),
            new FinalExam("Final Exam №2", DateTime.Now, 90),
            new FinalExam("Final Exam №3", DateTime.Now, 60),
            new FinalExam("Final Exam №4", DateTime.Now, 30)
        };

            binarySerializer.Serialize(exams, "exams.bin");
            var deserializedBinaryExams = binarySerializer.Deserialize<FinalExam[]>("exams.bin");
            Console.WriteLine("Binary десериализация:");
            foreach (var ex in deserializedBinaryExams)
            {
                Console.WriteLine(exam.Name + ", " + exam.Date + ", " + exam.Duration);
            }

            jsonSerializer.Serialize(exams, "exams.json");
            var deserializedJsonExams = jsonSerializer.Deserialize<FinalExam[]>("exams.json");
            Console.WriteLine("JSON десериализация:");
            foreach (var ex in deserializedJsonExams)
            {
                Console.WriteLine(exam.Name + ", " + exam.Date + ", " + exam.Duration);
            }

            xmlSerializer.Serialize(exams, "exams.xml");
            var deserializedXmlExams = xmlSerializer.Deserialize<FinalExam[]>("exams.xml");
            Console.WriteLine("XML десериализация:");
            foreach (var ex in deserializedXmlExams)
            {
                Console.WriteLine(exam.Name + ", " + exam.Date + ", " + exam.Duration);
            }

            soapSerializer.Serialize(exams, "exams.soap");
            var deserializedSoapExams = soapSerializer.Deserialize<FinalExam[]>("exams.soap");
            Console.WriteLine("Soap десериализация:");
            foreach (var ex in deserializedSoapExams)
            {
                Console.WriteLine(exam.Name + ", " + exam.Date + ", " + exam.Duration);
            }

            Console.WriteLine("\nРабота с XML через XPath:");
            XmlDocument doc = new XmlDocument();
            doc.Load("exams.xml");

            XmlNode nameNode = doc.SelectSingleNode("/FinalExam/Name");
            if (nameNode != null)
            {
                Console.WriteLine($"Название экзамена: {nameNode.InnerText}");
            }
            else
            {
                Console.WriteLine("Название экзамена не найдено в XML.");
            }

            XmlNode durationNode = doc.SelectSingleNode("/FinalExam/Duration");
            if (durationNode != null)
            {
                Console.WriteLine($"Продолжительность экзамена: {durationNode.InnerText}");
            }
            else
            {
                Console.WriteLine("Продолжительность экзамена не найдена в XML.");
            }

            Console.WriteLine("\nРабота с XML через LINQ to XML:");
            var linqDoc = XDocument.Load("exams.xml");

            var name = linqDoc.Descendants("FinalExam").Select(e => e.Element("Name")?.Value).FirstOrDefault();
            Console.WriteLine($"Название экзамена: {name}");

            var duration = linqDoc.Descendants("FinalExam").Select(e => e.Element("Duration")?.Value).FirstOrDefault();
            Console.WriteLine($"Продолжительность экзамена: {duration}");

            Console.WriteLine("\nПрограмма завершена.");
        }
    }

}

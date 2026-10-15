using System;
using System.Collections.Generic;
using System.IO;
#nullable enable

namespace LR5.Lab5Lib
{
    public class FileWriter : IWriter
    {
        private string _filename;
        public string FileName
        {
            get => _filename;
        }

        public FileWriter(string? filename = null)
        {
            this._filename = filename ?? Constant.FileName;
        }

        public string? Save(string? message)
        {
            File.WriteAllText(this._filename, message);
            return this._filename;
        }
    }
}

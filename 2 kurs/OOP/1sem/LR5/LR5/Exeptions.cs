using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR5
{
    public class FinalExamsException : Exception
    {
        public FinalExamsException(string message) : base(message) { }
    }

    public class TrialNotFoundException : ApplicationException
    {
        public TrialNotFoundException(string message) : base(message) { }
    }

    public class DuplicateTrialException : Exception
    {
        public DuplicateTrialException(string message) : base(message) { }
    }

    public class EmptyTrialException : Exception
    {
        public EmptyTrialException(string message) : base(message) { }
    }
}

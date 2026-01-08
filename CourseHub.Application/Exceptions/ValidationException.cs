using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseHub.Application.Exceptions
{

    public class ValidationException : ApplicationExceptionBase
    {
        public ValidationException(string message) : base(message) { }
    }
}

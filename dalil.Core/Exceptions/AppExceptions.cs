using System;
using System.Collections.Generic;
using System.Text;

namespace dalil.Core.Exceptions
{
    public class AppExceptions
    {
        // Core/Exceptions
        public class NotFoundException(string message) : Exception(message);
        public class ConflictException(string message) : Exception(message);
        public class UnauthorizedException(string message) : Exception(message);
        public class InvalidFileException(string message) : Exception(message);
    }
}

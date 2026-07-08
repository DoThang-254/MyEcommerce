using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Domain.Exceptions
{
    public class NotFoundException : DomainException
    {
        protected NotFoundException(string message)
        : base(message, "Not Found") // Truyền title "Not Found" cho base
        {
        }

        protected NotFoundException(string name, object key)
            : base($"Entity \"{name}\" ({key}) was not found.", "Not Found")
        {
        }
    }
}

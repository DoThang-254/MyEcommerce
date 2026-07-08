using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Domain.Exceptions
{
    public abstract class DomainException : Exception
    {
        protected DomainException(string message) : base(message)
        {
        }

        protected DomainException(string message, string title) : base(message)
        {
            Title = title;
        }

        public string? Title { get; }
    }
}

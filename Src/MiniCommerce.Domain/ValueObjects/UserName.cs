using System;

namespace MiniCommerce.Domain.ValueObjects
{
    public class UserName
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }

        private UserName() { } 

        public UserName(string firstName, string lastName)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentException("First name cannot be empty", nameof(firstName));
            
            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentException("Last name cannot be empty", nameof(lastName));

            FirstName = firstName;
            LastName = lastName;
        }

        public string FullName => $"{FirstName} {LastName}";

        public override string ToString()
        {
            return FullName;
        }
    }
}

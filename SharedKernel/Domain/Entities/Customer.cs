﻿namespace SharedKernel.Domain.Entities
{
    public class Customer : IntBaseEntity
    {
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string FirstName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
    }
}

﻿namespace YourRest.Domain.Entities
{
    public abstract class BaseEntity<T> where T : notnull
    {
        public T Id { get; set; }
    }
}

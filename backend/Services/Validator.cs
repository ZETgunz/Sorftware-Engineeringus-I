using backend.Models;
using backend.Interfaces;
using backend.Exceptions;
using System;
using System.Collections.Generic;

namespace backend.Services
{
    public class Validator<T> : IValidator<T>
        where T : class, IValidatable
    {
        private readonly List<ValidationRule<T>> _rules = new();

        public void AddRule(Func<T, bool> rule, string errorMessage)
        {
            _rules.Add(new ValidationRule<T>(rule, errorMessage));
        }

        public bool Validate(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (!entity.IsValid())
                throw new InvalidOperationException($"{typeof(T).Name} is not valid");

            foreach (var rule in _rules)
            {
                if (!rule.Validate(entity))
                    throw new InvalidCredentialsException(rule.ErrorMessage);
            }

            return true;
        }
    }
}
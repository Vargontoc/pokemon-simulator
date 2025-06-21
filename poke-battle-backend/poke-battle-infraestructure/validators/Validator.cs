using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace poke_battle_infraestructure.validators
{
    public abstract class Validator<T>
    {
        public Validator() { }

        private readonly List<string> _errors = new();

        protected void Require(string? value, string field)
        {
            if (string.IsNullOrEmpty(value))
                _errors.Add($"El campo '{field}' es obligatorio");
        }

        protected void MustMatch(string? value, string pattern, string field, string? message)
        {
            if (!string.IsNullOrEmpty(value) && !Regex.IsMatch(value, pattern))
                    _errors.Add(message ?? $"El campo '{field}' no tiene un formato valido");
        }

        protected void RequireInRange(int value, int min, int max, string field)
        {
            if (min > max)
                throw new InvalidOperationException("Bad range");
            if (value < min || value > max)
                _errors.Add($"El campo '{field}' debe estar entr {min} y {max}");
        }

        protected void ValidateNow()
        {
            if (_errors.Count > 0)
                throw new ValidationException(_errors);
        }

        public abstract void Validate(T entity);
    }
}

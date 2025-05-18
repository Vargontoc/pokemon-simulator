using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace poke_battle_infraestructure.validators
{
    public class Validator
    {
        public Validator() { }

        private readonly List<string> _errors = new();
        public void Check(bool condition, string message)
        {
            if (condition)
            {
                _errors.Add(message);
            }
        }

        public void ThrowIfInvalid()
        {
            if (_errors.Count > 0)
                throw new ValidationException(_errors);
        }
    }
}

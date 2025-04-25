using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace poke_battle_infraestructure.validators
{
    public class ValidationException : Exception
    {
        public List<string> Errors { get; }
        public ValidationException(IEnumerable<string> errors):base("Errores de validación") { Errors = errors.ToList(); }

    }
}

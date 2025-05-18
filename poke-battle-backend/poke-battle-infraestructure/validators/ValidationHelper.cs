using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace poke_battle_infraestructure.validators
{
    public class ValidationHelper
    {
        public static void Validate(Action<Validator> validatorAction)
        {
            var validator = new Validator();
            validatorAction(validator);
            validator.ThrowIfInvalid();
        }
    }

    
}

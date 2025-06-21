using poke.battle.Models.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace poke_battle_infraestructure.validators.impl
{
    public class TypeValidator : Validator<TypeModel>
    {
        public override void Validate(TypeModel entity)
        {
            Require(entity.Name, "Nombre Interno");
            MustMatch(entity.Name, "^[a-z\\-]+$", "Nombre Interno", "Solo se adminten letras minúsculas y guiones");

            Require(entity.DisplayName, "Nombre visible");


            ValidateNow();
        }
    }
}

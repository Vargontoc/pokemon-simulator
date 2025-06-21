using poke.battle.Models.Impl;

namespace poke_battle_infraestructure.validators.impl
{
    public class AbilityValidator : Validator<AbilityModel>
    {
        public override void Validate(AbilityModel entity)
        {

            Require(entity.Name, "Nombre Interno");
            MustMatch(entity.Name, "^[a-z\\-]+$", "Nombre Interno", "Solo se adminten letras minúsculas y guiones");

            Require(entity.DisplayName, "Nombre visible");


            ValidateNow();
        }
    }
}

using poke.battle.Models.Impl;
using poke.battle.services;
using poke_battle_api.dtos;
using poke_battle_api.utils;

namespace poke_battle_api.mappers.impl
{
    public class TypeMapper : IMapper<TypeDto, TypeModel>
    {
        private readonly IEnumerable<TypeModel> _types;

        public TypeMapper(ITypeService service)
        {
            _types = service.FindAll(null!, null!).Results;
        }

        public TypeModel convertToBack(TypeDto model)
        {
            return new()
            {
                Id = model.Id,
                Name = model.InternalName,
                DisplayName = model.Name,
                Weakness = model.Weakness.GetKeysSelected().ToArray(),
                Resistences = model.Resistences.GetKeysSelected().ToArray(),
                Inmunities = model.Inmunities.GetKeysSelected().ToArray(),
                Icon = model.Icon
            };
        }

        public IEnumerable<TypeModel> convertToBack(IEnumerable<TypeDto> list) => list.Select(x => convertToBack(x));

        public TypeDto convertToFront(TypeModel model)
        {
            return new()
            {
                Id = model.Id,
                Name = model.DisplayName,
                InternalName = model.Name,
                Resistences = CreateCombo(model.Resistences),
                Weakness = CreateCombo(model.Weakness),
                Inmunities = CreateCombo(model.Inmunities),
                Icon = model.Icon
            };
        }

        private Combo CreateCombo(string[] typesBack)
        {
            return new()
            {
                Items = _types.Where(x => typesBack.Contains(x.Name)).Select(x => new ComboItem() { Key = x.Name, Value = x.DisplayName, Icon = x.Icon, Checked = typesBack.Contains(x.Name) }).ToList(),
            };
        }


        public IEnumerable<TypeDto> convertToFront(IEnumerable<TypeModel> list) => list.Select(x => convertToFront(x));
    }
}

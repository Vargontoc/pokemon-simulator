using poke.battle.core;
using poke.battle.Models.Impl;
using poke.battle.services;
using poke.battle.services.impl;
using poke_battle_api.dtos;

namespace poke_battle_api.mappers.impl
{
    public static class PBattlerMapper
    {
        public static PBattlerDto ConvertTo(PBattler battler)
        {
            IMapper<TypeDto, TypeModel> mapper = new TypeMapper(new TypesService(new HttpClient()));
            return new()
            {
                Nickname = battler.Nickname,
                CurrentHP = battler.CurrentHp,
                Level = battler.Level,
                Moves = battler.Moves.Select(x => x.Move.Name).ToArray(),
                Types = mapper.convertToFront(battler.Types).ToArray(),
                Stats = convertStats(battler.Stats)
            };
        }

        private static object[] convertStats(StatEntry[] stats)
        {
            return stats.Select(x => new { 
                type  = x.Stat.Code,
                rawValue = x.RawValue,
                iv = x.IV,
                ev = x.EV
            }).ToArray();
        }
    }
}

using poke.battle.core;

namespace poke.battle.bridge
{
    public interface IBattleContextFormatter
    {
        string FormatAsText(Battle battle);
    }
}

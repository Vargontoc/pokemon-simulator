using poke.battle.core;
using System.Text;

namespace poke.battle.bridge
{
    public class SimpleBattleContextFormatter : IBattleContextFormatter
    {


        public string FormatAsText(Battle battle)
        {
            var context = battle.Context;
            var sb = new StringBuilder();
            sb.AppendLine($"Turn: {context.Turn}");
            sb.AppendLine();
            sb.AppendLine("Tu equipo: " + string.Join(",", context.Enemy.Where(x => !x.IsFainted && !x.IsFainted)));
            sb.AppendLine("Tu Pokémon actual: ");
            sb.AppendLine(context.Enemy.First(x => x.IsActive && !x.IsEgg && !x.IsFainted).GetBattlerInfo());
            sb.AppendLine();
            sb.AppendLine("Equipo jugador: " + string.Join(",", context.Player.Where(x => !x.IsFainted && !x.IsFainted)));
            sb.AppendLine("Pokemon jugador actual: ");
            sb.AppendLine(context.Player.First(x => x.IsActive && !x.IsEgg && !x.IsFainted).GetBattlerInfo());
            sb.AppendLine();
            sb.AppendLine("El jugador va a realizar la siguiente accion: ");
            sb.AppendLine(ParseAction(battle.Actions));
            sb.AppendLine();
            sb.AppendLine("¿Qué deberias hacer?");
            return sb.ToString();
        }

        private static string ParseAction(List<PlayerAction> actions)
        {
            return actions.Count switch
            {
                0 => "No hay acciones pendientes.",
                1 => actions[0] switch
                {
                    MoveAction moveAction => $"{moveAction.Actor.Name} va a atacar con {moveAction.Move.Move.Name}.",
                    SwtichAction switchAction => $"{switchAction.Actor.Name} va a cambiar a {switchAction.Target.Name}.",
                    _ => "Acción desconocida."
                },
                _ => string.Join("\n", actions.Select(action =>
                    action switch
                    {
                        MoveAction moveAction => $"{moveAction.Actor.Name} va a atacar con {moveAction.Move.Move.Name}.",
                        SwtichAction switchAction => $"{switchAction.Actor.Name} va a cambiar a {switchAction.Target.Name}.",
                        _ => "Acción desconocida."
                    }))
            };
        }
    }
}

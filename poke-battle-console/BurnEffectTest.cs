using poke.battle.core;
using poke.battle.factories;
using poke_battle_services.factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace poke_battle_console
{
    public class BurnEffectTest
    {
        public static void Run(BattlerFactory factory)
        {
            Console.WriteLine("=== Test: Burn effect on physical damage ===");
            
            var move = factory.CreateMove("tackle");
            var attacker = factory.CreateBattler("charmander", 50, null, 1, 1);
            attacker.SetMoves([move]);

            var defender = factory.CreateBattler("charmander", 50, null, 1, 1);

            var select = attacker.Moves[0];

            var context = new BattleContext(player: [attacker], rival: [defender]);

            var normal = DamageCalculator.Calculate(attacker, defender, move.Move, context, out var effect, out var critic);
            Console.WriteLine($"Daño sin quemadura: {normal}");

            attacker.ApplyStatus(PStatus.Burn, new ActionResult());
            var burned = DamageCalculator.Calculate(attacker, defender, move.Move, context, out var effect2, out var critic2);
            Console.WriteLine($"Daño sin quemadura: {burned}");

            Console.WriteLine("=====================================");

            Console.WriteLine(normal > burned ? 
                "El daño con quemadura se redujo correctamente": "El daño con quemadura no fue menor. Revisa el cálculo");




        }
    }
}

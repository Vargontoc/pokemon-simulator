using poke.battle.core;
using poke.battle.Models.helpers;

namespace poke_battle_tests.Mechanics
{
    public class TransformTest
    {
        [Fact]
        public void Transform_ShouldTransformToTarget()
        {
            var ditto = Utils.GetBattler("ditto", 50);
            var target = Utils.GetBattler("pikachu", 50);



            var transform = Utils.GetMove("transform");
            transform.Move.MoveEffect = MoveEffect.Transform;

            var context = new BattleContext([ditto], [target]);
            var action = new MoveAction()
            {
                Actor = ditto,
                Move = transform,
                Context = context
            };

            var result = action.Execute(target);

            Assert.True(ditto.HasTransformed);
            Assert.Equal(target.Specie.Name, ditto.Specie.Name);
            Assert.Equal(target.Types, ditto.Types);
            Assert.Equal(target.Moves.Length, ditto.Moves.Length);

            foreach(var move in ditto.Moves)
                Assert.Contains(ditto.Moves, m => m.Move.Name == move.Move.Name);

            Assert.Contains(result.Events, e => e.Type == "transform");

        }
    }
}

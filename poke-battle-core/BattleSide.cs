using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace poke_battle_core
{
    public class BattleSide
    {
        public List<FieldEffect> Effects { get; set; } = [];
        public bool HasEffect(string effectname) => Effects.Any(f => f.Name == effectname && f.TurnLeft > 0);
        public void ApplyEffect(string effect, int duration)
        {
            var existing = Effects.FirstOrDefault(f => f.Name == effect);
            if(existing != null)
                existing.TurnLeft = duration;
            else
                Effects.Add(new FieldEffect() { TurnLeft = duration, Name = effect });
        }

        public void TickEffects()
        {
            foreach(var f in Effects)
            {
                if(f.TurnLeft > 0)
                    f.TurnLeft--;
            }
        }
    }
}

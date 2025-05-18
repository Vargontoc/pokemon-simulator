using Newtonsoft.Json;
using poke.battle.infraestructure.filters;
using poke.battle.Models;

namespace poke.battle.infraestructure.repositories.impl
{
    public class MovesRepository : GenericRepository<MoveModel, MovesFilter>, IMoveRepository
    {
        public MovesRepository() : base()
        {
            STORED_FILE = "moves.json";
        }


        private MoveModel Map(dynamic data, int count)
        {
            return new MoveModel 
            {
                Id = count,
                Name = data.name,
                Type = data.type.name,
                Power = data.power != null ? (int)data.power : 0,
                Accuracy = data.accuracy != null ? (int)data.accuracy : 0,
                Priority = (int)data.priority,
                MoveType = MapMoveType((string)data.damage_class.name),
                Target = MapTarget((string)data.target.name),
                PP = data.pp != null ? (int)data.pp : 0
            };
        }

        private MoveType MapMoveType(string v) 
        {
            return v switch {
                "physical" => MoveType.Physical,
                "special" => MoveType.Special,
                "status" => MoveType.Status,
                _ => MoveType.Physical
            };
        }
        private TargetType MapTarget(string v)
        {
            return v switch {
                "selected-pokemon" => TargetType.Enemy,
                "all-opponents" => TargetType.AllEnemies,
                "user" => TargetType.Self,
                "entire-field" => TargetType.All,
                _ => TargetType.Enemy
            };
        }

        protected override void CreatePredicates(MovesFilter filter)
        {
            // Empty Method
        }

        public void SaveAll(List<MoveModel> models)
        {
            Save(models);
        }
    }
}
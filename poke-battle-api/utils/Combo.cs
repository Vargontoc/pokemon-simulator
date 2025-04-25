namespace poke_battle_api.utils
{
    public class Combo
    {
        public List<ComboItem> Items { get; set; }

        public List<string> GetKeysSelected() => Items.Where(x => x.Checked).Select(x => x.Key).ToList();

       
    }
}

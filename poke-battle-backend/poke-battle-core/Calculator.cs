using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Net;
using poke.battle.Models.Impl;

namespace poke.battle.core 
{
    public static class Calculator
    {
        #region  Stats
        public static void CalculateStat(StatEntry stat, int level, Nature nature) {
            if (stat.Stat.Equals(Stat.Hp))
            {
                stat.RawValue = CalculateHp(stat.BaseValue, stat.IV, stat.EV, level);
            } else
            {
                stat.RawValue = CalculateStat(stat.BaseValue, stat.IV, stat.EV, level, nature.GetModifier(stat.Stat));
            }

        }

        private static int CalculateHp(int value, int iv, int ev, int level) {
            return (int)(((2 * value + iv + (ev / 4)) * level) / 100.0) + level + 10;
        }

        private static int CalculateStat(int value, int iv, int ev, int level, double natureMod)
        {
            double stat = (((2 * value + iv + (ev / 4)) * level) / 100.0) + 5;
            return (int)(stat * natureMod);
        }

        public static double GetModMultiplier(int mod) {
            return mod switch {
                >= 0 => (2.0 + mod) / 2.0,
                < 0 => 2.0 / (2.0 - mod),
            };
        }

        #endregion

        #region  Experience

        private static int Fast(int n) => (int)(4 * Math.Pow(n, 3) / 5);
        private static int Medium(int n) => (int)Math.Pow(n, 3);
        private static int Slow(int n) => (int)(5 * Math.Pow(n, 3) / 4);
        private static int Parabolic(int n) => (int)(1.2 * Math.Pow(n, 3) - 15 * Math.Pow(n, 2) + 100 * n - 140);
        private static int Erratic(int n) {
            if (n <= 50) return (int)(Math.Pow(n, 3) * (100 - n) / 50);
            if (n <= 68) return (int)(Math.Pow(n, 3) * (150 - n) / 100);
            if (n <= 98) return (int)(Math.Pow(n, 3) * ((1911 - 10 * n) / 3) / 500);
            return (int)(Math.Pow(n, 3) * (160 - n) / 100);
        }

        private static int Fluctuating(int n) {
            if (n <= 15) return (int)(Math.Pow(n, 3) * (((n + 1.0) / 3 + 24) / 50));
            if (n <= 36) return (int)(Math.Pow(n, 3) * ((n + 14) / 50.0));
            return (int)(Math.Pow(n, 3) * ((n / 2.0 + 32) / 50));
        }

        private static readonly Dictionary<Growth, Func<int, int>> _formulas = new() {
            [Growth.Fast] = Fast,
            [Growth.Medium] = Medium,
            [Growth.Slow] = Slow,
            [Growth.Erratic] = Erratic,
            [Growth.Parabolic] = Parabolic,
            [Growth.Fluctuating] = Fluctuating
        };

        private static readonly Dictionary<Growth, Dictionary<int, int>> _tables = new();
        private static int GetExperience(Growth growth, int level, int levelCap = 100)
        {
            if (level < 1 || level > levelCap)
                throw new ArgumentOutOfRangeException(nameof(level));

            var baseExp = _formulas[growth](level);
            if (levelCap == 100)
                return baseExp;

            var max = GetExperience(growth, 100);
            var scale = (double)GetExperience(growth, levelCap) / baseExp;
            return (int)(baseExp * scale);
        }

        private static Dictionary<int, int> PreloadExperience(Growth growth, int levelCap = 100)
        {
            var table = new Dictionary<int, int>(levelCap);
            for (int n = 1; n <= levelCap; n++)
                table[n] = GetExperience(growth, n, levelCap);
            return table;
        }

        public static void InitializeTables(int levelCap = 100) {
            foreach (var g in Growth.GetValues())
                _tables[g] = PreloadExperience(g, levelCap);
        }

        public static int GetExp(Growth growth, int level) {
            if (!_tables.TryGetValue(growth, out var table))
                throw new InvalidOperationException($"Tabla de experiencia para '{growth.Name}' no inicializada");
            return table.TryGetValue(level, out var exp) ? exp : throw new ArgumentOutOfRangeException(nameof(level), $"Nivel no valido: {level}");
        }

        public static int[] GetExperienceValues(Growth g)
        {
            if (!_tables.TryGetValue(g, out var table))
                throw new InvalidOperationException($"Tabla de experiencia para '{g.Name}' no inicializada");
            return [.. table.Values.OrderBy(x => x)];
        }

        public static int CalculateExpGainer(int level, int expBase, int participants = 1, bool luckyEgg = false, bool trainer = false)
        {
            if (level < 0 || level > CoreSettings.MAX_LEVEL || expBase < 0)
                return 0;

            double trainerMultiplier = trainer ? 1.5 : 1.0;
            double baseExp = (expBase * trainerMultiplier * level) / (7.0 * participants);

            if (luckyEgg) baseExp *= 1.5;
            return (int)Math.Floor(baseExp);
            
        }

        #endregion 
    }
}
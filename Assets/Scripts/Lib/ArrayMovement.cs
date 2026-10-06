using System.Linq;
using System;

namespace Lib
{
    public static class ArrayMovement
    {
        /// <summary>
        /// Enum représentant les types de comparaisons possibles.
        /// </summary>
        public enum ComparaisonType
        {
            LowerThan = -2,
            LessThanOrEqualTo = -1,
            EqualTo = 0,
            GreaterThanOrEqualTo = 1,
            GreaterThan = 2
        }

        /// <summary>
        /// Vérifie si une valeur dépasse une limite maximale ou minimale et la réinitialise en conséquence.
        /// </summary>
        /// 
        /// <param name="value">La valeur à vérifier.</param>
        /// <param name="max">La valeur maximale permise (ou fin du tableau).</param>
        /// <param name="min">La valeur minimale permise (ou début du tableau). <c>0</c> par défaut.</param>
        /// <param name="comparaison">La règle de comparaison. <c>ComparaisonType.GreaterThanOrEqualTo</c> par défaut.</param>
        /// <param name="reverse">Détermine si la vérification doit être effectuée à l'envers (vérification contre min au lieu de max). <c>false</c> par défaut.</param>
        /// 
        /// <returns><paramref name="value"/> si elle ne dépasse pas l'une des limites <paramref name="max"/> ou <paramref name="min"/>, sinon retourne la limite apropriée.</returns>
        public static int CheckForResetLoop(int value, int max, int min = 0, ComparaisonType comparaison = ComparaisonType.GreaterThanOrEqualTo, bool reverse = false)
        {
            //Debug.Log($"value: {value}, max: {max}");
            if (new[] { -2, -1 }.Contains((int)comparaison) ^ reverse)
            {
                throw new Exception($"Invalid comparaison rule for the given direction (value).\n" +
                    $"Value: {value}    ComparaisonType: {comparaison}    Reverse: {reverse}");
            }

            //if (!(new[] { -2, -1 }.Contains((int)comparaison) && reverse))
            //{
            //    //throw new Exception("Invalid comparaison rule for the given direction.");
            //    Debug.LogWarning("Mismatch in comparaison rule and direction (value). defaults to value\'s direction");
            //    reverse = (value < 0);
            //    comparaison = (ComparaisonType)(-(int)comparaison);
            //}

            if ((int)comparaison == 0 && value == max) return min;
            if ((int)comparaison == 0 && value == min) return max;

            if (!reverse)
            {
                if ((int)comparaison == 2 && value > max ||
                    (int)comparaison == 1 && value >= max)
                {
                    //Debug.Log($"reset from {value} to {min}");
                    return min;
                }
            }
            else
            {
                if ((int)comparaison == -1 && value <= min ||
                    (int)comparaison == -2 && value < min)
                {
                    //Debug.Log($"reset from {value} to {min}");
                    return max;
                }
            }
            //Debug.Log($"no reset, value stays {value}");
            return value;
        }

        /// <summary>
        /// Itère la méthode <c>CheckForLoopback()</c> pour un nombre d'itérations.
        /// </summary>
        /// 
        /// <param name="baseValue">La valeur de base avant de commencer l'itération.</param>
        /// <param name="max">La valeur maximale permise (ou fin du tableau).</param>
        /// <param name="iterations">Le nombre d'itérations à faire.</param>
        /// <param name="min">La valeur minimale permise (ou début du tableau). <c>0</c> par défaut.</param>
        /// <param name="comparaison">La règle de comparaison. <c>ComparaisonType.GreaterThan</c> par défaut.</param>
        /// <param name="reverse">Détermine si la vérification doit être effectuée à l'envers (vérification contre min au lieu de max). <c>false</c> par défaut.</param>
        /// 
        /// <returns>Sortie de <c>CheckForLoopback()</c> après <paramref name="iterations"/> itérations.</returns>
        public static int CheckForLoopback(int baseValue, int max, int iterations, int min = 0, ComparaisonType comparaison = ComparaisonType.GreaterThan, bool reverse = false)
        {
            for(int i = iterations; i > 0; i--)
            {
                baseValue += reverse ? -1 : 1;
                baseValue = CheckForResetLoop(baseValue, max, min, comparaison, reverse);
            }

            return baseValue;
            //return CheckForResetLoop(baseValue, max, min, comparaison, reverse);
        }
    }
}

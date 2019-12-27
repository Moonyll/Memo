using System;
using System.ComponentModel.DataAnnotations; // Espace Nom pour accéder aux attributs.

namespace Memo.Models
{
    public class Operation
    {
        [RegularExpression(Pattern, ErrorMessage = Error)]
        public float x { get; set; } // 1er nombre
        [RegularExpression(Pattern, ErrorMessage = Error)]
        public float y { get; set; } // 2nd nombre
        private const string Pattern = @"^\d*,?\d+$";
        public const string Error = "input should be a number";
        public string op { get; set;} // Opérateur
        public string result { get; set; } // Résultat
        public string Calculate() // Méthode de calcul
        {
            return result =
                (op == "+") ? (x + y).ToString() : // Addition
                (op == "-") ? (x - y).ToString() : // Soustraction
                (op == "*") ? (x * y).ToString() : // Multiplication
                (op == "/" && y != 0) ? (x / y).ToString() : " Div by 0..."; // Division
        }
    }
    public class Number
    {
        [Range(0,999, ErrorMessage = "Invalid number, please enter a 3 digit one...")]
        public int? a { get; set; }
        public int? b { get; set; }
        public int? c { get; set; }

    }
    public class Picture
    {
        public int pictureId { get; set; }
        public string pictureTitle { get; set; }
        public string pictureDescritption { get; set; }
        public string pictureLocationUrl { get; set; }
        public int pictureViewsNumber { get; set; }
        public int pictureRatingValue { get; set; }


    }
}
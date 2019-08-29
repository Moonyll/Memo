using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Memo.Models
{
    public class Operation
    {
        public float x { get; set; } // 1er nombre
        public float y { get; set; } // 2nd nombre
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
}
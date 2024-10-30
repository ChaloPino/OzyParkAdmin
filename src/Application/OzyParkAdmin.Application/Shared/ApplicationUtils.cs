using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OzyParkAdmin.Application.Shared;
/// <summary>
/// Utilizataris de la capa Aplicacion
/// </summary>
public static class ApplicationUtils
{
    /// <summary>
    /// Generates a random string with the given length
    /// </summary>
    /// <param name="size">Size of the string</param>
    /// <param name="lowerCase">If true, generate lowercase string</param>
    /// <returns>Random string</returns>
    public static string RandomString(int size, bool lowerCase)
    {
        StringBuilder builder = new StringBuilder();
        Random random = new Random();
        char ch;
        int azar;
        for (int i = 0; i < size; i++)
        {
            azar = Convert.ToInt32(Math.Floor(42 * random.NextDouble() + 48));
            if (azar >= 65 || azar <= 57) //fuera entre 58 y 64
            {
                ch = Convert.ToChar(azar);
                builder.Append(ch);
            }
        }
        if (lowerCase)
            return builder.ToString().ToLower();
        return builder.ToString();
    }
}

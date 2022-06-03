using System;

namespace Simple1C77
{
    internal class Program
    {
        static void Main(string[] args)
        {
            foreach(var pair in Const.ReservedTokens )
            {
                Console.WriteLine("{0}: {1}",pair.Key, pair.Value);
            }

            //Console.ReadLine();
        }
    }
}

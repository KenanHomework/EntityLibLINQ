using ConsoleUI.ConsoleTable.Services;
using System;
using System.Collections.Generic;

namespace ConsoleUI.ConsoleTable.Structural
{
    public class Row
    {
        public Row(List<string> values)
        {
            Values = values;
        }

        public List<Column> Columns { get; set; }

        public ConsoleColor DefColor { get; set; } = ConsoleColor.White;

        public List<string> Values { get; set; }

        public void Write()
        {
            for (int i = 0; i < Values.Count; i++)
            {
                if (Columns[i].IsStatColumn)
                    ConsoleWriteService.WriteStat(float.Parse(Values[i]), Columns[i].AverageValue, Columns[i].MaxLenght);
                else
                    ConsoleWriteService.Write(Values[i], Columns[i].MaxLenght, DefColor);
            }
            Console.WriteLine();
        }
    }
}

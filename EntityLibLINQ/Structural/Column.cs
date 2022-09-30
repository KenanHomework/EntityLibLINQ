using ConsoleUI.ConsoleTable.Services;
using System;

namespace ConsoleUI.ConsoleTable.Structural
{
    public class Column
    {

        public string Name { get; set; }

        public ConsoleColor Color { get; set; } = ConsoleColor.White;

        public int MaxLenght { get; set; } = 20;

        public float AverageValue { get; set; } = 0;

        public bool IsStatColumn { get; set; } = false;

        public Column(string name)
        {
            Name = name;
        }

        public Column(string name, int maxLenght, ConsoleColor color = ConsoleColor.White)
        {
            Name = name;
            Color = color;
            MaxLenght = maxLenght;
            IsStatColumn = false;
        }


        public Column(string name, int maxLenght, float averageValue, ConsoleColor color = ConsoleColor.White)
        {
            Name = name;
            Color = color;
            MaxLenght = maxLenght;
            AverageValue = averageValue;
            IsStatColumn = true;
        }

        public void Write() 
            => ConsoleWriteService.Write(Name, MaxLenght, Color);

        public void WriteUnderline()
        {
            for (int i = 0; i < MaxLenght; i++)
                Console.Write('-');
            Console.Write(" | ");
        }
    }
}

using ConsoleUI.ConsoleTable.Styles;
using ConsoleUI.ConsoleTable.Structural;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleUI.ConsoleTable.Services
{
    public abstract class ConsoleWriteService
    {

        public static void WriteValue(string value, int maxLenght = 20, bool isStat = false)
        {
            string AddPersentSymbol(string text)
            {
                if (text.Contains('%')) return text;


                if (text.Length > maxLenght)
                {
                    StringBuilder builder = new StringBuilder();
                    for (int i = 0; i < value.Length - (text.Length - maxLenght) - 1; i++)
                        builder.Append(text[i]);
                    text = builder.ToString();
                }
                else if (text.Length == maxLenght)
                {

                    text.Remove(text.Length - 1);
                }

                text = $"{text}%";

                return text;
            }

            string FillTomaxLenght()
            {
                value = value.Trim();
                if (value.Length > maxLenght)
                {
                    StringBuilder builder = new  StringBuilder();
                    for (int i = 0; i < value.Length - (value.Length - maxLenght) - 2; i++)
                        builder.Append(value[i]);
                    builder.Append("..");
                    value = builder.ToString();
                }
                else if (value.Length < maxLenght)
                {
                    float num = (maxLenght - value.Length) / 2;
                    for (float i = num; i >= 1; i--)
                        value = $" {value}";

                    for (int i = maxLenght - value.Length; i >= 1; i--)
                        value = $"{value} ";
                }

                return value;
            }

            if (isStat)
                value = AddPersentSymbol(value);
            value = FillTomaxLenght();

            Console.Write(value);
        }

        public static void Write(string text, int maxLenght = 20, ConsoleColor color = ConsoleColor.White, ConsolePosition position = null)
        {
            if (text == null) throw new ArgumentNullException("Text can't be null !");
            if (maxLenght < 0) throw new ArgumentOutOfRangeException("Max Legnht can't be less than zero !");
            if (position != null) Console.SetCursorPosition(position.Left, position.Top);

            Console.ForegroundColor = color;


            WriteValue(text, maxLenght);

            Console.ResetColor();

            Console.Write(" | ");
        }

        public static void Write(List<string> texts, int maxLenght = 20, ConsoleColor color = ConsoleColor.White, ConsolePosition position = null)
            => texts.ForEach(t => Write(t, maxLenght, color, position));


        public static void WriteStat(float value, float average, int maxLenght = 20, ConsolePosition position = null)
        {
            if (value == null) throw new ArgumentNullException("Value can't be null !");
            if (maxLenght < 0) throw new ArgumentOutOfRangeException("Max Legnht can't be less than zero !");
            if (position != null) Console.SetCursorPosition(position.Left, position.Top);

            Console.ForegroundColor = value == average ? Style.EqualColor : value > average ? Style.GreaterColor : Style.LessColor;

            WriteValue(value.ToString(), maxLenght, true);

            Console.ResetColor();

            Console.Write(" | ");
        }

        public static void WriteStat(List<string> values, float average, int maxLenght = 20, ConsolePosition position = null)
            => values.ForEach(v => WriteStat(float.Parse(v), average, maxLenght, position));


    }
}

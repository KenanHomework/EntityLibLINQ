using System;
using System.Collections.Generic;

namespace ConsoleUI.ConsoleTable.Structural
{
    public class Table
    {
        private List<Column> Colums = new List<Column>();

        private List<Row> Rows = new List<Row>();

        public Table(List<Column> colums)
        {
            Colums = colums;
        }

        public void Reset()
        {
            Rows = new List<Row>();
            Colums = new List<Column>();
        }

        public void AddRow(Row row)
        {
            if (row.Values.Count != Colums.Count) throw new ArgumentException("Row values count not equal Colums count !");
            row.Columns = Colums;
            Rows.Add(row);
        }

        public void Write()
        {
            Colums.ForEach(c => c.Write());
            Console.WriteLine();
            Colums.ForEach(c => c.WriteUnderline());
            Console.WriteLine();
            Rows.ForEach(r => r.Write());
        }
    }
}

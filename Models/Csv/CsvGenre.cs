﻿namespace BlazorLibrary.Models.Csv
{
    public class CsvGenre
    {
        public CsvGenre(string name)
        {
            this.Name = name;
        }

        public string Name { get; set; }
    }
}
﻿namespace BlazorLibrary.Modelle
{
    public class Genre
    {
        public Genre(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return $"{{{nameof(Id)}={Id}, {nameof(Name)}={Name}}}";
        }
    }
}
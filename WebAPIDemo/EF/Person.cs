namespace WebAPIDemo.EF
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public Sexes Sex { get; set; } 
    }
    public enum Sexes
    { 
        Man = 0,
        Woman = 1,
    }
}

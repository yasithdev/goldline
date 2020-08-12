namespace Core
{
    public class Entity
    {
        public Entity(int id, string name, string contactInfo)
        {
            Id = id;
            Name = name;
            ContactInfo = contactInfo;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string ContactInfo { get; set; }
    }
}
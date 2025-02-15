namespace CodeBase.Facts
{
    public class BreedFact
    {
        private readonly string _name;
        private readonly string _description;

        public string Name => _name;
        public string Description => _description;

        public BreedFact(string name, string description)
        {
            _name = name;
            _description = description;
        }
    }
}
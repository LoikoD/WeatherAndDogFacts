namespace CodeBase.Facts
{
    public class BreedData
    {
        private readonly string _id;
        private readonly string _name;

        public string Id => _id;
        public string Name => _name;

        public BreedData(string id, string name)
        {
            _id = id;
            _name = name;
        }
    }
}
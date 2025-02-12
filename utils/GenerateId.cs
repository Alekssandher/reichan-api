using IdGen;

namespace reichan_api.Utils
{
    public static class SnowflakeIdGenerator
    {
        private static readonly IIdGenerator<long> _generator;

        static SnowflakeIdGenerator()
        {
            var epoch = new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var structure = new IdStructure(41, 10, 12); 
            var options = new IdGeneratorOptions(structure, new DefaultTimeSource(epoch));

            _generator = new IdGenerator(123, options); 
        }

        public static long GenerateId() => _generator.CreateId();
    }
}

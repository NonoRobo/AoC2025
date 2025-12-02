using AoCTools.Loggers;
using AoCTools.Workers;
using System.Text;
using System.Text.RegularExpressions;

namespace AoC2025.Workers.Day02
{
    public class ProductIdWorker : WorkerBase
    {
        override public object Data => _ranges;
        private IdRanges _ranges;

        protected override void ProcessDataLines()
        {
            _ranges = new IdRanges();
            var line = DataLines.First();
            var parts = line.Split(',', 
                StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            foreach (var part in parts)
            {
                var rangeParts = part.Split('-',
                    StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                _ranges.Ranges.Add((long.Parse(rangeParts[0]), long.Parse(rangeParts[1])));
            }
        }

        private static string _invalidIdPattern = @"^([0-9]+)\1$";
        private Regex _invalidIdRegex = new Regex(_invalidIdPattern, RegexOptions.Compiled);
        protected override long WorkOneStar_Implementation()
        {
            return Compute(_invalidIdRegex);
        }

        private static string _invalidComplexIdPattern = @"^([0-9]+)\1{1,}$";
        private Regex _invalidComplexIdRegex = new Regex(_invalidComplexIdPattern, RegexOptions.Compiled);
        protected override long WorkTwoStars_Implementation()
        {
            return Compute(_invalidComplexIdRegex);
        }

        private long Compute(Regex regex)
        {
            var result = 0L;
            foreach (var (min, max) in _ranges.Ranges)
            {
                Logger.Log($"Range {min} to {max}");
                for (long i = min; i <= max; i++)
                {
                    var numStr = i.ToString();
                    if (regex.Match(numStr).Success)
                    {
                        result += i;
                        Logger.Log($" > Invalid ID {numStr} (total {result})", SeverityLevel.Low);
                    }
                }
            }
            return result;
        }
    }

    public class IdRanges
    {
        public List<(long Min, long Max)> Ranges { get; init; } = new List<(long Min, long Max)>();

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("=== ID RANGES ===");
            foreach (var (min, max) in Ranges)
            {
                sb.AppendLine($"{min} to {max}");
            }
            return sb.ToString();
        }
    }
}

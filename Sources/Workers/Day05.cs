using AoCTools.Loggers;
using AoCTools.Workers;
using System.Text;
using Range = AoCTools.Numbers.Range;

namespace AoC2025.Workers.Day05
{
    public class FoodSpoilageWorker : WorkerBase
    {
        public override object Data => _inventory;
        private FoodInventory _inventory;

        protected override void ProcessDataLines()
        {
            _inventory = new FoodInventory();
            int i = 0;
            for (; i < DataLines.Length; i++)
            {
                string line = DataLines[i];
                if (string.IsNullOrEmpty(line))
                    break;

                var parts = line.Split('-', StringSplitOptions.RemoveEmptyEntries);
                _inventory.FreshRanges.Add(Range.CreateFromMinMax(long.Parse(parts[0]), long.Parse(parts[1])));
            }
            i++;
            for (; i < DataLines.Length; i++)
            {
                string line = DataLines[i];
                _inventory.Items.Add(long.Parse(line));
            }
        }

        protected override long WorkOneStar_Implementation()
        {
            var freshCount = 0;
            foreach (var item in _inventory.Items)
            {
                var isFresh = false;
                foreach (var range in _inventory.FreshRanges)
                {
                    if (range.IsInRange(item))
                    {
                        freshCount++;
                        isFresh = true;
                        Logger.Log($"Item {item} is in range {range}. Total fresh = {freshCount}.");
                        break;
                    }
                }

                if (!isFresh)
                    Logger.Log($"Item {item} is spoiled.");
            }
            return freshCount;
        }

        protected override long WorkTwoStars_Implementation()
        {
            var freshRanges = new List<Range> { _inventory.FreshRanges[0] };
            Logger.Log($"Range {freshRanges[0]} added to Fresh (1).");
            foreach (var range in _inventory.FreshRanges.Skip(1))
            {
                var curRange = range;
                var tryOverlapFix = true;
                do
                {
                    Logger.Log($"Analysing range {curRange}...");
                    var overlapped = freshRanges.Find(fr => fr.IsInRange(curRange, out _));
                    if (overlapped == null)
                    {
                        freshRanges.Add(curRange);
                        Logger.Log($"Range {curRange} added to Fresh ({freshRanges.Count}).");
                        tryOverlapFix = false;
                        continue;
                    }

                    Logger.Log($" > range {curRange} overlaps with {overlapped}.");
                    freshRanges.Remove(overlapped);
                    Logger.Log($"Removed range {overlapped} from Fresh ({freshRanges.Count}).");
                    var unioned = Range.CreateFromUnion(curRange, overlapped);
                    Logger.Log($" > using range {unioned[0]} instead of {overlapped} and {curRange}.");
                    curRange = unioned[0];
                } while (tryOverlapFix);
            }
            return freshRanges.Sum(r => (long)r.Size);
        }
    }

    public class FoodInventory
    {
        public List<Range> FreshRanges { get; init; } = new();
        public List<long> Items { get; init; } = new();

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("=== FOOD INVENTORY ===");
            sb.AppendLine($"Fresh ranges = {FreshRanges.Count} ; Items = {Items.Count}");

            foreach (var range in FreshRanges)
            {
                sb.AppendLine($"{range}");
            }
            sb.AppendLine("- - - - -");
            foreach (var item in Items)
            {
                sb.AppendLine($"{item}");
            }
            return sb.ToString();
        }
    }
}

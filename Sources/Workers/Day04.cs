using AoCTools.Frame.TwoDimensions;
using AoCTools.Frame.TwoDimensions.Map;
using AoCTools.Loggers;
using AoCTools.Workers;
using System.Text;

namespace AoC2025.Workers.Day04
{
    public class PaperRollWorker : WorkerBase
    {
        public override object Data => _map;
        private PaperRollMap _map;

        protected override void ProcessDataLines()
        {
            _map = new PaperRollMap(DataLines);
        }

        protected override long WorkOneStar_Implementation()
        {
            var rolls = FindRolls();
            Logger.Log($"Found {rolls.Count} paper rolls.", SeverityLevel.Low);
            Logger.Log(string.Join("\n", rolls.Select(r => r.ToString())), SeverityLevel.Low);
            return rolls.Count(r => r.Neighbors.Count < 4);
        }

        protected override long WorkTwoStars_Implementation()
        {
            var rolls = FindRolls();
            Logger.Log($"Found {rolls.Count} paper rolls.", SeverityLevel.Low);

            var removedRolls = 0;
            while (rolls.Count > 0)
            {
                var accessibleRolls = rolls.Where(r => r.Neighbors.Count(n => !n.Removed) < 4).ToList();
                if (accessibleRolls.Count == 0)
                    break;

                foreach (var accessibleRoll in accessibleRolls)
                {
                    accessibleRoll.Removed = true;
                    rolls.Remove(accessibleRoll);
                    removedRolls++;
                }
            }
            return removedRolls;
        }

        private List<PaperRollInfo> FindRolls()
        {
            var rolls = new List<PaperRollInfo>();
            for (var i = 0; i < _map.RowCount; i++)
            {
                for (var j = 0; j < _map.ColCount; j++)
                {
                    if (_map.MapCells[i][j].Content == '@')
                    {
                        var roll = new PaperRollInfo
                        {
                            Pos = new Coordinates(i, j)
                        };

                        rolls.Add(roll);
                        Logger.Log($" > adding roll at {roll.Pos}");

                        var leftNeighbor = rolls.Find(r => r.Pos.Equals(new Coordinates(i, j - 1)));
                        if (leftNeighbor != null)
                        {
                            roll.Neighbors.Add(leftNeighbor);
                            leftNeighbor.Neighbors.Add(roll);
                            Logger.Log($"   > found left neighbor at {leftNeighbor.Pos}");
                        }

                        var topNeighbor = rolls.Find(r => r.Pos.Equals(new Coordinates(i - 1, j)));
                        if (topNeighbor != null)
                        {
                            roll.Neighbors.Add(topNeighbor);
                            topNeighbor.Neighbors.Add(roll);
                            Logger.Log($"   > found top neighbor at {topNeighbor.Pos}");
                        }

                        var topLeftNeighbor = rolls.Find(r => r.Pos.Equals(new Coordinates(i - 1, j - 1)));
                        if (topLeftNeighbor != null)
                        {
                            roll.Neighbors.Add(topLeftNeighbor);
                            topLeftNeighbor.Neighbors.Add(roll);
                            Logger.Log($"   > found top-left neighbor at {topLeftNeighbor.Pos}");
                        }

                        var topRightNeighbor = rolls.Find(r => r.Pos.Equals(new Coordinates(i - 1, j + 1)));
                        if (topRightNeighbor != null)
                        {
                            roll.Neighbors.Add(topRightNeighbor);
                            topRightNeighbor.Neighbors.Add(roll);
                            Logger.Log($"   > found top-right neighbor at {topRightNeighbor.Pos}");
                        }
                    }
                }
            }
            return rolls;
        }
    }

    public class PaperRollInfo
    {
        public Coordinates Pos { get; set; }
        public List<PaperRollInfo> Neighbors = new List<PaperRollInfo>();
        public bool Removed { get; set; } = false;

        public override string ToString()
        {
            return $"Roll at {Pos} has {Neighbors.Count} neighbors.";
        }
    }

    public class PaperRollMap : CharMap
    {
        public PaperRollMap(string[] lines) : base(lines)
        { }

        protected override string LogTitle => "=== PAPER ROLL MAP ===";
    }
}

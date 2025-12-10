using AoCTools.Frame.TwoDimensions.Map.Abstracts;
using AoCTools.Loggers;
using AoCTools.Workers;

namespace AoC2025.Workers.Day07
{
    public class TachyonBeamWorker : WorkerBase
    {
        public override object Data => _manifold;
        private TachyonManifold _manifold;

        protected override void ProcessDataLines()
        {
            _manifold = new TachyonManifold(DataLines.Select(dl => dl.ToCharArray()).ToArray());
        }

        protected override long WorkOneStar_Implementation()
        {
            var beamToExtend = new List<TachyonManicell> { _manifold.Start };

            var divisionCount = 0;
            while (beamToExtend.Any())
            {
                var beam = beamToExtend.First();
                beamToExtend.RemoveAt(0);

                if (beam.Content == TachyonState.Beam)
                {
                    continue;
                }

                if (beam.Content == TachyonState.Empty && !beam.IsStart)
                    beam.SetContent(TachyonState.Beam);

                if (!_manifold.TryGetCell(beam.Coordinates.Row + 1, beam.Coordinates.Col, out var nextBeam))
                {
                    continue;
                }

                switch (nextBeam.Content)
                {
                    case TachyonState.Empty:
                        beamToExtend.Add(nextBeam);
                        break;

                    case TachyonState.Divider:
                        divisionCount++;
                        if (_manifold.TryGetCell(nextBeam.Coordinates.Row, nextBeam.Coordinates.Col - 1, out var dividerLeft))
                            beamToExtend.Add(dividerLeft);
                        if (_manifold.TryGetCell(nextBeam.Coordinates.Row, nextBeam.Coordinates.Col + 1, out var dividerRight))
                            beamToExtend.Add(dividerRight);
                        break;
                }
            }

            Logger.Log($"Divisions: {divisionCount}");
            Logger.Log(_manifold.ToString());

            return divisionCount;
        }
    }

    public class TachyonManifold : Map<TachyonManicell>
    {
        public TachyonManicell Start { get; private set; }

        public TachyonManifold(char[][] charCells) : base(charCells)
        {
            Start = GetCell(0, Array.IndexOf(charCells[0], 'S'));
            Start.IsStart = true;
            Logger.Log($"Start at {Start.Coordinates}");
        }

        protected override string LogTitle => "=== TACHYON MANIFOLD ===";
    }

    public class TachyonManicell : MapCell<TachyonState>
    {
        public bool IsStart { get; set; } = false;

        public TachyonManicell(TachyonState content, int row, int col) : base(content, row, col)
        { }

        public TachyonManicell(char content, int row, int col) : this(CharToType(content), row, col)
        { }

        public override string ToString()
        {
            return TypeToString(Content, IsStart);
        }

        public void SetContent(TachyonState newContent)
        {
            Content = newContent;
        }

        private static TachyonState CharToType(char content)
        {
            switch (content)
            {
                case 'S':
                case '.':
                    return TachyonState.Empty;
                case '|':
                    return TachyonState.Beam;
                case '^':
                    return TachyonState.Divider;
                default:
                    throw new ArgumentOutOfRangeException(nameof(content), $"Invalid cell content '{content}'.");
            }
        }

        private static string TypeToString(TachyonState content, bool isStart)
        {
            switch (content)
            {
                case TachyonState.Empty:
                    return isStart ? "S" : ".";
                case TachyonState.Beam:
                    return "|";
                case TachyonState.Divider:
                    return "^";
                default:
                    throw new ArgumentOutOfRangeException(nameof(content), $"Invalid cell content '{content}'.");
            }
        }
    }

    public enum TachyonState
    {
        Empty,
        Beam,
        Divider
    }
}

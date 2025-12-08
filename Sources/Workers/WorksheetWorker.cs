using AoCTools.Loggers;
using AoCTools.Workers;
using System.Text;

namespace AoC2025.Workers.Day06
{
    public class WorksheetWorker : WorkerBase
    {
        public override object Data => _sheet;
        private Worksheet _sheet;

        protected override void ProcessDataLines()
        {
            _sheet = new Worksheet();

            foreach (var line in DataLines.SkipLast(1))
            {
                var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var row = parts.Select(p => long.Parse(p)).ToArray();
                _sheet.Rows.Add(row);
            }

            _sheet.Operations.AddRange(DataLines.Last().Where(c => c != ' '));
        }

        protected override long WorkOneStar_Implementation()
        {
            var columns = _sheet.Rows[0].Length;
            Logger.Log($"Worksheet has {columns} calculus.");

            var total = 0L;
            for (int i = 0; i < columns; i++)
            {
                if (_sheet.Operations[i] == '+')
                {
                    var sum = _sheet.Rows.Select(r => r[i]).Sum();
                    Logger.Log($"Column {i} is a SUM = {sum}");
                    total += sum;
                }
                else if (_sheet.Operations[i] == '*')
                {
                    var mul = 1L;
                    foreach (var value in _sheet.Rows.Select(r => r[i]))
                        mul *= value;
                    Logger.Log($"Column {i} is a MULTIPLICATION = {mul}");
                    total += mul;
                }
                else
                {
                    Logger.Log($"Column {i} requires unknown calculus {_sheet.Operations[i]}", SeverityLevel.Always);
                }
                Logger.Log($" > TOTAL = {total}");
            }

            return total;
        }
    }

    public class Worksheet
    {
        public List<long[]> Rows { get; } = new List<long[]>();
        public List<char> Operations { get; } = new List<char>();

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("=== WORKSHEET ===");
            sb.AppendLine($"Rows: {Rows.Count}");
            foreach (var row in Rows)
            {
                sb.AppendLine($"  {string.Join(" ", row)}");
            }
            sb.AppendLine($"Operations: {string.Join(" ", Operations)} ({Operations.Count})");
            return sb.ToString();
        }
    }
}

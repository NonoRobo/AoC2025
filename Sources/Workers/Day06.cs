using AoCTools.Loggers;
using AoCTools.Workers;
using System.Text;

namespace AoC2025.Workers.Day06
{
    public class WorksheetWorker : WorkerBase
    {
        public override object Data => _sheet;
        private Worksheet _sheet;

        private bool _useCephalopodMath = false;

        public WorksheetWorker(bool useCephalopodMath) : base()
        {
            _useCephalopodMath = useCephalopodMath;
        }

        protected override void ProcessDataLines()
        {
            _sheet = new Worksheet();

            if (_useCephalopodMath)
                ReadCephalopodMath();
            else
                ReadNormalMath();
        }

        private void ReadNormalMath()
        {
            foreach (var line in DataLines.SkipLast(1))
            {
                var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var row = parts.Select(p => long.Parse(p)).ToArray();
                _sheet.AddValueRow(row);
            }

            _sheet.AddOperationRow(DataLines.Last().Where(c => c != ' ').ToArray());
        }

        private void ReadCephalopodMath()
        {
            var lines = DataLines.SkipLast(1).ToArray();
            var operations = DataLines.Last();

            var numbers = new List<long>();
            for (int idx = lines[0].Length - 1; idx >= 0; idx--)
            {
                var number = 0L;
                var decalMul = 1L;
                for (int l = lines.Length - 1; l >= 0; l--)
                {
                    var digitStr = lines[l][idx];
                    var digit = digitStr == ' ' ? 0L : (digitStr - '0');

                    if (digit == 0)
                        continue;

                    number += digit * decalMul;
                    decalMul *= 10L;
                }

                if (number != 0)
                {
                    numbers.Add(number);
                    continue;
                }

                _sheet.AddCalculus(numbers, operations[idx + 1]);
                numbers.Clear();
            }

            _sheet.AddCalculus(numbers, operations[0]);
        }

        protected override long WorkOneStar_Implementation()
        {
            return DoTheMath();
        }

        protected override long WorkTwoStars_Implementation()
        {
            return DoTheMath();
        }

        private long DoTheMath()
        {
            Logger.Log($"Worksheet has {_sheet.Calculuses.Count} calculus.");

            var total = 0L;
            foreach (var calculus in _sheet.Calculuses)
            {
                if (calculus.Operation == '+')
                {
                    var sum = calculus.Values.Sum();
                    Logger.Log($"SUM = {sum}");
                    total += sum;
                }
                else if (calculus.Operation == '*')
                {
                    var mul = 1L;
                    foreach (var value in calculus.Values)
                        mul *= value;
                    Logger.Log($"MULTIPLICATION = {mul}");
                    total += mul;
                }
                else
                {
                    Logger.Log($"Unknown calculus operation {calculus.Operation}", SeverityLevel.Always);
                }
                Logger.Log($" > TOTAL = {total}");
            }

            return total;
        }
    }

    public class Calculus
    {
        public char Operation { get; set; }
        public List<long> Values { get; } = new List<long>();

        public override string ToString()
        {
            return string.Join($" {Operation} ", Values);
        }
    }

    public class Worksheet
    {
        public List<Calculus> Calculuses { get; } = new List<Calculus>();

        public void AddValueRow(long[] row)
        {
            for (int i = 0; i < row.Length; i++)
            {
                if (Calculuses.Count <= i)
                    Calculuses.Add(new Calculus());
                Calculuses[i].Values.Add(row[i]);
            }
        }

        public void AddOperationRow(char[] operations)
        {
            for (int i = 0; i < operations.Length; i++)
            {
                if (Calculuses.Count <= i)
                    Calculuses.Add(new Calculus());
                Calculuses[i].Operation = operations[i];
            }
        }

        public void AddCalculus(List<long> values, char operation)
        {
            var calculus = new Calculus { Operation = operation };
            calculus.Values.AddRange(values);
            Calculuses.Add(calculus);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("=== WORKSHEET ===");
            sb.AppendLine($"Calculuses: {Calculuses.Count}");
            foreach (var calculus in Calculuses)
            {
                sb.AppendLine(calculus.ToString());
            }
            return sb.ToString();
        }
    }
}

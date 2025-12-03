using AoCTools.Loggers;
using AoCTools.Workers;
using System.Text;

namespace AoC2025.Workers.Day03
{
    public class JoltageWorker : WorkerBase
    {
        public override object Data => _banks;
        private BatteryBanks _banks;

        protected override void ProcessDataLines()
        {
            _banks = new BatteryBanks();
            foreach (var line in DataLines)
            {
                _banks.AddFromLine(line);
            }
        }

        protected override long WorkOneStar_Implementation()
        {
            var totalMaxJoltage = 0;
            foreach (var bank in _banks.Banks)
            {
                Logger.Log($"Processing bank {bank.ToString()}...", SeverityLevel.Low);

                var maxJoltage = -1;
                for (int cur10 = 0; cur10 < _banks.BankSize - 1; cur10++)
                {
                    var dozen = maxJoltage / 10;
                    if (bank.Batteries[cur10] < dozen)
                    {
                        Logger.Log($"   > skip index {cur10} (out of {_banks.BankSize - 2}) because {bank.Batteries[cur10]} < {dozen}.", SeverityLevel.Low);
                        continue;
                    }

                    Logger.Log($" > index {cur10} (out of {_banks.BankSize - 2})", SeverityLevel.Low);
                    for (int cur1 = cur10 + 1; cur1 < _banks.BankSize; cur1++)
                    {
                        var curJoltage = bank.Batteries[cur10] * 10 + bank.Batteries[cur1];
                        if (curJoltage > maxJoltage)
                        {
                            maxJoltage = curJoltage;
                            Logger.Log($" > max joltage = {maxJoltage}.", SeverityLevel.Low);
                        }
                    }
                }
                totalMaxJoltage += maxJoltage;
                Logger.Log($"Total max joltage = {totalMaxJoltage}.", SeverityLevel.Low);
            }
            return totalMaxJoltage;
        }
    }

    public class BatteryBanks
    {
        public List<BatteryBank> Banks { get; init; } = new List<BatteryBank>();
        public int BankSize = 0;

        override public string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("=== BATTERY BANKS ===");
            foreach (var bank in Banks)
            {
                sb.AppendLine(bank.ToString());
            }
            return sb.ToString();
        }

        public void AddFromLine(string line)
        {
            var batteries = line.Select(c => int.Parse(c.ToString())).ToArray();
            Banks.Add(new BatteryBank(batteries));
            BankSize = batteries.Length;
        }
    }

    public class BatteryBank
    {
        public int[] Batteries {  get; private set; }

        public BatteryBank(int[] batteries)
        {
            Batteries = batteries;
        }

        override public string ToString()
        {
            return string.Join("", Batteries);
        }
    }
}

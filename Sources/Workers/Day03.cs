using AoCTools.Loggers;
using AoCTools.Workers;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
            return ProcessHighestJoltageInBanks(2);
        }

        protected override long WorkTwoStars_Implementation()
        {
            return ProcessHighestJoltageInBanks(12);
        }

        private long ProcessHighestJoltageInBanks(int joltageDigitCount)
        {
            var totalMaxJoltage = 0L;
            foreach (var bank in _banks.Banks)
            {
                Logger.Log($"Processing bank {bank.ToString()}...", SeverityLevel.Low);

                var maxJoltage = 0L;
                var previousDigitIndex = -1;
                for (int digit = 0; digit < joltageDigitCount; digit++)
                {
                    var bestBattery = -1;
                    var end = bank.Batteries.Length - joltageDigitCount + digit + 1;
                    for (int i = previousDigitIndex + 1; i < end; i++)
                    {
                        if (bank.Batteries[i] > bestBattery)
                        {
                            bestBattery = bank.Batteries[i];
                            previousDigitIndex = i;
                        }
                    }
                    maxJoltage = maxJoltage * 10 + bestBattery;
                    Logger.Log($" > [{digit}] Best battery is {bestBattery} (at {previousDigitIndex}) => local joltage {maxJoltage} (from {digit} to {end})");
                }
                totalMaxJoltage += maxJoltage;
                Logger.Log($" > Local joltage {maxJoltage} => Total = {totalMaxJoltage}");
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

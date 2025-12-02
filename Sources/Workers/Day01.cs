using AoCTools.Loggers;
using AoCTools.Workers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2025.Workers.Day01
{
    public class DialWorker : WorkerBase
    {
        public override object Data => _inputs;
        private DialInputs _inputs;

        protected override void ProcessDataLines()
        {
            _inputs = new DialInputs();
            foreach (var line in DataLines)
            {
                _inputs.Add(line);
            }
        }

        protected override long WorkOneStar_Implementation()
        {
            var curValue = 50;
            var zeroCount = 0;
            Logger.Log($"Start\t->\t{curValue}", SeverityLevel.Low);
            foreach (var input in _inputs.Inputs)
            {
                curValue = (curValue + input.TrueValue + 100) % 100;
                Logger.Log($"{input.TrueValue.ToString("+0#;-0#;0")}\t->\t{curValue}", SeverityLevel.Low);
                if (curValue == 0)
                {
                    zeroCount++;
                    Logger.Log($"> On zero! Total {zeroCount}", SeverityLevel.Low);
                }
            }
            return zeroCount;
        }

        protected override long WorkTwoStars_Implementation()
        {
            var curValue = 50;
            var zeroClicks = 0;
            var previousWasHit0 = false;
            var previousWasHit100 = false;

            Logger.Log($"Start\t->\t{curValue}", SeverityLevel.Low);
            foreach (var input in _inputs.Inputs)
            {
                curValue += input.TrueValue;
                Logger.Log($"{input.TrueValue.ToString("+0#;-0#")}\t->\t{curValue}", SeverityLevel.Low);

                while (curValue < 0)
                {
                    if (previousWasHit0)
                    {
                        previousWasHit0 = false;
                    }
                    else
                    {
                        zeroClicks++;
                        Logger.Log($"  > Clicked on zero (L) : total {zeroClicks}", SeverityLevel.Low);
                    }

                    curValue += 100;
                    Logger.Log($"\t\t->\t{curValue}", SeverityLevel.Low);
                }

                while (curValue > 100)
                {
                    if (previousWasHit100)
                    {
                        previousWasHit100 = false;
                    }
                    else
                    {
                        zeroClicks++;
                    Logger.Log($"  > Clicked on zero (R) : total {zeroClicks}", SeverityLevel.Low);
                    }

                    curValue -= 100;
                    Logger.Log($"\t\t->\t{curValue}", SeverityLevel.Low);
                }

                previousWasHit0 = false;
                previousWasHit100 = false;

                if (curValue == 0 || curValue == 100)
                {
                    zeroClicks++;

                    if (curValue == 100)
                    {
                        previousWasHit100 = true;
                    }
                    else
                    {
                        previousWasHit0 = true;
                    }

                        Logger.Log($"  > Clicked on zero (HIT) : total {zeroClicks}", SeverityLevel.Low);
                }
            }

            return zeroClicks;
        }
    }

    public class DialInputs
    {
        public List<DialInput> Inputs { get; private set; } = new List<DialInput>();

        public void Add(string code)
        {
            var letter = code.First();
            var value = int.Parse(code.Substring(1));
            Inputs.Add(new DialInput(StrToRota(letter), value));
        }

        private Rotation StrToRota(char c)
        {
            switch (c)
            {
                case 'L': return Rotation.Left;
                case 'R': return Rotation.Right;
                default: throw new ArgumentException($"Invalid rotation letter {c}.");
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("=== DIAL INPUTS ===");
            foreach (var input in Inputs)
            {
                sb.AppendLine(input.TrueValue.ToString("+0#;-0#;0"));
            }
            return sb.ToString();
        }
    }

    public class DialInput
    {
        public DialInput(Rotation rotation, int value)
        {
            Rotation = rotation;
            Value = value;
            TrueValue = rotation == Rotation.Left ? -value : value;
        }

        public Rotation Rotation { get; private set; }
        public int Value { get; private set; }
        public int TrueValue { get; private set; }
    }

    public enum Rotation
    {
        Left,
        Right
    }
}

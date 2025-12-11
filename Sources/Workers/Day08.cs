using AoCTools.Frame.ThreeDimensions;
using AoCTools.Loggers;
using AoCTools.Workers;
using System.Text;
using static AoC2025.Workers.Day08.LightBox;

namespace AoC2025.Workers.Day08
{
    public class LightBoxWorker : WorkerBase
    {
        public override object Data => _box;
        private LightBox _box;

        public long ConnectionCount { get; init; }

        public LightBoxWorker(long connectionCount = -1)
        {
            ConnectionCount = connectionCount;
        }

        protected override void ProcessDataLines()
        {
            _box = new LightBox();
            foreach (var line in DataLines)
            {
                var parts = line.Split(',');
                _box.Junctions.Add(new Coordinates(long.Parse(parts[0]), long.Parse(parts[1]), long.Parse(parts[2])));
            }
        }

        protected override long WorkOneStar_Implementation()
        {
            _box.ComputeShortConnections(ConnectionCount);
            Logger.Log(_box.ToConnectionString(), SeverityLevel.Low);

            _box.ComputeCircuits();
            Logger.Log(_box.ToCircuitString(), SeverityLevel.Low);

            var biggerCircuits = _box.Circuits.OrderByDescending(c => c.Junctions.Count).Take(3);

            var mul = 1L;
            foreach (var circ in biggerCircuits)
                mul *= circ.Junctions.Count;
            return mul;
        }

        protected override long WorkTwoStars_Implementation()
        {
            _box.ComputeAllConnections();
            Logger.Log(_box.ToConnectionString(), SeverityLevel.Low);

            var lastConnection = _box.ComputeCircuitsUntilAllInOne();
            Logger.Log(_box.ToCircuitString(), SeverityLevel.Low);

            return lastConnection.From.Row * lastConnection.To.Row;
        }
    }

    public class LightBox
    {
        public class Connection
        {
            public Coordinates From { get; set; }
            public Coordinates To { get; set; }
            public double Distance { get; set; }
        }

        public class Circuit
        {
            public List<Coordinates> Junctions { get; init; } = new();
        }

        public List<Coordinates> Junctions { get; init; } = new();

        public List<Connection> Connections { get; init; } = new();

        public List<Circuit> Circuits { get; init; } = new();

        public void ComputeShortConnections(long connectionCount)
        {
            var maxDistance = double.MaxValue;
            Connection fartherJunction = null;
            for (int i = 0; i < Junctions.Count; i++)
            {
                for (int j = i + 1; j < Junctions.Count; j++)
                {
                    var distance = Junctions[i].Distance(Junctions[j]);
                    if (distance >= maxDistance)
                        continue;

                    var newConnection = new Connection
                    {
                        From = Junctions[i],
                        To = Junctions[j],
                        Distance = distance
                    };

                    if (Connections.Count >= connectionCount)
                        Connections.Remove(fartherJunction);

                    if (Connections.Count == 0)
                    {
                        fartherJunction = newConnection;
                        maxDistance = distance;
                    }
                    else
                    {
                        fartherJunction = Connections.OrderBy(c => c.Distance).Last();
                        maxDistance = fartherJunction.Distance;
                    }

                    Connections.Add(newConnection);
                }
            }
        }

        public void ComputeAllConnections()
        {
            for (int i = 0; i < Junctions.Count; i++)
            {
                for (int j = i + 1; j < Junctions.Count; j++)
                {
                    Connections.Add(new Connection
                    {
                        From = Junctions[i],
                        To = Junctions[j],
                        Distance = Junctions[i].Distance(Junctions[j])
                    });
                }
            }
        }

        public void ComputeCircuits()
        {
            foreach (var connection in Connections)
            {
                ApplyConnection(connection);
            }
        }

        public Connection ComputeCircuitsUntilAllInOne()
        {
            var junctionsUnplugged = new List<Coordinates>(Junctions);
            foreach (var connection in Connections.OrderBy(c => c.Distance))
            {
                ApplyConnection(connection);

                junctionsUnplugged.Remove(connection.From);
                junctionsUnplugged.Remove(connection.To);

                if (!junctionsUnplugged.Any() && Circuits.Count == 1)
                {
                    Logger.Log($"LAST CONNECTION HIT!!!");
                    return connection;
                }
            }

            return null;
        }

        public void ApplyConnection(Connection connection)
        {
            Logger.Log($"Connecting from {connection.From} to {connection.To}...");

            var fromCircuit = Circuits.FirstOrDefault(c => c.Junctions.Any(j => j == connection.From));
            var toCircuit = Circuits.FirstOrDefault(c => c.Junctions.Any(j => j == connection.To));

            if (fromCircuit == null && toCircuit == null)
            {
                var newCircuit = new Circuit();
                newCircuit.Junctions.Add(connection.From);
                newCircuit.Junctions.Add(connection.To);
                Circuits.Add(newCircuit);
                Logger.Log($" > new circuit created.");
            }
            else if (fromCircuit == null && toCircuit != null)
            {
                toCircuit.Junctions.Add(connection.From);
                Logger.Log($" > FROM added to TO.");
            }
            else if (toCircuit == null && fromCircuit != null)
            {
                fromCircuit.Junctions.Add(connection.To);
                Logger.Log($" > TO added to FROM.");
            }
            else
            {
                if (fromCircuit != toCircuit)
                {
                    fromCircuit.Junctions.AddRange(toCircuit.Junctions);
                    Circuits.Remove(toCircuit);
                    Logger.Log($" > merged circuits.");
                }
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("=== LIGHT BOX ===");
            sb.AppendLine($"Junctions: {Junctions.Count}");
            foreach (var junction in Junctions)
            {
                sb.AppendLine(junction.ToString());
            }
            return sb.ToString();
        }

        public string ToConnectionString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("=== LIGHT BOX CONNECTIONS ===");
            sb.AppendLine($"Connections: {Connections.Count}");
            foreach (var connection in Connections.OrderBy(c => c.Distance))
                sb.AppendLine($"From {connection.From} to {connection.To} : {connection.Distance}");
            return sb.ToString();
        }

        public string ToCircuitString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("=== LIGHT BOX CIRCUITS ===");
            sb.AppendLine($"Circuits: {Circuits.Count}");
            foreach (var circuit in Circuits.OrderByDescending(c => c.Junctions.Count))
                sb.AppendLine($"Circuit ({circuit.Junctions.Count}) : {string.Join(", ", circuit.Junctions)}");
            return sb.ToString();
        }
    }
}

using AoCTools.Loggers;
using AoCTools.Workers;
using NUnit.Framework;
using System.IO;

namespace AoC2025_Tests
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    public class Days
    {
        private string GetDataPath(int day, string append = "")
        {
            return Path.Combine(TestContext.CurrentContext.TestDirectory, "Data", "Days", $"{day:00}{append}.txt");
        }

        private void TestOneStar(IWorker worker, string dataPath, long expectedResult, SeverityLevel logLevel = SeverityLevel.Always)
        {
            var work = worker.WorkOneStar(dataPath, logLevel);
            Assert.That(work, Is.EqualTo(expectedResult), $"One Star returned {work}, expected {expectedResult}.");
        }

        private void TestOneStar(IWorker worker, string dataPath, string expectedResult)
        {
            var work = worker.WorkOneStar_String(dataPath, SeverityLevel.Never);
            Assert.That(work, Is.EqualTo(expectedResult), $"One Star returned {work}, expected {expectedResult}.");
        }

        private void TestTwoStars(IWorker worker, string dataPath, long expectedResult, SeverityLevel logLevel = SeverityLevel.Always)
        {
            var work = worker.WorkTwoStars(dataPath, logLevel);
            Assert.That(work, Is.EqualTo(expectedResult), $"Two Stars returned {work}, expected {expectedResult}.");
        }

        [Test]
        public void Day01()
        {
            TestOneStar(new AoC2025.Workers.Day01.DialWorker(), GetDataPath(1), 1118);
            TestTwoStars(new AoC2025.Workers.Day01.DialWorker(), GetDataPath(1), 6289);
        }

        [Test]
        public void Day02()
        {
            TestOneStar(new AoC2025.Workers.Day02.ProductIdWorker(), GetDataPath(2), 29818212493);
            TestTwoStars(new AoC2025.Workers.Day02.ProductIdWorker(), GetDataPath(2), 37432260594);
        }

        [Test]
        public void Day03()
        {
            TestOneStar(new AoC2025.Workers.Day03.JoltageWorker(), GetDataPath(3), 17031);
            TestTwoStars(new AoC2025.Workers.Day03.JoltageWorker(), GetDataPath(3), 168575096286051);
        }

        [Test]
        public void Day04()
        {
            TestOneStar(new AoC2025.Workers.Day04.PaperRollWorker(), GetDataPath(4), 1367);
            TestTwoStars(new AoC2025.Workers.Day04.PaperRollWorker(), GetDataPath(4), 9144);
        }

        [Test]
        public void Day05()
        {
            TestOneStar(new AoC2025.Workers.Day05.FoodSpoilageWorker(), GetDataPath(5), 690);
            TestTwoStars(new AoC2025.Workers.Day05.FoodSpoilageWorker(), GetDataPath(5), 344323629240733);
        }

        //[Test]
        //public void Day06() // 17s
        //{
        //    TestOneStar(new AoC2024.Workers.Day06.PastPlaceAnalizer(), GetDataPath(6), 4973);
        //    TestTwoStars(new AoC2024.Workers.Day06.PastPlaceAnalizer(), GetDataPath(6), 1482);
        //}

        //[Test]
        //public void Day07()
        //{
        //    TestOneStar(new AoC2024.Workers.Day07.RopeBridge(), GetDataPath(7), 10741443549536);
        //    TestTwoStars(new AoC2024.Workers.Day07.RopeBridge(), GetDataPath(7), 500335179214836);
        //}

        //[Test]
        //public void Day08()
        //{
        //    TestOneStar(new AoC2024.Workers.Day08.Antennas(), GetDataPath(8), 329);
        //    TestTwoStars(new AoC2024.Workers.Day08.Antennas(), GetDataPath(8), 1190);
        //}

        //[Test]
        //public void Day09()
        //{
        //    TestOneStar(new AoC2024.Workers.Day09.Defragmentation(), GetDataPath(9), 6353658451014);
        //    TestTwoStars(new AoC2024.Workers.Day09.Defragmentation(), GetDataPath(9), 6382582136592);
        //}

        //[Test]
        //public void Day10()
        //{
        //    TestOneStar(new AoC2024.Workers.Day10.TrailFinder(), GetDataPath(10), 611);
        //    TestTwoStars(new AoC2024.Workers.Day10.TrailFinder(), GetDataPath(10), 1380);
        //}

        //[Test]
        //public void Day11()
        //{
        //    TestOneStar(new AoC2024.Workers.Day11.QuanticStoneBlinker(), GetDataPath(11), 217443);
        //    TestTwoStars(new AoC2024.Workers.Day11.QuanticStoneBlinker(), GetDataPath(11), 257246536026785);
        //}

        //[Test]
        //public void Day12()
        //{
        //    TestOneStar(new AoC2024.Workers.Day12.GardenFencer(), GetDataPath(12), 1467094);
        //    TestTwoStars(new AoC2024.Workers.Day12.GardenFencer(), GetDataPath(12), 881182);
        //}

        //[Test]
        //public void Day13()
        //{
        //    TestOneStar(new AoC2024.Workers.Day13.ClawMachineHacker(), GetDataPath(13), 38714);
        //    TestTwoStars(new AoC2024.Workers.Day13.ClawMachineHacker(), GetDataPath(13), 74015623345775);
        //}

        //[Test]
        //public void Day14()
        //{
        //    TestOneStar(new AoC2024.Workers.Day14.ToiletRobots { BathroomSize = new Coordinates(103, 101) },
        //                GetDataPath(14), 211773366);
        //    TestTwoStars(new AoC2024.Workers.Day14.ToiletRobots { BathroomSize = new Coordinates(103, 101) },
        //                GetDataPath(14), 7344, SeverityLevel.Low);
        //}

        //[Test]
        //public void Day15()
        //{
        //    TestOneStar(new AoC2024.Workers.Day15.AnglerFishWarehouse(), GetDataPath(15), 1437174);
        //    TestTwoStars(new AoC2024.Workers.Day15.AnglerFishWarehouse { DoubleSize = true }, GetDataPath(15), 1437468);
        //}

        //[Test]
        //public void Day16()
        //{
        //    TestOneStar(new AoC2024.Workers.Day16.ReindeerRace(), GetDataPath(16), 106512); // 30s
        //    TestTwoStars(new AoC2024.Workers.Day16.ReindeerRace(), GetDataPath(16), 563); // 29s
        //}

        //[Test]
        //public void Day17()
        //{
        //    TestOneStar(new AoC2024.Workers.Day17.Compiler(), GetDataPath(17), "7,3,5,7,5,7,4,3,0");
        //}
    }
}

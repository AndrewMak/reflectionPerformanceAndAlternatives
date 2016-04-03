using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ReflectionPerformanceAndAlternatives.Tests;

namespace ReflectionPerformanceAndAlternatives
{
	public class Program
	{
		private const int TIMES_TO_RUN_EACH_TEST = 1000;
		private const int SUBJECT_COUNT = 1000;
		private const int TIMES_TO_RUN_ALL_TESTS = 3;

		public static void Main()
		{
#if DEBUG
			Console.WriteLine("This test should be run in RELEASE mode. Press [Enter] to continue...");
			Console.ReadLine();
#endif

			var runs = new Dictionary<Type, List<long>>()
			{
				{typeof(OriginalTest), new List<long>(TIMES_TO_RUN_ALL_TESTS)},
				{typeof(EmitILTest), new List<long>(TIMES_TO_RUN_ALL_TESTS)},
				{typeof(DynamicTest), new List<long>(TIMES_TO_RUN_ALL_TESTS)},
				{typeof(CachedReflectionTest), new List<long>(TIMES_TO_RUN_ALL_TESTS)},
				{typeof(SimpleReflectionTest), new List<long>(TIMES_TO_RUN_ALL_TESTS)},
			};

			for(var testSuitRunCount = 0; testSuitRunCount < TIMES_TO_RUN_ALL_TESTS; testSuitRunCount++)
			{
				Console.WriteLine($"Running {testSuitRunCount + 1}/{TIMES_TO_RUN_ALL_TESTS}");

				runs[typeof(OriginalTest)].Add(RunTest<OriginalTest>(TIMES_TO_RUN_EACH_TEST));
				runs[typeof(EmitILTest)].Add(RunTest<EmitILTest>(TIMES_TO_RUN_EACH_TEST));
				runs[typeof(DynamicTest)].Add(RunTest<DynamicTest>(TIMES_TO_RUN_EACH_TEST));
				runs[typeof(CachedReflectionTest)].Add(RunTest<CachedReflectionTest>(TIMES_TO_RUN_EACH_TEST));
				runs[typeof(SimpleReflectionTest)].Add(RunTest<SimpleReflectionTest>(TIMES_TO_RUN_EACH_TEST));

				Console.WriteLine();
			}

			Console.WriteLine();
			Console.WriteLine("Results");
			Console.WriteLine("=======");

			var results = runs.ToDictionary(k => k.Key, v => v.Value.Average(i => i)).OrderBy(i => i.Value).ToArray();
			var fastestTime = results[0].Value;

			foreach(var result in results)
			{
				var slower = 1.0 / fastestTime * result.Value;

				if(Math.Abs(slower - 1.0) < 0.01)
				{
					Console.WriteLine("{0,-20}\t{1:N0}\t(= time)", result.Key.Name, result.Value);
				}
				else
				{
					Console.WriteLine("{0,-20}\t{1:N0}\t({2:N1}x slower)", result.Key.Name, result.Value, slower);
				}
			}
		}

		private static long RunTest<T>(int timesToRun) where T : ITest, new()
		{
			GC.Collect();
			GC.WaitForPendingFinalizers();

			var tester = new T();
			var sw = new Stopwatch();

			Console.Write("Running {0,-20}... ", typeof(T).Name);

			sw.Start();

			for(var ct = 0; ct < timesToRun; ct++)
			{
				tester.Run(SUBJECT_COUNT);
			}

			sw.Stop();

			Console.WriteLine("done in {0:N0} ms", sw.ElapsedMilliseconds);

			GC.Collect();
			GC.WaitForPendingFinalizers();

			return sw.ElapsedMilliseconds;
		}
	}
}

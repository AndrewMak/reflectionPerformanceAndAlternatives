using System;

namespace ReflectionPerformanceAndAlternatives.Tests
{
	public class OriginalTest : ITest
	{

		public void Run(int subjectCount)
		{
			var rand = new Random();

			for(var ct = 0; ct < subjectCount; ct++)
			{
				var subject = CreateSubject(rand.Next(0, subjectCount), "Name " + rand.Next(0, subjectCount), DateTime.Today.AddMonths(-rand.Next(13, subjectCount)));

				if(CalculateSubjectAge(subject) < 1)
				{
					throw new ArgumentOutOfRangeException();
				}
			}
		}

		public TestSubject CreateSubject(int id, string name, DateTime birthday)
		{
			var subject = new TestSubject
			{
				Id = id,
				Name = name,
				Birthday = birthday
			};

			return subject;
		}

		public int CalculateSubjectAge(object subject)
		{
			return ((TestSubject)subject).CalculateAge();
		}
	}
}

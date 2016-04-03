using System;

namespace ReflectionPerformanceAndAlternatives.Tests
{
	public class DynamicTest : ITest
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
			dynamic newSubject = Activator.CreateInstance<TestSubject>();

			newSubject.Id = id;
			newSubject.Name = name;
			newSubject.Birthday = birthday;

			return newSubject;
		}

		public int CalculateSubjectAge(object subject)
		{
			dynamic dynamicSubject = subject;

			return dynamicSubject.CalculateAge();
		}
	}
}

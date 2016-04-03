using System;
using System.Reflection;

namespace ReflectionPerformanceAndAlternatives.Tests
{
	public class SimpleReflectionTest : ITest
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
			var newSubject = Activator.CreateInstance<TestSubject>();

			newSubject.GetType().GetTypeInfo().GetDeclaredProperty("Id").SetValue(newSubject, id);
			newSubject.GetType().GetTypeInfo().GetDeclaredProperty("Name").SetValue(newSubject, name);
			newSubject.GetType().GetTypeInfo().GetDeclaredProperty("Birthday").SetValue(newSubject, birthday);

			return newSubject;
		}

		public int CalculateSubjectAge(object subject)
		{
			return (int)subject.GetType().GetTypeInfo().GetDeclaredMethod("CalculateAge").Invoke(subject, null);
		}
	}
}

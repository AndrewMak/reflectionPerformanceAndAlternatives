using System;

namespace ReflectionPerformanceAndAlternatives.Tests
{
	public interface ITest
	{

		void Run(int subjectCount);

		TestSubject CreateSubject(int id, string name, DateTime birthday);

		int CalculateSubjectAge(object subject);

	}
}

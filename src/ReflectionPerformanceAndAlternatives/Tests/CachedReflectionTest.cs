using System;
using System.Reflection;

namespace ReflectionPerformanceAndAlternatives.Tests
{
	public class CachedReflectionTest : ITest
	{

		private PropertyInfo _idProperty;
		private PropertyInfo _nameProperty;
		private PropertyInfo _birthdayProperty;
		private MethodInfo _calculateAgeMethod;

		public void Run(int subjectCount)
		{
			var typeInfo = typeof(TestSubject).GetTypeInfo();

			_idProperty = typeInfo.GetDeclaredProperty("Id");
			_nameProperty = typeInfo.GetDeclaredProperty("Name");
			_birthdayProperty = typeInfo.GetDeclaredProperty("Birthday");
			_calculateAgeMethod = typeInfo.GetDeclaredMethod("CalculateAge");

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

			_idProperty.SetValue(newSubject, id);
			_nameProperty.SetValue(newSubject, name);
			_birthdayProperty.SetValue(newSubject, birthday);

			return newSubject;
		}

		public int CalculateSubjectAge(object subject)
		{
			return (int)_calculateAgeMethod.Invoke(subject, null);
		}
	}
}

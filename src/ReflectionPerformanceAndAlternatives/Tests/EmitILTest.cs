using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace ReflectionPerformanceAndAlternatives.Tests
{
	public class EmitILTest : ITest
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

		private Func<TestSubject> _constructor;
		private Action<TestSubject, int, string, DateTime> _setter;
		private Func<TestSubject, int> _calculateAge;

		public TestSubject CreateSubject(int id, string name, DateTime birthday)
		{
			if(_constructor == null)
			{
				var testSubjectType = typeof(TestSubject);
				var testSubjectTypeInfo = testSubjectType.GetTypeInfo();
				var dm = new DynamicMethod("CreateInstance", typeof(TestSubject), new Type[] { });
				var il = dm.GetILGenerator();

				il.Emit(OpCodes.Newobj, testSubjectTypeInfo.DeclaredConstructors.First());
				il.Emit(OpCodes.Ret);

				_constructor = (Func<TestSubject>)dm.CreateDelegate(typeof(Func<TestSubject>));

				dm = new DynamicMethod("SetProperties", null, new[] { typeof(TestSubject), typeof(int), typeof(string), typeof(DateTime) });
				il = dm.GetILGenerator();

				il.Emit(OpCodes.Ldarg_0);
				il.Emit(OpCodes.Ldarg_1);
				il.Emit(OpCodes.Callvirt, testSubjectTypeInfo.GetDeclaredProperty("Id").GetSetMethod());
				il.Emit(OpCodes.Ldarg_0);
				il.Emit(OpCodes.Ldarg_2);
				il.Emit(OpCodes.Callvirt, testSubjectTypeInfo.GetDeclaredProperty("Name").GetSetMethod());
				il.Emit(OpCodes.Ldarg_0);
				il.Emit(OpCodes.Ldarg_3);
				il.Emit(OpCodes.Callvirt, testSubjectTypeInfo.GetDeclaredProperty("Birthday").GetSetMethod());
				il.Emit(OpCodes.Ret);

				_setter = (Action<TestSubject, int, string, DateTime>)dm.CreateDelegate(typeof(Action<TestSubject, int, string, DateTime>));

				dm = new DynamicMethod("CalculateAge", typeof(int), new[] { testSubjectType });
				il = dm.GetILGenerator();

				il.Emit(OpCodes.Ldarg_0);
				il.Emit(OpCodes.Call, testSubjectTypeInfo.GetDeclaredMethod("CalculateAge"));
				il.Emit(OpCodes.Ret);

				_calculateAge = (Func<TestSubject, int>)dm.CreateDelegate(typeof(Func<TestSubject, int>));
			}

			var testSubject = _constructor();

			_setter(testSubject, id, name, birthday);

			return testSubject;
		}

		public int CalculateSubjectAge(object subject)
		{
			return _calculateAge((TestSubject)subject);
		}
	}
}

using System;

namespace ReflectionPerformanceAndAlternatives
{
    public class TestSubject
    {

	    public int Id { get; set; }

	    public string Name { get; set; }

	    public DateTime Birthday { get; set; }

	    public int CalculateAge()
	    {
			var age = DateTime.Today.Year - Birthday.Year;

		    if(Birthday > DateTime.Today.AddYears(-age))
		    {
			    age--;
		    }

			return age;
	    }

    }
}

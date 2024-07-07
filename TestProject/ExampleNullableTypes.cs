using YngveHestem.GenericParameterCollection;

namespace TestProject.ExampleNullableTypes
{
    public class ExampleNullableTypesClass
	{
        public ParameterCollection[] GetExampleParameters()
        {
            var tasks = new List<Task>
                {
                    new Task
                    {
                        TaskName = "Walk the dog",
                        Added = DateTime.Now,
                        StartTime = DateTime.Now.AddHours(5),
                    },
                    new Task
                    {
                        TaskName = "Homework",
                        Added = DateTime.Now
                    },
                    new Task
                    {
                        TaskName = "Watch TV",
                        Added = DateTime.Now,
                        StartTime = DateTime.Now.AddHours(-5),
                        EndTime = DateTime.Now.AddMinutes(-5)
                    }
                };

            return tasks.Select(t => ParameterCollection.FromObject(t)).ToArray();
        }

        public List<Task> GetAsList(IEnumerable<ParameterCollection> parameters)
        {
            return parameters.Select(p => p.ToObject<Task>()).ToList();
        }
    }

    [AttributeConvertible]
    public class Task
    {
        [ParameterProperty]
        public string TaskName { get; set; }

        [ParameterProperty]
        public DateTime Added { get; set; }

        [ParameterProperty]
        public DateTime? StartTime { get; set; }

        [ParameterProperty]
        public DateTime? EndTime { get; set; }


    }
}


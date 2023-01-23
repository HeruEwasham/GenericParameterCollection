// See https://aka.ms/new-console-template for more information
using YngveHestem.GenericParameterCollection;

Console.WriteLine("Hello, World!");

var parameters = new ParameterCollection();

parameters.Add("Parameter 1 (string)", "Dette er en test.");
parameters.Add("Parameter 2 (string, multiline)", "Dette er en test." + Environment.NewLine + "Dette er en ny linje", true);
parameters.Add("Parameter 3 (bool)", true);
parameters.Add("Parameter 4 (datetime)", new DateTime(2022, 05, 14, 15, 22, 5));
parameters.Add("Parameter 4 (date)", new DateTime(2022, 05, 17), true);
parameters.Add("Parameter 5 (collection)", new ParameterCollection
{
    { "x", 20 },
    { "y", 50 },
    { "er tall", true }
});

Console.WriteLine("Parameters: " + parameters);

Console.WriteLine(Environment.NewLine + Environment.NewLine);

var json = parameters.ToJson();

Console.WriteLine("JSON: " + json);

Console.WriteLine(Environment.NewLine + Environment.NewLine);

var parameters2 = ParameterCollection.FromJson(json);

Console.WriteLine("Parameters from json: " + parameters2);
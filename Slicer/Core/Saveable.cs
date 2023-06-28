using System;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Slicer.Core
{
	public class Saveable : Attribute
	{
		public Saveable() { }

        public static void SaveAs(string fileName)
        {
            Assembly assembly = Assembly.GetExecutingAssembly(); 
            Type[] types = assembly.GetTypes();

            var saveableProperties = types.SelectMany(t => t.GetProperties())
                                          .Where(p => Attribute.IsDefined(p, typeof(Saveable)));

            using (StreamWriter writer = new StreamWriter(fileName))
            {
                foreach (var property in saveableProperties)
                {
                    string line = $"{property.Name}: {property.GetValue(null)}";
                    writer.WriteLine(line);
                }
            }
        }

        public static void LoadFrom(string fileName)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Type[] types = assembly.GetTypes();

            var saveableProperties = types.SelectMany(t => t.GetProperties())
                                          .Where(p => Attribute.IsDefined(p, typeof(Saveable)));

            string[] lines = File.ReadAllLines(fileName);

            foreach (string line in lines)
            {
                string[] parts = line.Split(':');
                if (parts.Length != 2)
                    continue;

                string propertyName = parts[0].Trim();
                string propertyValue = parts[1].Trim();

                var property = saveableProperties.FirstOrDefault(p => p.Name == propertyName);
                if (property != null)
                {
                    object value = Convert.ChangeType(propertyValue, property.PropertyType);
                    property.SetValue(null, value);
                }
            }
        }
    }
}


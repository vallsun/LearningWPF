using System.Text.Json.Serialization;
using DevelopmentCommon.Common;

namespace SerializeTestDriver
{
	public class Program
	{
		static void Main(string[] args)
		{
			var serializer = new JsonSerializer();
			var customers = new Person[]
{
				new Person() { Name = "山田", Age = 29 },
				new Person() { Name = "鈴木", Age = 22 }
};
			serializer.Save(@"C:\Users\godva", "Sample.json", customers);
		}

		private class Person
		{
			public string Name { get; set; }

			/// <summary>
			/// 年齢
			/// </summary>
			/// <remarks>
			/// JsonIgnoreの属性を設定することでシリアライズ対象から外すことができる
			/// </remarks>
			[JsonIgnore]
			public int Age { get; set; }
			public override string ToString() => $"Name:{Name}, Age:{Age}";
		}
	}
}

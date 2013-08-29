using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace ObjectDeepCopy
{
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    [Serializable]
    public class Phone
    {
        public string No { get; set; }
    }

    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    [Serializable]
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }

        public Phone Phone { get; set; }
        public List<int> Integers { get; set; }
    }

    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    [Serializable]
    public struct SPerson
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }

        public Phone Phone { get; set; }
        public List<int> Integers { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=================================================================");
            Console.WriteLine("====================== Without Deep Clone =======================");
            Console.WriteLine("=================================================================");
            Console.Write(Environment.NewLine);
            WithoutDeepClone();
            Console.ReadLine();

            Console.WriteLine("=================================================================");
            Console.WriteLine("======================== With Deep Clone ========================");
            Console.WriteLine("=================================================================");
            Console.Write(Environment.NewLine);
            WithDeepClone();
            Console.ReadLine();

            Console.WriteLine("=================================================================");
            Console.WriteLine("=================== With Protobuf Deep Clone ====================");
            Console.WriteLine("=================================================================");
            Console.Write(Environment.NewLine);
            WithProtobufDeepClone();
            Console.ReadLine();


            Stopwatch sw = Stopwatch.StartNew();
            for (int i = 0; i < 1000; i++)
            {
                WithDeepClone();
            }
            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds);
            Console.ReadLine();

            sw.Restart();
            for (int i = 0; i < 1000; i++)
            {
                WithProtobufDeepClone();
            }
            Console.WriteLine(sw.ElapsedMilliseconds);
            Console.ReadLine();
        }

        static void WithoutDeepClone()
        {
            Person person = new Person
            {
                Id = 1,
                Name = "Tugberk",
                Surname = "Ugurlu",
                Age = 26,
                Phone = new Phone { No = "123456789" },
                Integers = new List<int> { 1, 3, 5 }
            };

            SPerson sPerson = new SPerson
            {
                Id = 1,
                Name = "Tugberk",
                Surname = "Ugurlu",
                Age = 26,
                Phone = new Phone { No = "123456789" },
                Integers = new List<int> { 1, 3, 5 }
            };

            Person person2 = person;
            SPerson sPerson2 = sPerson;

            Console.WriteLine("Should write 'Ali Tugberk'");
            person2.Name = "Ali Tugberk";
            Console.WriteLine(person.Name);
            Console.WriteLine("=================================================================");
            Console.Write(Environment.NewLine);

            Console.WriteLine("Should write 'Tugberk'");
            sPerson2.Name = "Ali Tugberk";
            Console.WriteLine(sPerson.Name);
            Console.WriteLine("=================================================================");
            Console.Write(Environment.NewLine);

            Console.WriteLine("Should write '27'");
            person.Age = 27;
            Console.WriteLine(person2.Age);
            Console.WriteLine("=================================================================");
            Console.Write(Environment.NewLine);

            Console.WriteLine("Should write '26'");
            sPerson.Age = 27;
            Console.WriteLine(sPerson2.Age);
            Console.WriteLine("=================================================================");
            Console.Write(Environment.NewLine);

            Console.WriteLine("Should write '987654321'");
            person2.Phone.No = "987654321";
            Console.WriteLine(person.Phone.No);
            Console.WriteLine("=================================================================");
            Console.Write(Environment.NewLine);

            Console.WriteLine("Should write '987654321'");
            sPerson2.Phone.No = "987654321";
            Console.WriteLine(sPerson.Phone.No);
            Console.WriteLine("=================================================================");
            Console.Write(Environment.NewLine);

            Console.WriteLine("Should write '2'");
            sPerson2.Integers.RemoveAt(2);
            Console.WriteLine(sPerson.Integers.Count);
            Console.WriteLine("=================================================================");
            Console.Write(Environment.NewLine);

            Console.WriteLine("Should write '2'");
            person2.Integers.RemoveAt(2);
            Console.WriteLine(person.Integers.Count);
            Console.WriteLine("=================================================================");
            Console.Write(Environment.NewLine);
        }

        static void WithDeepClone()
        {
            // ref: http://stackoverflow.com/questions/78536/deep-cloning-objects-in-c-sharp

            Person person = new Person
            {
                Id = 1,
                Name = "Tugberk",
                Surname = "Ugurlu",
                Age = 26,
                Phone = new Phone { No = "123456789" },
                Integers = new List<int> { 1, 3, 5 }
            };

            SPerson sPerson = new SPerson
            {
                Id = 1,
                Name = "Tugberk",
                Surname = "Ugurlu",
                Age = 26,
                Phone = new Phone { No = "123456789" },
                Integers = new List<int> { 1, 3, 5 }
            };

            Person person2 = ObjectCopier.Clone(person);
            SPerson sPerson2 = ObjectCopier.Clone(sPerson);

            Console.WriteLine("Should write 'Tugberk'");
            person2.Name = "Ali Tugberk";
            Console.WriteLine(person.Name);
            Console.WriteLine("=================================================================");
            Console.Write(Environment.NewLine);

            Console.WriteLine("Should write 'Tugberk'");
            sPerson2.Name = "Ali Tugberk";
            Console.WriteLine(sPerson.Name);
            Console.WriteLine("=================================================================");
            Console.Write(Environment.NewLine);

            Console.WriteLine("Should write '26'");
            person.Age = 27;
            Console.WriteLine(person2.Age);
            Console.WriteLine("=================================================================");
            Console.Write(Environment.NewLine);

            Console.WriteLine("Should write '26'");
            sPerson.Age = 27;
            Console.WriteLine(sPerson2.Age);
            Console.WriteLine("=================================================================");
            Console.Write(Environment.NewLine);

            Console.WriteLine("Should write '123456789'");
            person2.Phone.No = "987654321";
            Console.WriteLine(person.Phone.No);
            Console.WriteLine("=================================================================");
            Console.Write(Environment.NewLine);

            Console.WriteLine("Should write '123456789'");
            sPerson2.Phone.No = "987654321";
            Console.WriteLine(sPerson.Phone.No);
            Console.WriteLine("=================================================================");
            Console.Write(Environment.NewLine);

            Console.WriteLine("Should write '3'");
            sPerson2.Integers.RemoveAt(2);
            Console.WriteLine(sPerson.Integers.Count);
            Console.WriteLine("=================================================================");
            Console.Write(Environment.NewLine);

            Console.WriteLine("Should write '3'");
            person2.Integers.RemoveAt(2);
            Console.WriteLine(person.Integers.Count);
            Console.WriteLine("=================================================================");
            Console.Write(Environment.NewLine);
        }

        static void WithProtobufDeepClone()
        {
            //ref: http://stackoverflow.com/questions/852064/faster-deep-cloning

            Person person = new Person
            {
                Id = 1,
                Name = "Tugberk",
                Surname = "Ugurlu",
                Age = 26,
                Phone = new Phone { No = "123456789" },
                Integers = new List<int> { 1, 3, 5 }
            };

            SPerson sPerson = new SPerson
            {
                Id = 1,
                Name = "Tugberk",
                Surname = "Ugurlu",
                Age = 26,
                Phone = new Phone { No = "123456789" },
                Integers = new List<int> { 1, 3, 5 }
            };

            Person person2 = Serializer.DeepClone(person);
            SPerson sPerson2 = Serializer.DeepClone(sPerson);

            Console.WriteLine("Should write 'Tugberk'");
            person2.Name = "Ali Tugberk";
            Console.WriteLine(person.Name);
            Console.WriteLine("=================================================================");
            Console.Write(Environment.NewLine);

            Console.WriteLine("Should write 'Tugberk'");
            sPerson2.Name = "Ali Tugberk";
            Console.WriteLine(sPerson.Name);
            Console.WriteLine("=================================================================");
            Console.Write(Environment.NewLine);

            Console.WriteLine("Should write '26'");
            person.Age = 27;
            Console.WriteLine(person2.Age);
            Console.WriteLine("=================================================================");
            Console.Write(Environment.NewLine);

            Console.WriteLine("Should write '26'");
            sPerson.Age = 27;
            Console.WriteLine(sPerson2.Age);
            Console.WriteLine("=================================================================");
            Console.Write(Environment.NewLine);

            Console.WriteLine("Should write '123456789'");
            person2.Phone.No = "987654321";
            Console.WriteLine(person.Phone.No);
            Console.WriteLine("=================================================================");
            Console.Write(Environment.NewLine);

            Console.WriteLine("Should write '123456789'");
            sPerson2.Phone.No = "987654321";
            Console.WriteLine(sPerson.Phone.No);
            Console.WriteLine("=================================================================");
            Console.Write(Environment.NewLine);

            Console.WriteLine("Should write '3'");
            sPerson2.Integers.RemoveAt(2);
            Console.WriteLine(sPerson.Integers.Count);
            Console.WriteLine("=================================================================");
            Console.Write(Environment.NewLine);

            Console.WriteLine("Should write '3'");
            person2.Integers.RemoveAt(2);
            Console.WriteLine(person.Integers.Count);
            Console.WriteLine("=================================================================");
            Console.Write(Environment.NewLine);
        }
    }

    public class ObjectCopier
    {
        public static T Clone<T>(T source)
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", "source");
            }

            // Don't serialize a null object, simply return the default for that object
            if (object.ReferenceEquals(source, null))
            {
                return default(T);
            }

            IFormatter formatter = new BinaryFormatter();

            using (Stream stream = new MemoryStream())
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }
    }
}
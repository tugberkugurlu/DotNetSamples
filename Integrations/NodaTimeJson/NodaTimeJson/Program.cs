using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NodaTime;
using NodaTime.Serialization.JsonNet;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodaTimeJson
{
    class Program
    {
        /// <remarks>
        /// http://nodatime.org/1.2.x/userguide/type-choices.html
        /// http://nodatime.org/1.3.x/userguide/serialization.html
        /// http://nodatime.org/1.3.x/userguide/core-types.html
        /// https://github.com/nodatime/nodatime/issues/406
        /// </remarks>
        static void Main(string[] args)
        {
            var json = GetContent();
            var serializerSettings = new JsonSerializerSettings();
            serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            serializerSettings.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);

            // deserialize
            var tripCreation = JsonConvert.DeserializeObject<TripCreationModel>(json, serializerSettings);

            // serialize
            var london = DateTimeZoneProviders.Tzdb["Europe/London"];
            var startInstant = Instant.FromDateTimeUtc(DateTime.UtcNow.AddDays(-60));
            var endInstant = startInstant.Plus(Duration.FromHours(1));
            var startLondonTime = startInstant.InZone(london);
            var endLondonTime = endInstant.InZone(london);

            var trip = new Trip
            {
                Name = "Foo",
                StatedAt = startLondonTime,
                EndedAt = endLondonTime
            };

            var tripAsJson = JsonConvert.SerializeObject(trip, serializerSettings);

            // LocalDate
            var person = new Person
            {
                Name = "Tugberk",
                BirthDate = new LocalDate(1970, 10, 20)
            };

            var personAsJson = JsonConvert.SerializeObject(person, serializerSettings);

            // Duration
            var lap = new Lap
            {
                Sequance = 1,
                Time = Duration.FromMinutes(1).Plus(Duration.FromSeconds(13)).Plus(Duration.FromMilliseconds(793))
            };

            var dropTheHour = lap.Time.ToString("m:ss.fff", CultureInfo.InvariantCulture);
            var lapAsJson = JsonConvert.SerializeObject(lap, serializerSettings);
        }

        private static string GetContent()
        {
            using (var stream = File.OpenRead(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "file.json")))
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        public class Lap
        {
            public byte Sequance { get; set; }
            public Duration Time { get; set; }
        }

        public class Person
        {
            public string Name { get; set; }
            public LocalDate BirthDate { get; set; }
        }

        public class Trip
        {
            public string Name { get; set; }
            public ZonedDateTime StatedAt { get; set; }
            public ZonedDateTime EndedAt { get; set; }
        }

        public class TripCreationModel
        {
            public string Name { get; set; }
            public Instant StartedAt { get; set; }
            public Instant EndedAt { get; set; }
        }
    }
}

using Funcky.Monads;
using Funcky.Xunit;
using Newtonsoft.Json;
using Xunit;

namespace Funcky.NewtonsoftJson.Test
{
    public sealed class JsonOptionConverterTest
    {
        private static readonly JsonSerializerSettings Settings = new JsonSerializerSettings().AddOptionConverter();

        [Fact]
        public void SerializesNoneAsNull()
        {
            const string expectedJson = "null";
            var json = JsonConvert.SerializeObject(Option<string>.None(), Settings);
            Assert.Equal(expectedJson, json);
        }

        [Fact]
        public void SerializesNoneAsNullWhenNested()
        {
            const string expectedJson = @"{""BloodType"":""B-"",""EmergencyContact"":null}";
            var @object = new MedicalId(bloodType: "B-", emergencyContact: Option<Person>.None());
            var json = JsonConvert.SerializeObject(@object, Settings);
            Assert.Equal(expectedJson, json);
        }

        [Fact]
        public void SerializesInnerObjectWhenSomeIsGiven()
        {
            const string expectedJson = @"{""FirstName"":""Peter"",""LastName"":""Pan""}";
            var json = JsonConvert.SerializeObject(Option.Some(new Person("Peter", "Pan")), Settings);
            Assert.Equal(expectedJson, json);
        }

        [Fact]
        public void SerializesInnerObjectWhenNested()
        {
            const string expectedJson = @"{""BloodType"":""B-"",""EmergencyContact"":{""FirstName"":""Peter"",""LastName"":""Pan""}}";
            var @object = new MedicalId(bloodType: "B-", emergencyContact: Option.Some(new Person("Peter", "Pan")));
            var json = JsonConvert.SerializeObject(@object, Settings);
            Assert.Equal(expectedJson, json);
        }

        [Fact]
        public void SerializesInnerIntegerWhenSomeIsGiven()
        {
            const string expectedJson = @"42";
            var json = JsonConvert.SerializeObject(Option.Some(42), Settings);
            Assert.Equal(expectedJson, json);
        }

        [Fact]
        public void DeserializesNoneFromNull()
        {
            const string json = @"null";
            FunctionalAssert.IsNone(JsonConvert.DeserializeObject<Option<string>>(json, Settings));
        }

        [Fact]
        public void DeserializesSomeFromObject()
        {
            const string json = @"{""FirstName"":""Peter"",""LastName"":""Pan""}";
            var expectedObject = new Person("Peter", "Pan");
            FunctionalAssert.IsSome(expectedObject, JsonConvert.DeserializeObject<Option<Person>>(json, Settings));
        }

        [Fact]
        public void DeserializesSomeFromNumber()
        {
            const string json = @"42";
            const int expectedInteger = 42;
            FunctionalAssert.IsSome(expectedInteger, JsonConvert.DeserializeObject<Option<int>>(json, Settings));
        }

        [Fact]
        public void DeserializesNoneFromNullWhenNested()
        {
            const string json = @"{""BloodType"":""B-"",""EmergencyContact"":null}";
            var expectedObject = new MedicalId(bloodType: "B-", emergencyContact: Option<Person>.None());
            Assert.Equal(expectedObject, JsonConvert.DeserializeObject<MedicalId>(json, Settings));
        }

        [Fact]
        public void DeserializesInnerObjectWhenNested()
        {
            const string json = @"{""BloodType"":""B-"",""EmergencyContact"":{""FirstName"":""Peter"",""LastName"":""Pan""}}";
            var expectedObject = new MedicalId(bloodType: "B-", emergencyContact: Option.Some(new Person("Peter", "Pan")));
            Assert.Equal(expectedObject, JsonConvert.DeserializeObject<MedicalId>(json, Settings));
        }

        [Equals(DoNotAddEqualityOperators = true)]
        private sealed class Person
        {
            public Person(string firstName, string lastName)
            {
                FirstName = firstName;
                LastName = lastName;
            }

            private Person()
            {
            }

            public string FirstName { get; set; } = null!;

            public string LastName { get; set; } = null!;
        }

        [Equals(DoNotAddEqualityOperators = true)]
        private sealed class MedicalId
        {
            public MedicalId(string bloodType, Option<Person> emergencyContact)
            {
                BloodType = bloodType;
                EmergencyContact = emergencyContact;
            }

            private MedicalId()
            {
            }

            public string BloodType { get; set; } = null!;

            public Option<Person> EmergencyContact { get; set; }
        }
    }
}

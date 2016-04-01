using System;
using System.Collections.Generic;
using System.Linq;
using Wintellect.PowerCollections;

namespace Collection_of_Persons
{
    public class PersonCollection : IPersonCollection
    {
        private Dictionary<string, Person> persons =
            new Dictionary<string, Person>();

        private Dictionary<string, SortedSet<Person>> personsByEmail =
            new Dictionary<string, SortedSet<Person>>();

        private Dictionary<string, SortedSet<Person>> personsByNameAndTown =
            new Dictionary<string, SortedSet<Person>>();

        private OrderedDictionary<int, SortedSet<Person>> personsByAge = 
            new OrderedDictionary<int, SortedSet<Person>>();

        private Dictionary<string, OrderedDictionary<int, SortedSet<Person>>> personsByAgeAndTown =
            new Dictionary<string, OrderedDictionary<int, SortedSet<Person>>>();

        public bool AddPerson(string email, string name, int age, string town)
        {
            if (this.persons.ContainsKey(email))
            {
                return false;
            }

            var person = new Person
            {
                Email = email,
                Name = name,
                Age = age,
                Town = town
            };

            this.persons.Add(email, person);

            this.personsByEmail.AppendValueToKey(this.ExtractDomain(email), person);

            this.personsByNameAndTown.AppendValueToKey(this.CombineNameAndTown(name, town), person);

            this.personsByAge.AppendValueToKey(age, person);

            this.personsByAgeAndTown.EnsureKeyExists(town);
            this.personsByAgeAndTown[town].AppendValueToKey(age, person);

            return true;
        }

        public int Count
        {
            get { return this.persons.Count; }
        }

        public Person FindPerson(string email)
        {
            Person person;

            this.persons.TryGetValue(email, out person);

            return person;
        }

        public bool DeletePerson(string email)
        {
            var person = this.FindPerson(email);
            if (person == null)
            {
                return false;
            }

            this.persons.Remove(email);

            var domain = this.ExtractDomain(email);
            this.personsByEmail[domain].Remove(person);

            var combine = this.CombineNameAndTown(person.Name, person.Town);
            this.personsByNameAndTown[combine].Remove(person);

            this.personsByAge[person.Age].Remove(person);

            this.personsByAgeAndTown[person.Town][person.Age].Remove(person);

            return true;
        }

        public IEnumerable<Person> FindPersons(string emailDomain)
        {
            return this.personsByEmail.GetValuesForKey(emailDomain);
        }

        private string ExtractDomain(string email)
        {
            var domain = email.Split('@')[1];

            return domain;
        }

        private string CombineNameAndTown(string name, string town)
        {
            var combine = name + "|!|" + town;

            return combine;
        }

        public IEnumerable<Person> FindPersons(string name, string town)
        {
            return this.personsByNameAndTown.GetValuesForKey(this.CombineNameAndTown(name, town));
        }

        public IEnumerable<Person> FindPersons(int startAge, int endAge)
        {
            var personByAge = this.personsByAge.Range(startAge, true, endAge, true);

            return personByAge.SelectMany(a => a.Value);
        }

        public IEnumerable<Person> FindPersons(
            int startAge, int endAge, string town)
        {
            if (!this.personsByAgeAndTown.ContainsKey(town))
            {
                yield break;
            }

            var personByAgeAndTown = this.personsByAgeAndTown[town]
                .Range(startAge, true, endAge, true);

            foreach (var person in personByAgeAndTown)
            {
                foreach (var personByAge in person.Value)
                {
                    yield return personByAge;
                }
            }
        }
    }
}

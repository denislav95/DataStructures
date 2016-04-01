namespace Collection_of_Persons
{
    using System.Collections.Generic;
    using Wintellect.PowerCollections;

    public class PersonCollection : IPersonCollection
    {
        private Dictionary<string, Person> personsByEmail =
           new Dictionary<string, Person>();

        private Dictionary<string, SortedSet<Person>> personsByEmailDomain =
            new Dictionary<string, SortedSet<Person>>();

        private Dictionary<string, SortedSet<Person>> personsByNameAndTown =
            new Dictionary<string, SortedSet<Person>>();

        private OrderedDictionary<int, SortedSet<Person>> personsByAge =
            new OrderedDictionary<int, SortedSet<Person>>();

        private Dictionary<string, OrderedDictionary<int, SortedSet<Person>>> personsByTownAndAge =
            new Dictionary<string, OrderedDictionary<int, SortedSet<Person>>>();

        public bool AddPerson(string email, string name, int age, string town)
        {
            if (this.FindPerson(email) != null)
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

            this.personsByEmail.Add(email, person);

            var emailDomain = this.ExtractEmailDomain(email);
            this.personsByEmailDomain.AppendValueToKey(emailDomain, person);

            var nameAndTown = this.CombineNameAndTown(name, town);
            this.personsByNameAndTown.AppendValueToKey(nameAndTown, person);

            this.personsByAge.AppendValueToKey(age, person);
            
            this.personsByTownAndAge.EnsureKeyExists(town);
            this.personsByTownAndAge[town].AppendValueToKey(age, person);

            return true;
        }

        public int Count
        {
            get { return this.personsByEmail.Count; }
        }

        public Person FindPerson(string email)
        {
            Person person;
            var personExist = this.personsByEmail.TryGetValue(email, out person);

            return person;
        }

        public bool DeletePerson(string email)
        {
            var person = this.FindPerson(email);
            if (person == null)
            {
                return false;
            }

            var personDeleted = this.personsByEmail.Remove(email);

            var emailDomain = this.ExtractEmailDomain(email);
            this.personsByEmailDomain[emailDomain].Remove(person);

            var nameAndTown = this.CombineNameAndTown(person.Name, person.Town);
            this.personsByNameAndTown[nameAndTown].Remove(person);

            this.personsByAge[person.Age].Remove(person);

            this.personsByTownAndAge[person.Town][person.Age].Remove(person);

            return true;
        }

        public string ExtractEmailDomain(string email)
        {
            var domain = email.Split('@')[1];

            return domain;
        }

        public IEnumerable<Person> FindPersons(string emailDomain)
        {
            return this.personsByEmailDomain.GetValuesForKey(emailDomain);
        }

        public IEnumerable<Person> FindPersons(string name, string town)
        {
            var nameAndTown = this.CombineNameAndTown(name, town);

            return this.personsByNameAndTown.GetValuesForKey(nameAndTown);
        }

        private string CombineNameAndTown(string name, string town)
        {
            const string separator = "|!|";

            return name + separator + town;
        }

        public IEnumerable<Person> FindPersons(int startAge, int endAge)
        {
            var personInRange =
                this.personsByAge.Range(startAge, true, endAge, true);
            foreach (var personByAge in personInRange)
            {
                foreach (var person in personByAge.Value)
                {
                    yield return person;
                }
            }
        }

        public IEnumerable<Person> FindPersons(
            int startAge, int endAge, string town)
        {
            if (!this.personsByTownAndAge.ContainsKey(town))
            {
                yield break;
            }

            var personInRange = this.personsByTownAndAge[town]
                .Range(startAge, true, endAge, true);

            foreach (var personByAge in personInRange)
            {
                foreach (var person in personByAge.Value)
                {
                    yield return person;
                }
            }
        }
    }
}

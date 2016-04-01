using System;
using System.Collections.Generic;

namespace Collection_of_Persons
{
    public class PersonCollectionSlow : IPersonCollection
    {

        public bool AddPerson(string email, string name, int age, string town)
        {
            // TODO: implement this
            throw new NotImplementedException();
        }

        public int Count
        {
            get
            {
                // TODO: implement this
                throw new NotImplementedException();
            }
        }

        public Person FindPerson(string email)
        {
            // TODO: implement this
            throw new NotImplementedException();
        }

        public bool DeletePerson(string email)
        {
            // TODO: implement this
            throw new NotImplementedException();
        }

        public IEnumerable<Person> FindPersons(string emailDomain)
        {
            // TODO: implement this
            throw new NotImplementedException();
        }

        public IEnumerable<Person> FindPersons(string name, string town)
        {
            // TODO: implement this
            throw new NotImplementedException();
        }

        public IEnumerable<Person> FindPersons(int startAge, int endAge)
        {
            // TODO: implement this
            throw new NotImplementedException();
        }

        public IEnumerable<Person> FindPersons(
            int startAge, int endAge, string town)
        {
            // TODO: implement this
            throw new NotImplementedException();
        }
    }
}

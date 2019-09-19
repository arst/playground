namespace RecursiveGenerics
{
    class PersonDummyFactory
    {
        public Person Create()
        {
            var person = PersonStarter.NewPerson
                .BuildName("Dummy")
                .BuildAge(1)
                .Build();

            return person;
        }
    }
}

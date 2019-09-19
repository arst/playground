namespace RecursiveGenerics
{
    class PersonStarter : PersonAgeBuilder<PersonStarter>
    {
        public static PersonStarter NewPerson => new PersonStarter();
    }
}

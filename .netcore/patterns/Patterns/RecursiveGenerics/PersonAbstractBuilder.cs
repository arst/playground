namespace RecursiveGenerics
{
    abstract class PersonAbstractBuilder
    {
        protected Person person;

        public PersonAbstractBuilder()
        {
            person = new Person();
        }

        public Person Build() => person;
    }
}

namespace RecursiveGenerics
{
    class PersonAgeBuilder<T> : PersonNameBuilder<PersonAgeBuilder<T>> where T: PersonAgeBuilder<T>
    {
        public T BuildAge(int age)
        {
            person.Age = age;

            return (T)this;
        }
    }
}

namespace RecursiveGenerics
{
    class PersonNameBuilder<T> : PersonAbstractBuilder where T: PersonNameBuilder<T> 
    {
        public T BuildName(string name)
        {
            person.Name = name;

            return (T)this;
        }
    }
}

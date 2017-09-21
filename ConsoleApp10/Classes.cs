namespace ConsoleApp10
{
    /// <summary>
    /// Идентификатор класса 
    /// <para>Наследует <see cref="Ident"/></para>
    /// </summary>
    class Classes : Ident
    {
        /// <summary>
        /// Конструктор идентификатора класса
        /// </summary>
        /// <param name="name">Имя идентификатора</param>
        /// <param name="_type">Тип данных</param>
        public Classes(string name, Type _type)
        {
            Name = name;
            Hash = Name.GetHashCode();
            Uses = Uses.CLASSES;
            _Type = _type;
        }
        /// <summary>
        /// Стороковое представление идентификатора
        /// </summary>
        public override string ToString()
        {
            return Name+" "+Hash+" "+Uses+" "+_Type+" ";
        }
    }
}

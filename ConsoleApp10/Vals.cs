namespace ConsoleApp10
{
    /// <summary>
    /// Идентификатор переменной 
    /// <para>Наследует <see cref="Ident"/></para>
    /// </summary>
    class Vals : Ident
    {
        /// <summary>
        /// Конструктор идентификатора переменной
        /// </summary>
        /// <param name="name">Имя идентификатора</param>
        /// <param name="_type">Тип данных</param>
        public Vals(string name, Type _type)
        {
            Name = name;
            Hash = Name.GetHashCode();
            Uses = Uses.VARS;
            _Type = _type;
        }
        /// <summary>
        /// Стороковое представление идентификатора
        /// </summary>
        public override string ToString()
        {
            return Name + " " + Hash + " " + Uses + " " + _Type + " ";
        }
    }
}

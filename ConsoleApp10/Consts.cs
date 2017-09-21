namespace ConsoleApp10
{
    /// <summary>
    /// Идентификатор постоянной
    /// <para>Наследует <see cref="Ident"/></para>
    /// </summary>
    /// <typeparam name="T">Тип хранимого значения</typeparam>
    class Consts<T>: Ident
    {
        /// <summary>
        /// Значение постоянной
        /// </summary>
        T Value;
        /// <summary>
        /// Конструктор идентификатора постоянной
        /// </summary>
        /// <param name="name">Имя идентификатора</param>
        /// <param name="_type">Тип данных</param>
        /// <param name="value">Значение постоянной</param>
        public Consts(string name, Type _type,T value)
        {
            Name = name;
            Hash = Name.GetHashCode();
            Uses = Uses.CONSTS;
            _Type = _type;
            Value = value;
        }
        /// <summary>
        /// Стороковое представление идентификатора
        /// </summary>
        public override string ToString()
        {
            return Name + " " + Hash + " " + Uses + " " + _Type + " "+Value.ToString()+" ";
        }
    }
}

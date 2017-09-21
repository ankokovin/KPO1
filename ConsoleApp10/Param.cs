namespace ConsoleApp10
{
    /// <summary>
    /// Параметр идентификатора метода <see cref="Methods"/>
    /// </summary>
    class Param
    {
        /// <summary>
        /// Тип данных параметра
        /// </summary>
        Type _type;
        /// <summary>
        /// Способ передачи параметра
        /// </summary>
        ParaType ParaType;
        /// <summary>
        /// Конструктор параметра
        /// </summary>
        /// <param name="type">Тип данных параметра</param>
        /// <param name="paraType">Способ передачи параметра</param>
        public Param(Type type, ParaType paraType)
        {
            _type = type;
            ParaType = paraType;
        }
        /// <summary>
        /// Стороковое представление параметра
        /// </summary>
        public override string ToString()
        {
            return ParaType + " " + _type;
        }
    }
}

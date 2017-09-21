namespace ConsoleApp10
{
    /// <summary>
    /// Абстрактный класс идентификаторов
    /// </summary>
    abstract class Ident
    {
        /// <summary>
        /// Имя идентификатора
        /// </summary>
        public string Name;
        /// <summary>
        /// Значение хэша идентификатора
        /// </summary>
        public int Hash;
        /// <summary>
        /// Тип использования идентификатора
        /// </summary>
        protected Uses Uses;
        /// <summary>
        /// Тип данных идентификатора
        /// </summary>
        protected Type _Type;
    }
}

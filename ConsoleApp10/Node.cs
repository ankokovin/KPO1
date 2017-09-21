namespace ConsoleApp10
{
    /// <summary>
    /// Вершина бинареного дерева <see cref="Tree"/>
    /// </summary>
    class Node
    {
        /// <summary>
        /// Идентификатор, записаный в вершину
        /// </summary>
        Ident info;
        /// <summary>
        /// Значение хэша идентификатора
        /// </summary>
        public int Hash
        {
            get
            {
                return info.Hash;
            }
        }
        /// <summary>
        /// Ссылка на левый потомок
        /// </summary>
        public Node left;
        /// <summary>
        /// Ссылка на правый потомок
        /// </summary>
        public Node right;
        /// <summary>
        /// Конструктор вершины 
        /// </summary>
        /// <param name="ident">Идентификатор вершины</param>
        public Node(Ident ident)
        {
            info = ident;
        }
        /// <summary>
        /// Строковое представление вершины
        /// </summary>
        public override string ToString()
        {
            return info.Name + " " + info.Hash;
        }
    }
}

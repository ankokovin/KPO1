namespace ConsoleApp10
{
    /// <summary>
    /// Бинарное дерево идентификаторов
    /// </summary>
    class Tree
    {
        /// <summary>
        /// Корень дерева
        /// </summary>
        Node root;
        /// <summary>
        /// Добавление идентификатора
        /// </summary>
        /// <param name="ident">Добавляемый идентификатор</param>
        public void Add(Ident ident)
        {
            AddNode(new Node(ident));
        }
        /// <summary>
        /// Добавление вершины в дерево
        /// </summary>
        /// <param name="node">Добавляемая вершина</param>
        void AddNode(Node node)
        {
            if (root == null)
            {
                root = node;
                return;
            }
            Node temp=root;
            bool stop = false;
            while (!stop)
            {
                if (node.Hash > temp.Hash)
                {
                    if (temp.right == null)
                    {
                        temp.right = node;
                        stop = true;
                    }
                    temp = temp.right;
                } else
                {
                    if (temp.left == null)
                    {
                        temp.left = node;
                        stop = true;
                    }
                    temp = temp.left;
                }
            }
        }
    }
}

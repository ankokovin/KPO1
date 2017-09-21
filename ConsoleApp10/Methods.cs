using System.Collections.Generic;

namespace ConsoleApp10
{
    /// <summary>
    /// Идентификатор метода
    /// <para>Наследует <see cref="Ident"/></para>
    /// </summary>
    class Methods : Ident
    {
        /// <summary>
        /// Список параметров метода
        /// </summary>
        List<Param> Params;
        /// <summary>
        /// Конструктор идентификатора метода
        /// </summary>
        /// <param name="name">Имя идентификатора</param>
        /// <param name="_type">Тип данных</param>
        /// <param name="param">Параметры</param>
        public Methods(string name, Type _type,params Param[] param)
        {
            Name = name;
            Hash = Name.GetHashCode();
            Uses = Uses.METHODS;
            _Type = _type;
            Params = new List<Param>(param);
        }
        /// <summary>
        /// Стороковое представление идентификатора
        /// </summary>
        public override string ToString()
        {
            string ParamsToString="";
            foreach (Param p in Params) ParamsToString += p.ToString() + " ";
            return Name + " " + Hash + " " + Uses + " " + _Type + " Params:("+ParamsToString+")";
        }
    }
}

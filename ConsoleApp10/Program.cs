using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;

namespace ConsoleApp10
{
    class Program
    {
        /// <summary>
        /// Функция считывания идентификаторов из текстового файла с именем *name*.txt
        /// <para>Строит из данных идентификаторов бинарное дерево <see cref="Tree"/></para>
        /// </summary>
        /// <param name="name">Имя файла без расширения</param>
        /// <returns>Бинарное дерево идентификаторов из файла</returns>
        static Tree ReadFromFile(string name)
        {
            Tree tree = new Tree();//создаём экземпляр дерева
            StreamReader streamReader;//"читатель"
            try
            {
                streamReader = new StreamReader(name + ".txt"); //создаём "читатель"
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
            int line = 0;
            while (!streamReader.EndOfStream)//пока не дойдём до конца файла
            {
                try
                {
                    line++;
                    string input = streamReader.ReadLine();//считываем строку
                    Ident ident = GetIdent(input);//получаем идентификатор
                    Console.WriteLine("Строка {0} Идентификатор: {1}", line, ident);
                    tree.Add(ident);//добавляем идентификатор
                }
                catch(Exception e)
                {
                    Console.WriteLine("Строка {0} Ошибка: {1}", line, e.Message);
                }
            }
            streamReader.Close();//закрываем "читатель"
            return tree;//возвращаем дерево
        }
        /// <summary>
        /// Приводит строку к соответствующему элемету <see cref="Type"/>
        /// </summary>
        /// <param name="input">Входная строка</param>
        /// <returns>Соответствующий элемент <see cref="Type"/></returns>
        static Type GetType(string input)
        {
            switch (input)
            {
                case "int":
                    return Type.int_type;
                case "float":
                    return Type.float_type;
                case "bool":
                    return Type.bool_type;
                case "char":
                    return Type.char_type;
                case "string":
                    return Type.string_type;
                case "class":
                    return Type.class_type;
            }
            throw new ArgumentException("Неизвестный тип данных: "+input);
        }
        /// <summary>
        /// Приводит строку к соответствующему элементу <see cref="ParaType"/>
        /// </summary>
        /// <param name="input">Входная строка</param>
        /// <returns>Соответствующий элемент <see cref="ParaType"/></returns>
        static ParaType GetParaType(string input)
        {
            switch (input)
            {
               case "out":return ParaType.param_out;
               case "ref":return ParaType.param_ref;
               case "":return ParaType.param_val;
            }
            throw new ArgumentException("Неизвестный тип передачи параметра "+input);
        }
        /// <summary>
        /// Приводит строку к соответствующему идентификатору <see cref="Ident"/>
        /// <para>Получаемый идентификатор имеет один из типов: <see cref="Vals"/> <see cref="Consts{T}"/>
        /// <see cref="Methods"/> <see cref="Classes"/></para>
        /// </summary>
        /// <param name="input">Входная строка</param>
        /// <returns>Идентификатор <see cref="Ident"/></returns>
        static Ident GetIdent(string input)
        {
            //регулярное выражение переменной или класса
            Regex ValOfClassCheck = new Regex("^[ ]*[a-z]+[ ]+[A-Za-z0-9]+[ ]*;[ ]*$");
            //если входная строка соответствует выражению - это класс или переменная
            if (ValOfClassCheck.IsMatch(input)) return GetValsOrClasses(input);
            //регулярное выражение константы
            Regex ConstCheck = new Regex("^[ ]*const[ ]*[a-z]+[ ]*[a-zA-Z0-9]+[ ]*=[ ]*.+[ ]*;[ ]*$");
            //если входная строка соответствует выражению - это константа
            if (ConstCheck.IsMatch(input)) return GetConst(input);
            //рег. выражение для методов
            Regex MethodCheck = new Regex("^[ ]*[a-z]+[ ]+[a-zA-Z0-9]+[ ]*" +
                "[(](([ ]*,[ ]*)?((ref[ ]+)|(out[ ]+)|([ ]*))[a-z]+[ ]+[a-zA-Z0-9]+)*[ ]*[)][ ]*;[ ]*$");
            if (MethodCheck.IsMatch(input)) return GetMethod(input);
            throw new ArgumentException("Не был обнаружен индентификатор");
        }
        /// <summary>
        /// Удаление лишних пробелов из начала и конца строки
        /// </summary>
        /// <param name="input">входная строка</param>
        static void CleanIdent(ref string input)
        {
            //удаляем лишние пробелы в начале
            while (input[0] == ' ') input = input.Substring(1, input.Length - 1);
            //удаляем лишние пробелы после ;
            while (input[input.Length - 1] == ' ') input = input.Substring(0, input.Length - 1);
            //удаляем лишние пробелы перед ;
            while (input[input.Length - 2] == ' ') input = input.Substring(0, input.Length - 2) + ';';
        }
        /// <summary>
        /// Приводит строку к соответствующему идентификатору <see cref="Methods"/>
        /// </summary>
        /// <param name="input">Входная строка</param>
        /// <returns>Соответствующий идентификатор</returns>
        static Ident GetMethod(string input)
        {
            CleanIdent(ref input);
            string[] inputs = new Regex("[ (]+").Split(input);//разделяем входные данные
            string _Type = inputs[0];//тип данных
            string Name = inputs[1];//имя
            Regex ParamsRegex = new Regex("[(][a-zA-Z 0-9,]+[)]");//рег. выражение, определающее параметры
            Match Params = ParamsRegex.Match(input);//получаем параметры (Match)
            string StParams = Params.Value;//получаем параметры (string)
            //если что-то ещё подходит под описание
            if (Params.NextMatch().Success) throw new ArgumentException("Неожиданно большое количество скобок");
            Regex ParamRegex = new Regex("([(]|[,])[ ]*[a-z]+[ ]+[a-zA-Z0-9]+[ ]*([,]|[)])");//рег. выражение для простых параметров
            Match MatchParam = ParamRegex.Match(StParams); //получаем параметры по одному
            List<Param> ParamList = new List<Param>();//список получившихся параметров
            while (MatchParam.Success)//до тех пора пока получаем совпадающие фрагменты
            {
                string Param = MatchParam.Value;
                while (Param[0] == ' '||Param[0]=='('||Param[0]==',')
                    Param = Param.Substring(1);
                while (Param[Param.Length - 1] == ' ' || Param[Param.Length - 1] == ','
                    ||Param[Param.Length-1]==')') Param = Param.Substring(0, Param.Length - 1);
                string[] tempParam = new Regex("[ ]+").Split(Param);//разделяем строку
                Type type = GetType(tempParam[0]);//тип данных метода
                ParamList.Add(new Param(type,ParaType.param_val));//добавляем получившийся параметр
                MatchParam = MatchParam.NextMatch();//переходим к следующему фрагменту
            }
            ParamRegex = new Regex("((ref[ ]+)|(out[ ]+))[a-z]+[ ]+[A-Za-z0-9]+");//рег. выражение для out и ref
            MatchParam = ParamRegex.Match(StParams);
            while (MatchParam.Success)
            {
                string Param = MatchParam.Value;
                while (Param[0] == ' ')
                    Param = Param.Substring(1);
                while (Param[Param.Length - 1] == ' ' || Param[Param.Length - 1] == ',')
                    Param = Param.Substring(0, Param.Length - 1);
                string[] tempParam = new Regex("[ ]+").Split(Param);
                ParaType paraType = GetParaType(tempParam[0]);
                Type type = GetType(tempParam[1]);
                ParamList.Add(new Param(type, paraType));
                MatchParam = MatchParam.NextMatch();
            }

            return new Methods(Name, GetType(_Type), ParamList.ToArray());
        }
        /// <summary>
        /// Приводит строку к соответствующему идентификатору <see cref="Consts{T}"/>
        /// </summary>
        /// <param name="input">Входная строка</param>
        /// <returns>Соответствующий идентификатор</returns>
        static Ident GetConst(string input)
        {
            CleanIdent(ref input);
            string[] inputs = new Regex("[ =]+").Split(input);//разделяем входные данные
            if (inputs.Length != 4) throw new ArgumentException();//длина должна быть равна 4
            inputs[3]= inputs[3].Remove(inputs[3].Length - 1, 1);//удаляем ;
            Type type = GetType(inputs[1]);//Получаем тип данных
            switch (type)
            {
                case Type.bool_type:
                    return new Consts<bool>(inputs[2], type, bool.Parse(inputs[3]));
                case Type.char_type:
                    return new Consts<char>(inputs[2], type, inputs[3][1]);
                case Type.float_type:
                    return new Consts<float>(inputs[2], type, float.Parse(inputs[3]));
                case Type.int_type:
                    return new Consts<int>(inputs[2], type, int.Parse(inputs[3]));
                case Type.string_type:
                    inputs[3] = inputs[3].Substring(1, inputs[3].Length - 2);
                    return new Consts<string>(inputs[2], type, inputs[3]);
            }
            throw new Exception("Константа имеет неизвестный тип");
        }
        /// <summary>
        /// Приводит строку к соответствующему идентификатору <see cref="Vals"/> или <see cref="Classes"/>
        /// </summary>
        /// <param name="input">Входная строка</param>
        /// <returns></returns>
        static Ident GetValsOrClasses(string input)
        {
            CleanIdent(ref input);
            string[] inputs = new Regex("[ ]+").Split(input);//разделяем входные данные
            //длина должна быть равна 2
            if (inputs.Length != 2) throw new ArgumentException("Неожиданноe количество пробелов");
            Type type = GetType(inputs[0]);//получаем тип данных
            //если тип - класс, возвращаем класс
            if (type == Type.class_type) return new Classes(inputs[1].Substring(0,inputs[1].Length-1), type); 
            //иначе возвращаем переменную
            else return new Vals(inputs[1].Substring(0,inputs[1].Length-1), type);
        }
        static void Main(string[] args)
        {
            Tree tree = ReadFromFile("Test");
            Console.ReadLine();
        }
    }
}

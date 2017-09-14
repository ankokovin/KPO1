using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ConsoleApp10
{
    enum Uses { CLASSES,CONSTS,VARS,METHODS}
    enum Type { int_type,float_type,bool_type,char_type,string_type,class_type}
    enum ParaType { param_val,param_out,param_ref}
    class Tree
    {
        Node root;
        public void Add(Ident ident)
        {
            AddNode(new Node(ident));
        }
        void AddNode(Node node)
        {
            if (root == null)
            {
                root = node;
                return;
            }
            Node temp=root;
            while (true)
            {
                if (node.Hash > temp.Hash)
                {
                    if (temp.right == null)
                    {
                        temp.right = node;
                        return;
                    }
                    temp = temp.right;
                } else
                {
                    if (temp.left == null)
                    {
                        temp.left = node;
                        return;
                    }
                    temp = temp.left;
                }
            }
        }
    }
    class Node
    {
        Ident info;
        public int Hash
        {
            get
            {
                return info.Hash;
            }
        }
        public Node left, right;
        public Node(Ident ident)
        {
            info = ident;
        }
        public override string ToString()
        {
            return info.Name + " " + info.Hash;
        }
    }
    abstract class Ident
    {
        public string Name;
        public int Hash;
        protected Uses Uses;
        protected Type _Type;
    }
    class Classes : Ident
    {
        public Classes(string name,Uses uses, Type _type)
        {
            Name = name;
            Hash = Name.GetHashCode();
            Uses = uses;
            _Type = _type;
        }
    }
    class Vals : Ident
    {
        public Vals(string name, Uses uses, Type _type)
        {
            Name = name;
            Hash = Name.GetHashCode();
            Uses = uses;
            _Type = _type;
        }
    }
    class Consts<T>: Ident
    {
        T Value;
        public Consts(string name, Uses uses, Type _type,T value)
        {
            Name = name;
            Hash = Name.GetHashCode();
            Uses = uses;
            _Type = _type;
            Value = value;
        }
    }
    class Param
    {
        Type _type;
        ParaType ParaType;
        public Param(Type type, ParaType paraType)
        {
            _type = type;
            ParaType = paraType;
        }
    }
    class Methods : Ident
    {
        List<Param> Params;
        public Methods(string name, Uses uses, Type _type,params Param[] param)
        {
            Name = name;
            Hash = Name.GetHashCode();
            Uses = uses;
            _Type = _type;
            Params = new List<Param>(param);
        }
    }
    class Program
    {
        static Tree ReadFromFile(string name)
        {
            Tree tree = new Tree();
            StreamReader streamReader = new StreamReader(name + ".txt");
            while (!streamReader.EndOfStream)
            {
                string input = streamReader.ReadLine();
                tree.Add(GetIdent(input));
            }
            streamReader.Close();
            return tree;
        }
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
            throw new ArgumentException("Неизвестный тип данных");
        }
        static ParaType GetParaType(string input)
        {
            switch (input)
            {
               case "out":return ParaType.param_out;
               case "ref":return ParaType.param_ref;
               case "":return ParaType.param_val;
            }
            throw new ArgumentException("Неизвестный тип передачи параметра");
        }
        static Ident GetIdent(string input)
        {
            Regex InputCheck = new Regex("([a-z]+ [a-z0-9]+[(][a-z0-9 ,]+);$");
            Regex ValOfClassCheck = new Regex("^[a-z]+ [A-Za-z0-9]+;$");
            if (ValOfClassCheck.IsMatch(input)) return GetValsOrClasses(input);
            Regex ConstCheck = new Regex("^const [a-z]+ [a-z]+[ ]*=[ ]*[a-z0-9]+;$");
            if (ConstCheck.IsMatch(input)) return GetConst(input);
            try
            {
                return GetMethod(input);
            }
            catch (ArgumentException)
            {
                throw new Exception("Не сумели определить индентификатор");
            }
        }
        static Ident GetMethod(string input)
        {
            string[] inputs = new Regex("[ (]+").Split(input);
            string _Type = inputs[0];
            string Name = inputs[1];
            Regex ParamsRegex = new Regex("[(][a-zA-Z 0-9,]+[)]");
            Match Params = ParamsRegex.Match(input);
            string StParams = Params.Value;
            Regex ParamRegex = new Regex("(ref |out |)[a-z]+ [A-Za-z0-9]+");
            Match MatchParam = ParamRegex.Match(StParams);
            List<Param> ParamList = new List<Param>();
            while (MatchParam.Success)
            {
                string[] tempParam = MatchParam.Value.Split();
                ParaType paraType;
                if (tempParam.Length == 2)
                {
                    paraType = GetParaType("");
                } else
                {
                    paraType = GetParaType(tempParam[0]);
                }
                Type type = GetType(tempParam[tempParam.Length - 2]);
                ParamList.Add(new Param(type,paraType));
                MatchParam = MatchParam.NextMatch();
            }
            return new Methods(Name, Uses.METHODS, GetType(_Type), ParamList.ToArray());
        }
        static Ident GetConst(string input)
        {
            string[] inputs = new Regex("[ =]+").Split(input);
            if (inputs.Length != 4) throw new ArgumentException();
            inputs[3]= inputs[3].Remove(inputs[3].Length - 1, 1);
            Type type = GetType(inputs[1]);
            switch (type)
            {
                case Type.bool_type:
                    return new Consts<bool>(inputs[2], Uses.CONSTS, type, bool.Parse(inputs[3]));
                case Type.char_type:
                    return new Consts<char>(inputs[2], Uses.CONSTS, type, char.Parse(inputs[3]));
                case Type.float_type:
                    return new Consts<float>(inputs[2], Uses.CONSTS, type, float.Parse(inputs[3]));
                case Type.int_type:
                    return new Consts<int>(inputs[2], Uses.CONSTS, type, int.Parse(inputs[3]));
                case Type.string_type:
                    return new Consts<string>(inputs[2], Uses.CONSTS, type, inputs[3]);
            }
            throw new Exception("Константа имеет неизвестный тип");
        }
        static Ident GetValsOrClasses(string input)
        {
            string[] inputs = input.Split();
            if (inputs.Length > 2) throw new ArgumentException("Неожиданно большое количество пробелов");
            Type type = GetType(inputs[0]);
            if (type == Type.class_type) return new Classes(inputs[1], Uses.CLASSES, type);
            else return new Vals(inputs[1], Uses.VARS, type);
        }
        static void Main(string[] args)
        {
            Tree tree = ReadFromFile("Test");
            /*
            Tree tree = new Tree();
            tree.Add(GetIdent("int a;"));
            tree.Add(GetIdent("const int a=10;"));
            tree.Add(GetIdent("class MyClass;"));
            tree.Add(GetIdent("string method1(int x1, ref char x2, out float x3);"));
            */
            Console.ReadLine();
        }
    }
}

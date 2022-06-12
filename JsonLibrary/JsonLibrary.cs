using System.Collections;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Reflection;

namespace JsonLibrary
{
    public static class JsonLibrary
    {
        public static string Serialize(this object? obj)
        {
            if (obj is null)
            {
                return "null";
            }
            else if (obj is JsonValue jv)
            {
                return jv.Value.Serialize();
            }
            else if (obj is string str)
            {
                return $"\"{str}\"";
            }
            else if (obj is double dnum)
            {
                return dnum.ToString("g", CultureInfo.InvariantCulture);
            }
            else if (obj is int inum)
            { 
                return inum.ToString("g", CultureInfo.InvariantCulture);
            }
            else if (obj is bool b)
            {
                return b.ToString().ToLower();
            }
            else if (obj is List<JsonValue> jvl)
            {
                return SerializeList(jvl);
            }
            else if (obj is List<string> sl)
            {
                return SerializeList(sl);
            }
            else if (obj is List<double> dl)
            {
                return SerializeList(dl);
            }
            else if (obj is List<int> il)
            {
                return SerializeList(il);
            }
            else if (obj is List<bool> bl)
            {
                return SerializeList(bl);
            }
            else if (obj is Dictionary<string, JsonValue> jvd)
            {
                return SerializeDict(jvd);
            }
            else if (obj is Dictionary<string, string> sd)
            {
                return SerializeDict(sd);
            }
            else if (obj is Dictionary<string, double> dd)
            {
                return SerializeDict(dd);
            }
            else if (obj is Dictionary<string, int> id)
            {
                return SerializeDict(id);
            }
            else if (obj is Dictionary<string, bool> bd)
            {
                return SerializeDict(bd);
            }
            else
            {
                return SerializeClass(obj);
            }
        }
        static string SerializeList<T>(List<T> list)
        {
            string result = "[";
            bool comma_skipped = false;
            foreach (var value in list)
            {
                if (comma_skipped)
                {
                    result += ",";
                }
                else
                {
                    comma_skipped = true;
                }
                result += Serialize(value);

            }
            result += "]";
            return result;
        }
        static string SerializeDict<T>(Dictionary<string,T> list)
        {
            string result = "{";
            bool comma_skipped = false;
            foreach (var pair in list)
            {
                if (comma_skipped)
                {
                    result += ",";
                }
                else
                {
                    comma_skipped = true;
                }
                result += Serialize(pair.Key);
                result += ":";
                result += Serialize(pair.Value);

            }
            result += "}";
            return result;
        }
        static string SerializeClass(object obj)
        {
            Type type = obj.GetType();
            string typeString = type.AssemblyQualifiedName;
            Dictionary<string, JsonValue> dict = new();
            dict.Add("class", new JsonValue(typeString));
            foreach (PropertyInfo property in type.GetProperties())
            {
                if (property.Name == "class")
                {
                    throw new Exception("В классе уже есть поле class");
                };
                dict.Add(property.Name, new JsonValue(property.GetValue(obj)));
            }
            return dict.Serialize();

        }

    }

    public class JsonValue
    {
        public object? Value;
        public JsonValue(object? obj) { Value = obj; }
        public JsonValue() { }
    }

    

}
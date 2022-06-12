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
            string? typeString = type.AssemblyQualifiedName;
            if (typeString == null) { throw new Exception(); };
            Dictionary<string, JsonValue> dict = new();
            dict.Add("class", new JsonValue(typeString));
            foreach (FieldInfo field in type.GetFields())
            {
                if (field.Name == "class")
                {
                    throw new Exception("В классе уже есть поле class");
                };
                dict.Add(field.Name, new JsonValue(field.GetValue(obj)));
            }
            return dict.Serialize();

        }
        public static object? Deserialize(this string str)
        {
            int i = 0;
            str = str.Trim();
            skip_spaces();
            return inner_deserialize();
            void skip_spaces()
            {
                while (i < str.Length && char.IsWhiteSpace(str[i]))
                {
                    i++;
                }
            }
            object? inner_deserialize()
            {
                if (str[i] == '{')
                {
                    i++;
                    skip_spaces();
                    Dictionary<string, object?> dict = new();
                    if (str[i] != '}')
                    {
                        while (true)
                        {
                            string? name = (string?)inner_deserialize();
                            skip_spaces();
                            if (str[i] == ':')
                            {
                                i++;
                                skip_spaces();
                            } else
                            {
                                throw new Exception();
                            }
                            object? value = inner_deserialize();
                            if (name != null)
                            {
                                dict.Add(name, value);

                            } else
                            {
                                throw new Exception();
                            }
                            skip_spaces();
                            if (str[i] == ',')
                            {
                                i++;
                            } else
                            {
                                break;
                            }
                        }
                        if (str[i] != '}')
                        {
                            throw new Exception();
                        }
                        if (dict.ContainsKey("class"))
                        {
                            object? classTypeString = dict["class"];
                            if (classTypeString != null)
                            {
                                Type? desrializedObjectType = Type.GetType((string)classTypeString);
                                if (desrializedObjectType != null)
                                {
                                    object? obj = Activator.CreateInstance(desrializedObjectType);
                                    foreach (FieldInfo field in desrializedObjectType.GetFields())
                                    {
                                        if (field.Name == "class")
                                        {
                                            throw new Exception("В классе уже есть поле class");
                                        };
                                        //dict.Add(field.Name, new JsonValue(field.GetValue(obj)));
                                        field.SetValue(obj, dict[field.Name]);
                                    }
                                    return obj;
                                } else 
                                { 
                                    throw new Exception(); 
                                }
                            } else
                            {
                                throw new Exception("Класс не найден");
                            }
                        } else
                        {
                            Dictionary<string, JsonValue> results = new();
                            foreach (var pair in dict)
                            {
                                results.Add(pair.Key, new JsonValue(pair.Value));
                            }
                            return new JsonValue(results);
                        }
                    } else
                    {
                        return new JsonValue(new Dictionary<string, JsonValue>());
                    }

                }
                else if (str[i] == '[')
                {
                    throw new NotImplementedException();
                }
                else if (str[i] == '"')
                {
                    i++;
                    string resultingString = "";
                    while (true)
                    {
                        if (str[i] == '"' && str[i-1] != '\\')
                        {
                            break;
                        }
                        resultingString+= str[i];
                        i++;
                    }
                    i++;
                    return resultingString;
                }
                else if (char.IsDigit(str[i]) || str[i] == '-')
                {
                    throw new NotImplementedException();
                }
                else
                {
                    string substring = str.Substring(i, 5);
                    if (substring == "false")
                    {
                        i += 5;
                        return false;
                    }
                    else if (substring.StartsWith("true"))
                    {
                        i += 4;
                        return true;
                    }
                    else if (substring.StartsWith("null"))
                    {
                        i += 4;
                        return null;
                    }
                    else
                    {
                        throw new Exception("Как неожиданно и неприятно");
                    }
                }
            }
        }
    }

    public class JsonValue
    {
        public object? Value;
        public JsonValue(object? obj) { Value = obj; }
        public JsonValue() { }
    }

    

}
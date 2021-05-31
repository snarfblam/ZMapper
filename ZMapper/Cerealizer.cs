using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FasTrak
{
    class Cerealizer
    {
        HashSet<object> objs = new HashSet<object>();

        private Cerealizer() { }

        public static Cereal CerealFromObject(object o) {
            return (new Cerealizer()).FromObject(o);
        }
        public static object ObjectFromCereal(Cereal c, Type t) {
            //return (new Jsonifier()).FromCereal(c, t);
            return DataToObject(c, t, null);
        }

        Cereal FromObject(object o) {
            Cereal result = new Cereal();

            var type = o.GetType();
            var props = type.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            foreach (var p in props) {
                var atts = p.GetCustomAttributes(typeof(CerealIgnore), false);
                if (atts.Length == 0 && p.GetIndexParameters().Length == 0) {
                    var name = p.Name;
                    var value = p.GetValue(o, null);
                    if (isSimpleType(value)) {
                        result[name] = value;
                    } else if (value is System.Collections.IList) {
                        var list = value as System.Collections.IList;
                        //System.Collections.ArrayList array = new System.Collections.ArrayList(list);
                        var array = new System.Collections.ArrayList();
                        foreach (var item in list) {
                            if (isSimpleType(item)) {
                                array.Add(item);
                            } else if (item is System.Collections.IList) {
                                // Todo: handle this. it's a PITA that this method must return a Cereal when we might want to return a simple type or list
                                // current setup can't handle lists nested directly in lists
                                throw new NotImplementedException();
                            } else {
                                array.Add(FromObject(item));
                            }
                        }
                        result[name] = array;
                    } else {
                        if (!(value is ValueType)) {
                            if (objs.Contains(value)) throw new ArgumentException("The specified object contains circular reference(s)");
                            objs.Add(value);
                        }

                        result[name] = CerealFromObject(value);
                    }
                }
            }

            return result;
        }

        //Cereal FromCereal(Cereal c, Type type) {
        //    var result = Activator.CreateInstance(type);

        //    var props = type.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        //    foreach (var p in props) {
        //        var atts = p.GetCustomAttributes(typeof(JsonIgnoreAttribute), false);
        //        if (atts.Length == 0 && p.GetIndexParameters().Length == 0) {
        //            var pName = p.Name;
        //            var cValue = c[pName];
        //            var pType = p.PropertyType;

        //            if (typeof(System.Collections.IList).IsAssignableFrom(pType)) {
        //                var cList = cValue as System.Collections.IList;
        //                var pList = p.GetValue(result, null);
        //                // var propList = p.GetValue(result) as System.Collections.IList;
        //                //foreach(var element in cList) propList.Add(
        //                ApplyCerealList(p, cList, pList);
        //                result[pName] = array;
        //            } else if (pType.IsClass) {
        //                if (!(cValue is ValueType)) {
        //                    if (objs.Contains(cValue)) throw new ArgumentException("The specified object contains circular reference(s)");
        //                    objs.Add(cValue);
        //                }

        //                result[pName] = CerealFromObject(cValue);
        //            } else {
        //                result[pName] = cValue;
        //            }
        //        }
        //    }

        //    return result;
        //}


        static Type stringType = typeof(string);
        static Type intType = typeof(string);
        static Type listType = typeof(System.Collections.IList);
        static Type genericListType = typeof(IList<>);

        /// <summary>
        /// Converts Cereal data to the object it represents.
        /// </summary>
        /// <param name="cereal">Cereal data</param>
        /// <param name="expectedType">Type the data is expected to represent. Used when objects need to be instantiated, NOT for type checking.</param>
        /// <returns></returns>
        static object DataToObject(object cereal, Type expectedType, object parent) {
            if (cereal is Cereal) {
                return CerealDataToObject((Cereal)cereal, expectedType, parent);
            } else if (cereal is System.Collections.IList) {
                return CerealListToObject((System.Collections.IList)cereal, expectedType, parent);
                //} else if (isSimpleType(cereal)) {
            } else {
                return cereal;
            }

        }

        static object CerealListToObject(System.Collections.IList array, Type expectedType, object parent) {
            var result = new System.Collections.ArrayList();
            //Type elementType;

            //if (expectedType.IsArray) {
            //    elementType = expectedType.GetElementType();
            //} else if (expectedType.IsGenericType) {
            //    elementType = expectedType.GetGenericArguments()[0];
            //} else {
            //    elementType = typeof(object);
            //}
            Type elementType = GetElementType(expectedType);
            if (elementType != null) foreach (var cereal in array) {
                    result.Add(DataToObject(cereal, elementType, parent));
                }

            return result;
        }

        static Type GetElementType(Type collectionTypeMaybe) {
            Type iListType;

            try {
                if (collectionTypeMaybe.IsArray) {
                    return collectionTypeMaybe.GetElementType();
                } else if (null != (iListType = collectionTypeMaybe.GetInterface("IList`1"))) {
                    return iListType.GetGenericArguments()[0];
                }
            } catch (System.Reflection.AmbiguousMatchException) {
                // return null;
            }
            return null;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cereal"></param>
        /// <param name="expectedType"></param>
        /// <returns></returns>
        static private object CerealDataToObject(Cereal cereal, Type expectedType, object parent) {
            var result = CreateInstance(expectedType, parent);

            foreach(KeyValuePair<string, object> cProp in cereal) {
                var prop = expectedType.GetProperty(cProp.Key);
                var pType = prop.PropertyType;
                var pNewValue = DataToObject(cProp.Value, pType, result);
                if (listType.IsAssignableFrom(pType)) {
                    AssignListToProperty(result, prop, (System.Collections.IList)pNewValue);
                } else {
                    prop.SetValue(result, pNewValue, null);
                }
            }

            return result;
        }

        private static object CreateInstance(Type expectedType, object parent) {
            // If applicable, we will prefer a constructor that accepts the parent object
            if (parent != null) {
                var parentType = parent.GetType();

                // If a constructor is available that will accept the parent object, use that
                var ctors = expectedType.GetConstructors();
                foreach(var ctor in ctors){
                    var prams = ctor.GetParameters();
                    if (prams.Length == 1) {
                        var pType = prams[0].ParameterType;
                        if (pType.IsAssignableFrom(parentType)) {
                            return ctor.Invoke(new object[] { parent });
                        }
                    }
                }
            }

            // When either there is no parent, or no constructor can accept it, we hope for a default constructor
            return Activator.CreateInstance(expectedType);
        }

        static private void AssignListToProperty(object obj, System.Reflection.PropertyInfo property, System.Collections.IList listValues) {
            var propertyList = property.GetValue(obj, null) as System.Collections.IList;
            foreach (var item in listValues) propertyList.Add(item);
        }


        /// <summary>
        /// Returns whether the specified object meets the criterea: 
        /// can be expressed as a simple literal JSON value,
        /// can be expressed as a non-object, non-array Cereal object property.
        /// </summary>
        static bool isSimpleType(object value) {
            return
                value == null ||
                value is int ||
                value is string || 
                value is bool;
        }
    }

    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    sealed class CerealIgnore : Attribute
    {
        public CerealIgnore() {
        }
    }



}

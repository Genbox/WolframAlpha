using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;

namespace Genbox.WolframAlpha.Serialization
{
    internal class SimpleXmlSerializer
    {
        public T Deserialize<T>(Stream stream)
        {
            XDocument doc = XDocument.Load(stream);
            T x = Activator.CreateInstance<T>();
            return (T)Map(x, doc.Root);
        }

        private object Map(object x, XElement root)
        {
            Type objType = x.GetType();
            PropertyInfo[] props = objType.GetProperties();

            foreach (PropertyInfo prop in props)
            {
                Type type = prop.PropertyType;
                bool typeIsPublic = type.IsPublic || type.IsNestedPublic;

                if (!typeIsPublic || !prop.CanWrite)
                    continue;

                XName name = prop.Name;
                bool isAttr = false;

                SerializeInfoAttribute attribute = prop.GetCustomAttribute<SerializeInfoAttribute>(false);

                if (attribute != null)
                {
                    name = attribute.Name;
                    isAttr = attribute.IsAttribute;
                }

                string value = GetValueFromXml(root, name, isAttr);

                //We have 2 case for lists. The first one:
                //
                //<list>
                //  <item></item>
                //  <item></item>
                //</list>
                //
                //In this case we use the property's real name to find the container and map the items from children

                //Second one:
                //
                //<item></item>
                //<item></item>
                //
                //In this case we use the generic type's real name and use the root to find the children

                if (value == null)
                {
                    if (type.IsGenericType)
                    {
                        Type genericType = type.GetGenericArguments()[0];

                        string realName = genericType.Name;

                        SerializeInfoAttribute attr = genericType.GetCustomAttribute<SerializeInfoAttribute>();

                        if (attr != null)
                            realName = attr.Name;

                        XElement first = GetElementByName(root, realName);
                        IList list = (IList)Activator.CreateInstance(type);

                        if (first != null && root != null)
                        {
                            IEnumerable<XElement> elements = root.Elements(first.Name);

                            PopulateListFromElements(genericType, elements, list);
                        }

                        prop.SetValue(x, list, null);
                    }

                    continue;
                }

                if (TryParseSimpleValue(value, type, out object parsedValue))
                {
                    prop.SetValue(x, parsedValue, null);
                    continue;
                }

                //If it wasn't a supported primitive type, objVal is null here

                if (type.IsGenericType)
                {
                    if (type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        prop.SetValue(x, null, null);
                        continue;
                    }

                    IList list = (IList)Activator.CreateInstance(type);
                    XElement container = GetElementByName(root, name);

                    if (container.HasElements)
                    {
                        XElement first = container.Elements().FirstOrDefault();

                        if (first != null)
                        {
                            Type t = type.GetGenericArguments()[0];
                            IEnumerable<XElement> elements = container.Elements(first.Name);

                            PopulateListFromElements(t, elements, list);
                        }
                    }

                    prop.SetValue(x, list, null);
                }
                else if (typeof(List<>).IsAssignableFrom(type))
                {
                    object objVal = HandleListDerivative(root, prop.Name, type);

                    if (objVal != null)
                        prop.SetValue(x, objVal, null);
                }
                else
                {
                    // nested property classes
                    if (root == null)
                        continue;

                    XElement element = GetElementByName(root, name);

                    if (element == null)
                        continue;

                    object objVal = CreateAndMap(type, element);

                    if (objVal != null)
                        prop.SetValue(x, objVal, null);
                }
            }

            return x;
        }

        private static bool TryParseSimpleValue(string value, Type type, out object outVal)
        {
            outVal = null;

            if (type == typeof(string))
            {
                outVal = value;
                return true;
            }

            if (string.IsNullOrEmpty(value))
                return false;

            if (type.IsPrimitive)
                outVal = Convert.ChangeType(value, type, CultureInfo.InvariantCulture);
            else if (type.IsEnum)
                outVal = Enum.Parse(type, value, true);
            else if (type == typeof(Uri))
                outVal = new Uri(value, UriKind.RelativeOrAbsolute);
            else if (type == typeof(DateTime))
                outVal = DateTime.Parse(value, CultureInfo.InvariantCulture);
            else if (type == typeof(DateTimeOffset))
                outVal = XmlConvert.ToDateTimeOffset(value);
            else if (type == typeof(decimal))
                outVal = decimal.Parse(value, CultureInfo.InvariantCulture);
            else if (type == typeof(Guid))
                outVal = new Guid(value);
            else if (type == typeof(TimeSpan))
                outVal = XmlConvert.ToTimeSpan(value);
            else if (type == typeof(Version))
                outVal = Version.Parse(value);

            return outVal != null;
        }

        private void PopulateListFromElements(Type type, IEnumerable<XElement> elements, IList list)
        {
            foreach (object item in elements.Select(x => CreateAndMap(type, x)))
            {
                list.Add(item);
            }
        }

        private object HandleListDerivative(XElement root, string propName, Type type)
        {
            Type realType = type.IsGenericType
                ? type.GetGenericArguments()[0]
                : type.BaseType.GetGenericArguments()[0];

            IList list = (IList)Activator.CreateInstance(type);

            string name = realType.Name;

            SerializeInfoAttribute attribute = realType.GetCustomAttribute<SerializeInfoAttribute>();

            if (attribute != null)
                name = attribute.Name;

            IEnumerable<XElement> elements = FindDescendants(root, name);

            PopulateListFromElements(realType, elements, list);

            if (!type.IsGenericType)
                Map(list, root.Element(propName) ?? root);

            return list;
        }

        private object CreateAndMap(Type type, XElement element)
        {
            object item;

            if (type == typeof(string))
                item = element.Value;
            else if (type.GetTypeInfo().IsPrimitive)
                item = Convert.ChangeType(element.Value, type, CultureInfo.InvariantCulture);
            else
            {
                item = Activator.CreateInstance(type);
                Map(item, element);
            }

            return item;
        }

        private static string GetFromName(XElement root, XName name)
        {
            string val = null;

            if (root == null)
                return null;

            XElement element = GetElementByName(root, name);

            if (element == null)
            {
                XAttribute attribute = FindAttribute(root, name);

                if (attribute != null)
                    val = attribute.Value;
            }
            else
            {
                if (!element.IsEmpty || element.HasElements || element.HasAttributes)
                    val = element.Value;
            }

            return val;
        }

        private static string GetValueFromXml(XElement root, XName name, bool isAttr)
        {
            if (!isAttr)
                return GetFromName(root, name);

            XAttribute attributeVal = FindAttribute(root, name);

            return attributeVal?.Value ?? GetFromName(root, name);
        }

        private static XElement GetElementByName(XElement root, XName name)
        {
            XElement element = FindSelfOrElement(root, name);

            if (element != null)
                return element;

            IOrderedEnumerable<XElement> orderedDescendants = root
                                                              .Descendants()
                                                              .OrderBy(d => d.Ancestors().Count());

            element = orderedDescendants.FirstOrDefault(d => string.Equals(d.Name.LocalName, name.LocalName, StringComparison.OrdinalIgnoreCase));

            return element == null
                   && name == "Value"
                   && (!root.HasAttributes || root.Attributes().All(x => x.Name != name))
                ? root
                : element;
        }

        private static XAttribute FindAttribute(XElement root, XName name)
        {
            return root.Attributes().FirstOrDefault(x => string.Equals(x.Name.LocalName, name.LocalName, StringComparison.OrdinalIgnoreCase));
        }

        private static XElement FindSelfOrElement(XElement root, XName name)
        {
            if (root.Name.LocalName.Equals(name.LocalName, StringComparison.OrdinalIgnoreCase))
                return root;

            return FindElements(root, name).FirstOrDefault();
        }

        private static IEnumerable<XElement> FindElements(XElement root, XName name)
        {
            return root.Elements().Where(x => string.Equals(x.Name.LocalName, name.LocalName, StringComparison.OrdinalIgnoreCase));
        }

        private static IEnumerable<XElement> FindDescendants(XElement root, XName name)
        {
            return root.Descendants().Where(x => string.Equals(x.Name.LocalName, name.LocalName, StringComparison.OrdinalIgnoreCase));
        }
    }
}
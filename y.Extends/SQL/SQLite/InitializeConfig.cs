using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace y.Extends.SQL.SQLite
{
    internal class InitializeConfig
    {


        public void Initialize()
        {
            var x = new XmlDocument();
            var path = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
            x.Load(path);
            CreateConfigSectionsNode(x);
            CreateEntityFrameworkNode(x);
            CreateConnectionStringsNode(x);
            CreateSystemDataNode(x);
            CreateRunTimeNode(x);
            x.Save(path);
        }

        private void CreateConnectionStringsNode(XmlDocument x)
        {
            var child = x.SelectSingleNode("configuration");
            var node = AddNode(x, child, "connectionStrings", new Dictionary <string, string>());

            AddNode(x, node, "add", new Dictionary <string, string>
            {
                ["name"] = "SQLiteConnectionString",
                ["connectionString"] = "data source=D:\\default.sqlte",
                ["providerName"] = "System.Data.SQLite"
            });
        }

        private void CreateRunTimeNode(XmlDocument x)
        {
            var child = x.SelectSingleNode("configuration");

            var node = AddNode(x, child, "runtime", new Dictionary <string, string>());

            var child1 = AddNode(x, node, "assemblyBinding", new Dictionary <string, string>
            {
                ["xmlns"] = "urn:schemas-microsoft-com:asm.v1"
            });

            var flag = false;

            insertNode:
            var list = child1.ChildNodes.FindNode("dependentAssembly");
            if (list == null || list.Count == 0 || flag)

            {
                var child2 = AddNode(x, child1, "dependentAssembly", new Dictionary <string, string>(), true);
                AddNode(x, child2, "assemblyIdentity", new Dictionary <string, string>
                {
                    ["name"] = "System.Data.SQLite",
                    ["publicKeyToken"] = "db937bc2d44ff139",
                    ["culture"] = "neutral"
                });
                AddNode(x, child2, "bindingRedirect", new Dictionary <string, string>
                {
                    ["oldVersion"] = "0.0.0.0-1.0.105.2",
                    ["newVersion"] = "1.0.105.2"
                });

                var child3 = AddNode(x, child1, "dependentAssembly", new Dictionary <string, string>(), true);
                AddNode(x, child3, "assemblyIdentity", new Dictionary <string, string>
                {
                    ["name"] = "System.Data.SQLite.EF6",
                    ["publicKeyToken"] = "db937bc2d44ff139",
                    ["culture"] = "neutral"
                });
                AddNode(x, child3, "bindingRedirect", new Dictionary <string, string>
                {
                    ["oldVersion"] = "0.0.0.0-1.0.105.2",
                    ["newVersion"] = "1.0.105.2"
                });

                return;
            }

            if (list.Count > 0)
            {
                var exi = list.FirstOrDefault(i => InitConfigHelper.FindNode(i.ChildNodes, "assemblyIdentity").Exists(j => j.Attributes.ToList().Exists(iq => iq.InnerText == "System.Data.SQLite.EF6")));

                var exi2 = list.FirstOrDefault(i => i.ChildNodes.FindNode("assemblyIdentity").Exists(j => j.Attributes.ToList().Exists(iq => iq.InnerText == "System.Data.SQLite")));

                if (exi != null && exi2 != null)
                {
                    return;
                }

                if (exi != null)
                {
                    child1.RemoveChild(exi);
                }
                if (exi2 != null)
                {
                    child1.RemoveChild(exi2);
                }
                flag = true;
                goto insertNode;
            }
        }

        private void CreateSystemDataNode(XmlDocument x)
        {
            var child = x.SelectSingleNode("configuration");

            var node = AddNode(x, child, "system.data", new Dictionary <string, string>());

            var child1 = AddNode(x, node, "DbProviderFactories", new Dictionary <string, string>());
            AddNode(x, child1, "remove", new Dictionary <string, string>
            {
                ["invariant"] = "System.Data.SQLite.EF6"
            });
            AddNode(x, child1, "add", new Dictionary <string, string>
            {
                ["name"] = "SQLite Data Provider (Entity Framework 6)",
                ["invariant"] = "System.Data.SQLite.EF6",
                ["description"] = ".NET Framework Data Provider for SQLite (Entity Framework 6)",
                ["type"] = "System.Data.SQLite.EF6.SQLiteProviderFactory, System.Data.SQLite.EF6"
            });
            AddNode(x, child1, "remove", new Dictionary <string, string>
            {
                ["invariant"] = "System.Data.SQLite"
            });

            AddNode(x, child1, "add", new Dictionary <string, string>
            {
                ["name"] = "SQLite Data Provider",
                ["invariant"] = "System.Data.SQLite",
                ["description"] = ".NET Framework Data Provider for SQLite",
                ["type"] = "System.Data.SQLite.SQLiteFactory, System.Data.SQLite"
            });
        }

        private void CreateEntityFrameworkNode(XmlDocument x)
        {
            var child = x.SelectSingleNode("configuration");

            XmlNode Action()
            {
                var ele = x.CreateElement("entityFramework");
                child?.AppendChild(ele);
                return ele;
            }

            NodeSelector(child?.ChildNodes, "entityFramework", Action, out var node);

            var child1 = AddNode(x, node, "defaultConnectionFactory", new Dictionary <string, string>
            {
                ["type"] = "System.Data.Entity.Infrastructure.LocalDbConnectionFactory, EntityFramework"
            });

            var child2 = AddNode(x, child1, "parameters", new Dictionary <string, string>());

            AddNode(x, child2, "parameter", new Dictionary <string, string>
            {
                ["value"] = "v13.0"
            });

            var child3 = AddNode(x, node, "providers", new Dictionary <string, string>());

            var sqlite = child3.ChildNodes.ToList().FirstOrDefault(i => i.ChildNodes.ToList().Exists(j => j.Attributes.ToList().Exists(iq => iq.InnerText == "System.Data.SQLite")));
            if (sqlite == null)
            {
                AddNode(x, child3, "provider", new Dictionary <string, string>
                {
                    ["invariantName"] = "System.Data.SQLite",
                    ["type"] = "System.Data.SQLite.EF6.SQLiteProviderServices, System.Data.SQLite.EF6"
                });
            }

            var sqlserver = child3.ChildNodes.ToList().FirstOrDefault(i => i.ChildNodes.ToList().Exists(j => j.Attributes.ToList().Exists(iq => iq.InnerText == "System.Data.SqlClient")));

            if (sqlserver == null)
            {
                AddNode(x, child3, "provider", new Dictionary <string, string>
                {
                    ["invariantName"] = "System.Data.SqlClient",
                    ["type"] = "System.Data.Entity.SqlServer.SqlProviderServices, EntityFramework.SqlServer"
                });
            }
        }

        private XmlNode AddNode(XmlDocument x, XmlNode parent, string nodeName, IDictionary <string, string> keyValue, bool isNeed = false)
        {
            XmlNode node = null;

            if (keyValue.Count > 0)
            {
                var kv = keyValue.First();
                node = parent.ChildNodes.FindNode(nodeName, kv.Key, kv.Value)?.FirstOrDefault();
            }
            else
            {
                node = parent.ChildNodes.FindNode(nodeName)?.FirstOrDefault();
            }

            if (! isNeed && node != null)
            {
                return node;
            }

            var eleChild = x.CreateElement(nodeName);
            foreach (var kv in keyValue)
            {
                eleChild.SetAttribute(kv.Key, kv.Value);
            }

            parent?.AppendChild(eleChild);
            return eleChild;
        }

        private void CreateConfigSectionsNode(XmlDocument x)
        {
            var child = x.SelectSingleNode("configuration");

            XmlNode AddAction()
            {
                var ele1 = x.CreateElement("configSections");
                var ll = new List <XmlNode>();
                if (child?.ChildNodes != null)
                {
                    foreach (XmlNode nodes in child?.ChildNodes)
                    {
                        child.RemoveChild(nodes);
                        ll.Add(nodes);
                    }
                }

                child?.AppendChild(ele1);
                foreach (var nodes in ll)
                {
                    child?.AppendChild(nodes);
                }

                return ele1;
            }

            NodeSelector(child?.ChildNodes, "configSections", AddAction, out var node);
            var flag = true;
            XmlElement ele = null;
            if (node != null)
            {
                var list = node?.ChildNodes;
                if (list.ExistAttributes("name", "entityFramework"))
                {
                    flag = false;
                }

                ele = (XmlElement) node;
            }

            if (! flag)
            {
                return;
            }

            IDictionary <string, string> keyValue = new Dictionary <string, string>
            {
                ["name"] = "entityFramework",
                ["type"] = "System.Data.Entity.Internal.ConfigFile.EntityFrameworkSection, EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089",
                ["requirePermission"] = "false"
            };
            AddNode(x, ele, "section", keyValue);
        }

        private void NodeSelector(XmlNodeList nodeList, string targetNodeName, Func <XmlNode> addAction, out XmlNode xmlNode)
        {
            xmlNode = null;
            if (nodeList == null)
            {
                return;
            }

            foreach (XmlNode xmlNode2 in nodeList)
            {
                if (xmlNode2.Name == targetNodeName)
                {
                    xmlNode = xmlNode2;
                    return;
                }
            }

            xmlNode = addAction?.Invoke();

            //return false;
            //return nodeList.Cast < XmlNode > ( ).Any ( parent => parent.Name == targetNodeName );
        }
    }

    internal static class InitConfigHelper
    {
        public static bool ExistAttributes(this XmlNodeList list, string key, string value = "")
        {
            if (list == null)
            {
                return false;
            }

            foreach (XmlNode item in list)
            {
                if (item?.Attributes == null)
                {
                    continue;
                }

                foreach (XmlAttribute attr in item?.Attributes)
                {
                    if (Equals(attr.Name, key))
                    {
                        if (Equals(value, ""))
                        {
                            return true;
                        }
                        if (attr.InnerText == value)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public static List <XmlNode> FindNode(this XmlNodeList list, string nodeName, string oneKey = "", string oneValue = "")
        {
            if (list == null)
            {
                return null;
            }

            var ll = new List <XmlNode>();
            foreach (XmlNode variable in list)
            {
                if (variable.Name == nodeName)
                {
                    if (! string.IsNullOrWhiteSpace(oneKey) && ! string.IsNullOrWhiteSpace(oneValue))
                    {
                        if (variable.Attributes != null)
                        {
                            var flag = variable.Attributes.ToList().Exists(i => Equals(i.Name, oneKey) && Equals(i.InnerText, oneValue));

                            if (flag)
                            {
                                ll.Add(variable);
                                return ll;
                            }
                        }

                        continue;
                    }

                    ll.Add(variable);
                }
            }

            return ll.Count > 0 ? ll : null;
        }

        public static List <XmlNode> ToList(this XmlNodeList list)
        {
            var nn = new List <XmlNode>();
            if (list == null)
            {
                return nn;
            }

            foreach (XmlNode variable in list)
            {
                nn.Add(variable);
            }

            return nn;
        }

        public static List <XmlAttribute> ToList(this XmlAttributeCollection collection)
        {
            var nn = new List <XmlAttribute>();
            if (collection == null)
            {
                return nn;
            }

            foreach (XmlAttribute variable in collection)
            {
                nn.Add(variable);
            }

            return nn;
        }
    }
}
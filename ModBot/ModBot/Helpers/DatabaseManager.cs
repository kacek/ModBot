﻿using System;
using System.Xml;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ModBot.Helpers
{
    class DatabaseManager
    {
        private XmlDocument xml = new XmlDocument();

        public bool Init()
        {
            XmlDocument xml = new XmlDocument();
            try
            {
               xml.Load("users.xml");
            }
            catch (FileNotFoundException e)
            {
                XmlDeclaration xmlDec = xml.CreateXmlDeclaration("1.0", "UTF-8", null);
                XmlElement root = xml.DocumentElement;
                xml.InsertBefore(xmlDec, root);
                XmlElement users = xml.CreateElement(string.Empty, "users", string.Empty);
                xml.AppendChild(users);
                xml.Save("users.xml");
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
            return true;
        }

        private XmlElement CreateXmlUser(ModBot.models.User user)
        {
            XmlElement xmlUser = xml.CreateElement(string.Empty, "user", string.Empty);
            xmlUser.SetAttribute("UID", user.getUID().ToString());
            xmlUser.SetAttribute("wallet", user.getWallet().ToString());
            return xmlUser;
        }

        public void AddUser (ModBot.models.User user)
        {
            xml.Load("users.xml");
            XmlElement xmlUser = xml.CreateElement(string.Empty, "user", string.Empty);
            xmlUser.SetAttribute("UID", user.getUID().ToString());
            xmlUser.SetAttribute("wallet", user.getWallet().ToString());
            xml.SelectSingleNode("/users").AppendChild(xmlUser);
            xml.Save("users.xml");
            Console.WriteLine("user added to casino");
        }

        public void UpdateUser(ModBot.models.User user)
        {
            xml.Load("users.xml");
            var xUserOld = xml.SelectSingleNode(string.Format("//user[@UID='{0}']", user.UID));
            xml.SelectSingleNode("/users").ReplaceChild(CreateXmlUser(user), xUserOld);
            xml.Save("users.xml");
        }

        public ModBot.models.User GetUser(UInt64 id)
        {
            xml.Load("users.xml");
            ModBot.models.User user = new ModBot.models.User();
            XmlNode xmlUser;
            xmlUser = xml.SelectSingleNode(string.Format("//user[@UID='{0}']", id));
            if(xmlUser == null)
            {
                return null;
            }
            user.setUID(UInt64.Parse(xmlUser.Attributes["UID"].Value));
            user.setWallet(UInt64.Parse(xmlUser.Attributes["wallet"].Value));
            return user;
        }

        public ModBot.models.User GetUserNotNull(ulong id)
        {
            var user = GetUser(id);
            if (user == null)
            {
                user = new ModBot.models.User()
                {
                    UID = id,
                    Wallet = ModBot.models.User.DEFAULT_WALLET_CNT
                };
                AddUser(user);
            }
            return user;
        }
    }
}

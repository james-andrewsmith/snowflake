using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Snowflake.Server
{
    public sealed class Configuration
    {
        private static Configuration instance;
        public static Configuration Instance
        {
            get { return instance; }
        }

        static Configuration()
        {
            instance = new Configuration();
            instance.GetFromFile();
        }

        private void GetFromFile()
        {

            var oconfig = ConfigurationHandler.Get();
            if (oconfig == null)
                throw new Exception("snowflake section not found in config file");

            instance.LocationID = oconfig.LocationID;
            instance.NodeID = oconfig.NodeID;
            instance.Port = oconfig.Port;      
        }

        #region // Properties //
        public int LocationID { get; private set; }
        public int NodeID { get; private set; }
        public int Port { get; private set; } 
        #endregion

    }


    // Class that creates the configuration handler
    internal class ConfigurationHandler : ConfigurationSection
    {
        public static ConfigurationHandler Get()
        {
            return (ConfigurationHandler)ConfigurationManager.GetSection("snowflake/settings");
        }

        [ConfigurationProperty("locationID", IsRequired = true)]
        public int LocationID
        {
            get
            {
                return (int)this["locationID"];
            }
            set
            {
                this["locationID"] = value;
            }
        }

        [ConfigurationProperty("nodeID", IsRequired = true)]
        public int NodeID
        {
            get
            {
                return (int)this["nodeID"];
            }
            set
            {
                this["nodeID"] = value;
            }
        }

        [ConfigurationProperty("port", IsRequired = true)]
        public int Port
        {
            get
            {
                return (int)this["port"];
            }
            set
            {
                this["port"] = value;
            }
        }

        [ConfigurationProperty("binding", IsRequired = true)]
        public string Binding
        {
            get
            {
                return (string)this["binding"];
            }
            set
            {
                this["binding"] = value;
            }
        }
    }
}

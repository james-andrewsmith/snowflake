using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Snowflake.Client
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
                throw new Exception("Snowflake section not found in config file");

            instance.LocationID = oconfig.LocationID;
            instance.ZookeeperPort = oconfig.ZookeeperPort;      
        }

        #region // Properties //
        public int LocationID { get; private set; }
        public int ZookeeperPort { get; private set; } 
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

        [ConfigurationProperty("zookeeperPort", IsRequired = true)]
        public int ZookeeperPort
        {
            get
            {
                return (int)this["zookeeperPort"];
            }
            set
            {
                this["zookeeperPort"] = value;
            }
        }

        [ConfigurationProperty("zookeeperEndpoint", IsRequired = true)]
        public string ZookeeperEndpoint
        {
            get
            {
                return (string)this["zookeeperEndpoint"];
            }
            set
            {
                this["zookeeperEndpoint"] = value;
            }
        } 
    }
}

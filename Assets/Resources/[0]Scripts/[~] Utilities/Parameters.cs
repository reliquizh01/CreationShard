using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Utilities are used to make programming easier by finding generic methods and use them to create a general function.
/// </summary>
namespace Utilities
{
    public class Parameters{

        /// <summary>
        ///  entries are object(float / int / bool), depending on what was added,
        ///  that can be used to import multiple parameters in one go
        /// </summary>
        private Dictionary<string, object> entries;

        public void Initialize()
        {
            entries = new Dictionary<string, object>();
        }

        // Check if Parameter has the said key
        public bool HasParameter(string key)
        {
            if (entries.ContainsKey(key))
            {
                return true;
            }
            return false;
        }
        
        public  bool AddParameter<T>(string key, T value)
        {
            // Avoiding unnecessary memory consumption
            if(this.entries == null)
            {
                entries = new Dictionary<string, object>();
            }

            // Avoid parameters with the same key duplicating
            if (HasParameter(key))
            {
                Debug.LogWarning("Problem - Attempting to Add Duplicate key : [" + key + "]" );
                return false;
            }

            this.entries.Add(key, value);
            return true;
        }

        public void UpdateParameter<T>(string key, T newValue)
        {
            if (this.entries == null)
            {
                entries = new Dictionary<string, object>();
            }
            // Change the value of hte key
            if (this.entries.ContainsKey(key))
            {
                this.entries[key] = newValue;
            }
            // if entries does not contain the key
            else
            {
                this.entries.Add(key, newValue);
            }
        }
        // Get the value inside the entry [Used if you know the key already]
        public T GetWithKeyParameterValue<T>(string key, T defaultvalue)
        {
            //Debug.Log(key + this.GetType().ToString());
            if(this.entries == null)
            {
                Debug.Log("returning default Value!alc");
                return defaultvalue;
            }

            if (this.entries.ContainsKey(key))
            {
                return (T)this.entries[key];
            }
            Debug.Log("returning default Value!");
            return defaultvalue;
        }

        public T GetParameterValue<T>(T defaultvalue, string key = null)
        {
            if (this.entries == null)
            {
                T tmpValue = defaultvalue;
                return tmpValue;
            }

            if (this.entries.ContainsValue(defaultvalue))
            {
                return (T)this.entries[key];
            }

            return defaultvalue;
        }

        public int GetParameterCount
        {
            get
            {
                if(this.entries == null)
                {
                    Debug.Log("Parameter Does not Exist!");
                    return 0;
                }

                return entries.Count;
            }
        }

        public string[] GetAllKeys()
        {
            string[] keys = entries.Keys.ToArray();

            return keys;
        }
    }
}

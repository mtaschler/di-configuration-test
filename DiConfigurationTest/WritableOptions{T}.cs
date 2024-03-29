﻿namespace DiConfigurationTest
{
    using System;
    using System.IO;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class WritableOptions<T> : IWritableOptions<T> where T : class, new()
    {
        private readonly IOptionsMonitor<T> _options;
        private readonly string _section;
        private readonly string _file;

        public WritableOptions(
            IOptionsMonitor<T> options,
            string section,
            string file)
        {
            _options = options;
            _section = section;
            _file = file;
        }

        public T Value => _options.CurrentValue;
        public T Get(string name) => _options.Get(name);

        public void Update(Action<T> applyChanges)
        {
            var fileInfo = new FileInfo(_file);;
            var physicalPath = fileInfo.FullName;

            JObject jObject;
            if (File.Exists(physicalPath))
            {
                jObject = JsonConvert.DeserializeObject<JObject>(File.ReadAllText(physicalPath));
            }
            else
            {
                jObject = new JObject();
            }

            var sectionObject = jObject.TryGetValue(_section, out JToken section) ?
                JsonConvert.DeserializeObject<T>(section.ToString()) : (Value ?? new T());

            applyChanges(sectionObject);

            jObject[_section] = JObject.Parse(JsonConvert.SerializeObject(sectionObject));
            File.WriteAllText(physicalPath, JsonConvert.SerializeObject(jObject, Formatting.Indented));
        }
    }
}

namespace EasySettings.Dynamic
{
    using System.Collections.Generic;
    using System.Dynamic;

    internal class ReadOnlyDynamicDictonary : DynamicObject
    {
        readonly Dictionary<string, object> _dictionary = new Dictionary<string, object>();

        public ReadOnlyDynamicDictonary()
        {
            
        }

        public ReadOnlyDynamicDictonary(object obj)
        {
            var properties = obj.GetType().GetProperties();
            foreach (var property in properties)
            {
                _dictionary.Add(property.Name.ToLower(), property.GetValue(obj));
            }
        }
 
        public int Count
        {
            get
            {
                return _dictionary.Count;
            }
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            var name = binder.Name.ToLower();

            return _dictionary.TryGetValue(name, out result);
        }
    }
}

using System;
using System.Collections.Generic;

namespace YngveHestem.GenericParameterCollection.ParameterValueConverters
{
    public class ParameterCollectionParameterConverterForParameterCollection : ParameterCollectionParameterConverter<ParameterCollection>
    {
        protected override bool CanConvertFromListOfParameterCollection(IEnumerable<ParameterCollection> value)
        {
            return true;
        }

        protected override bool CanConvertFromParameterCollection(ParameterCollection value)
        {
            return true;
        }

        protected override bool CanConvertToListOfParameterCollection(IEnumerable<ParameterCollection> value)
        {
            return true;
        }

        protected override bool CanConvertToParameterCollection(ParameterCollection value)
        {
            return true;
        }

        protected override IEnumerable<ParameterCollection> ConvertFromListOfParameterCollection(IEnumerable<ParameterCollection> value)
        {
            return value;
        }

        protected override ParameterCollection ConvertFromParameterCollection(ParameterCollection value)
        {
            return value;
        }

        protected override IEnumerable<ParameterCollection> ConvertToListOfParameterCollection(IEnumerable<ParameterCollection> value)
        {
            return value;
        }

        protected override ParameterCollection ConvertToParameterCollection(ParameterCollection value)
        {
            return value;
        }
    }
}


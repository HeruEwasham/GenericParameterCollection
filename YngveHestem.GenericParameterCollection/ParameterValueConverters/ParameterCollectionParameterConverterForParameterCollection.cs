﻿using System;
using System.Collections.Generic;

namespace YngveHestem.GenericParameterCollection.ParameterValueConverters
{
    public class ParameterCollectionParameterConverterForParameterCollection : ParameterCollectionParameterConverter<ParameterCollection>
    {
        protected override bool CanConvertFromListOfParameterCollection(IEnumerable<ParameterCollection> value, ParameterCollection additionalInfo, IEnumerable<IParameterValueConverter> customConverters)
        {
            return true;
        }

        protected override bool CanConvertFromParameterCollection(ParameterCollection value, ParameterCollection additionalInfo, IEnumerable<IParameterValueConverter> customConverters)
        {
            return true;
        }

        protected override bool CanConvertToListOfParameterCollection(IEnumerable<ParameterCollection> value, ParameterCollection additionalInfo, IEnumerable<IParameterValueConverter> customConverters)
        {
            return true;
        }

        protected override bool CanConvertToParameterCollection(ParameterCollection value, ParameterCollection additionalInfo, IEnumerable<IParameterValueConverter> customConverters)
        {
            return true;
        }

        protected override IEnumerable<ParameterCollection> ConvertFromListOfParameterCollection(IEnumerable<ParameterCollection> value, ParameterCollection additionalInfo, IEnumerable<IParameterValueConverter> customConverters)
        {
            return value;
        }

        protected override ParameterCollection ConvertFromParameterCollection(ParameterCollection value, ParameterCollection additionalInfo, IEnumerable<IParameterValueConverter> customConverters)
        {
            return value;
        }

        protected override IEnumerable<ParameterCollection> ConvertToListOfParameterCollection(IEnumerable<ParameterCollection> value, ParameterCollection additionalInfo, IEnumerable<IParameterValueConverter> customConverters)
        {
            return value;
        }

        protected override ParameterCollection ConvertToParameterCollection(ParameterCollection value, ParameterCollection additionalInfo, IEnumerable<IParameterValueConverter> customConverters)
        {
            return value;
        }
    }
}


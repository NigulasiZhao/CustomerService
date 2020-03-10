using Abp.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace AfarsoftResourcePlan.Json
{
    public class AfarsoftResourcePlanContractResolver: DefaultContractResolver
    {
        protected override JsonContract CreateContract(Type objectType)
        {
            JsonContract jsonContract = base.CreateContract(objectType);
            if (objectType == typeof(DateTime) || objectType == typeof(DateTime?))
            {
                jsonContract.Converter = new AbpDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" };
            }
            return jsonContract;
        }
    }
}

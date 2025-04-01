using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadLike.Common.Application.DapperTypeHandlers
{
	public class JsonTypeHandler<T> : Dapper.SqlMapper.TypeHandler<T> where T : class
	{
		public override T? Parse(object value)
		{
			// in postgres or sqlserver all these field are just string
			if(value != null)
			{
				return JsonConvert.DeserializeObject<T>((string)value);
			}
			return null;
		}

		public override void SetValue(IDbDataParameter parameter, T? value)
		{
			if(value != null)
				parameter.Value = JsonConvert.SerializeObject(value);
			else 
				parameter.Value = value;
		}
	}
}

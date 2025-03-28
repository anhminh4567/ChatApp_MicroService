using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadLike.Common.Application.Dtos
{
	public class PagingResponse<T> 
	{
		public int CurrentPage { get; set; }
		public int TotalPages { get; set; }
		public int PageSize { get; set; }
		public List<T> Values { get; set; } = new();
	}
}

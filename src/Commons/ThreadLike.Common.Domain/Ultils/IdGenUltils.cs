using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadLike.Common.Domain.Ultils
{
	public static class IdGenUltils
	{
		public static string GetIdGen(int length)
		{
			if (length > 40 || length < 1)
			{
				throw new ArgumentException("Length must be between 1 and 40");
			}
			const string allowedGen = "qwertyuiopasdfghjklzxcvbnm1234567890QWERTYUIOPASSDFGHJKLZXCVBNM";
			Random random = new Random();
			char[] id = new char[length];
			for (int i = 0; i < length; i++)
			{
				id[i] = allowedGen[random.Next(0, allowedGen.Length)];
			}
			return new string(id);
		}
	}
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RichMail.Interfaces
{
	public interface IHaveHeaders
	{
		IDictionary<string, string> Headers { get; }
	}
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClientQUIZ
{
	public class Question
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int Theme { get; set; }
		public string Answer { get; set; }
	}
}
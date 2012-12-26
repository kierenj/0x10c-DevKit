using System;

namespace SmartAssembly.SmartExceptionsCore
{
	internal class ObjectAndType
	{
		private readonly Type m_TypeToInterpret;

		private readonly object m_O;

		private readonly bool m_FirstLevel;

		public bool FirstLevel
		{
			get
			{
				return this.m_FirstLevel;
			}
		}

		public ObjectAndType(object o, bool firstLevel) : this(o, (o != null ? o.GetType() : null), firstLevel)
		{
		}

		public ObjectAndType(object o, Type t, bool firstLevel)
		{
			this.m_O = o;
			this.m_TypeToInterpret = t;
			this.m_FirstLevel = firstLevel;
		}

		public object GetObject()
		{
			return this.m_O;
		}

		public Type GetType()
		{
			return this.m_TypeToInterpret;
		}
	}
}